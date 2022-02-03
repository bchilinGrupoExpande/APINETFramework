using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APIPresupuestos.Models;
using ConectarDatos;


namespace APIPresupuestos.Controllers
{
    public class IndicadoresCostosController : BaseController
    {
        [HttpPost]
        public Reply ListarCostoOperativo([FromBody] IndicadoresViewModel model)
        {
            Reply oR = new Reply();
            oR.result = 0;
            if (!Verify(model.token, model.IDUsuario))
            {
                oR.message = "No autorizado, idenfiquese para realizar las peticiones";
                return oR;
            }
            try
            {

                using (PresupuestosPruebaEntities db = new PresupuestosPruebaEntities())
                {
                    var codigo_pais = db.paises.Where(d => d.id == model.id_pais).First();
                    var codigo_pais_valor = codigo_pais.nombre;

                    List<IndicadorCostoOperativoViewModel> lst_indicador_costo_operativo = new List<IndicadorCostoOperativoViewModel>();
                    //Pais CEAM
                    if (model.id_pais == 1)
                    {
                        var presupuestos_gastos = (from ta in db.presupuestos
                                                   where ta.estado == 1
                                                   select ta.anio
                                                           ).Distinct().ToList();

                        //Se seleccionan todos los presupuestos de Ventas del País Seleeccionado

                        foreach (var dto in presupuestos_gastos)
                        {



                            //Se buscan los valores de la Venta del Presupuesto
                            var lst_reporte_ventas_presupuesto_1 = new List<sp_areas_equivalentes_valores_presupuesto_Result>();
                            //var lst_reporte_ventas_presupuesto_1 = db.sp_valores_presupuesto_ventas_periodos(dto.id, tipo_cambio_presupuesto_1, "1,2,3,4,5,6,7,8,9,10,11,12");

                            //Si el tipo de Reporte es 0 Buscar el reporte para el presupuesto y país seleccionado
                            if (model.id_tipo_datos_1 == 0)
                            {
                                var listado_presupuesto_ventas_anio = db.presupuestos.Where(d => d.anio == dto && d.estado == 1 && d.id_tipo_presupuesto == 1).ToList();
                                var string_presupuesto_ventas_1 = "";
                                foreach (var dto_p in listado_presupuesto_ventas_anio)
                                {
                                    var id_str = Convert.ToString(dto_p.id);
                                    string_presupuesto_ventas_1 = string_presupuesto_ventas_1 + id_str + ",";
                                }

                                if (string_presupuesto_ventas_1.Count() >= 1)
                                {
                                    string_presupuesto_ventas_1 = string_presupuesto_ventas_1.Remove(string_presupuesto_ventas_1.Length - 1);
                                }
                                lst_reporte_ventas_presupuesto_1 = db.sp_areas_equivalentes_valores_presupuesto_periodos_CEAM(string_presupuesto_ventas_1, "1,2,3,4,5,6,7,8,9,10,11,12").ToList();
                            }
                            //Valida si busca el Ejecutado
                            if (model.id_tipo_datos_1 == 1)
                            {
                                lst_reporte_ventas_presupuesto_1 = db.total_Ejecutado_Por_Area_Equivalente("ceam", dto, "1,2,3,4,5,6,7,8,9,10,11,12", 1).ToList();
                            }

                            double? Comercial_P1 = 0.0;
                            double? Logistica_P1 = 0.0;
                            double? Compras_P1 = 0.0;
                            double? Contabilidad_P1 = 0.0;

                            double? Creditos_P1 = 0.0;
                            double? Gestion_Humana_P1 = 0.0;
                            double? Mejora_Continua_P1 = 0.0;
                            double? Administracion_P1 = 0.0;
                            double? Tecnologa_de_Informacion_P1 = 0.0;
                            //Calculos Presupuesto de Ventas 1
                            foreach (var dto_venta in lst_reporte_ventas_presupuesto_1)
                            {
                                if (dto_venta.area_equivalente == "Comercial")
                                {
                                    Comercial_P1 = dto_venta.total;
                                }
                                else if (dto_venta.area_equivalente == "Logistica")
                                {
                                    Logistica_P1 = dto_venta.total;
                                }
                                else if (dto_venta.area_equivalente == "Compras")
                                {
                                    Compras_P1 = dto_venta.total;
                                }
                                else if (dto_venta.area_equivalente == "Contabilidad")
                                {
                                    Contabilidad_P1 = dto_venta.total;
                                }
                                else if (dto_venta.area_equivalente == "Creditos")
                                {
                                    Creditos_P1 = dto_venta.total;
                                }
                                else if (dto_venta.area_equivalente == "Gestion Humana")
                                {
                                    Gestion_Humana_P1 = dto_venta.total;
                                }
                                else if (dto_venta.area_equivalente == "Mejora Continua")
                                {
                                    Mejora_Continua_P1 = dto_venta.total;
                                }
                                else if (dto_venta.area_equivalente == "Administracion")
                                {
                                    Administracion_P1 = dto_venta.total;
                                }
                                else if (dto_venta.area_equivalente == "Tecnologia de Informacion (TI)")
                                {
                                    Tecnologa_de_Informacion_P1 = dto_venta.total;
                                }

                            }

                            //Se Calcula la Venta Neta =VentaBruta-Bonificaciones-Descuentos
                            var Gastos_de_Distribuidoras_P1 = Logistica_P1 + Comercial_P1;
                            var Gastos_de_Administracion_P1 = Administracion_P1 + Compras_P1 + Contabilidad_P1 + Creditos_P1 + Gestion_Humana_P1 + Mejora_Continua_P1 + Tecnologa_de_Informacion_P1;

                            //Se Calcula el total de la Venta
                            var Costo_Operativo = Convert.ToDouble(Gastos_de_Administracion_P1) + Convert.ToDouble(Gastos_de_Distribuidoras_P1);

                            lst_indicador_costo_operativo.Add(new IndicadorCostoOperativoViewModel()
                            {
                                pais = "CEAM",
                                anio = dto,
                                valor = Math.Round(Costo_Operativo / 1000000, 2),
                                creciminento = 0.0
                            });
                        }

                        if (lst_indicador_costo_operativo.Count() == 0)
                        {
                            lst_indicador_costo_operativo.Add(new IndicadorCostoOperativoViewModel()
                            {
                                pais = "--N/A--",
                                anio = 0,
                                valor = 0.0,
                                creciminento = 0.0
                            });
                        }
                    }
                    //Pais Espeficico
                    else
                    {
                        //Se seleccionan todos los presupuestos de Ventas del País Seleeccionado
                        var presupuestos_gastos = db.presupuestos.Where(d => d.estado == 1 && d.id_tipo_presupuesto == 1 && d.id_pais == model.id_pais).ToList();

                        foreach (var dto in presupuestos_gastos)
                        {

                            double tipo_cambio_presupuesto_1 = 1;
                            var lst_moneda_presupuesto_1 = db.monedas_presupuesto.Where(d => d.id_presupuesto_asignado == dto.id).First();
                            tipo_cambio_presupuesto_1 = Convert.ToDouble(lst_moneda_presupuesto_1.tasa_cambio_presupuesto);
                            var anio_presupuesto_1 = dto.anio;
                            var id_pais_presupuesto_1 = dto.id_pais;


                            //Se buscan los valores de la Venta del Presupuesto
                            var lst_reporte_ventas_presupuesto_1 = new List<sp_areas_equivalentes_valores_presupuesto_Result>();
                            //var lst_reporte_ventas_presupuesto_1 = db.sp_valores_presupuesto_ventas_periodos(dto.id, tipo_cambio_presupuesto_1, "1,2,3,4,5,6,7,8,9,10,11,12");

                            //Si el tipo de Reporte es 0 Buscar el reporte para el presupuesto y país seleccionado
                            if (model.id_tipo_datos_1 == 0)
                            {
                                lst_reporte_ventas_presupuesto_1 = db.sp_areas_equivalentes_valores_presupuesto_periodos(dto.id, tipo_cambio_presupuesto_1, "1,2,3,4,5,6,7,8,9,10,11,12").ToList();
                            }
                            //Valida si busca el Ejecutado
                            if (model.id_tipo_datos_1 == 1)
                            {
                                var datos_presupuesto = db.presupuestos.Where(d => d.id == dto.id).First();
                                var ejer = datos_presupuesto.anio;
                                lst_reporte_ventas_presupuesto_1 = db.total_Ejecutado_Por_Area_Equivalente(codigo_pais_valor, ejer, "1,2,3,4,5,6,7,8,9,10,11,12", 1).ToList();
                            }

                            double? Comercial_P1 = 0.0;
                            double? Logistica_P1 = 0.0;
                            double? Compras_P1 = 0.0;
                            double? Contabilidad_P1 = 0.0;

                            double? Creditos_P1 = 0.0;
                            double? Gestion_Humana_P1 = 0.0;
                            double? Mejora_Continua_P1 = 0.0;
                            double? Administracion_P1 = 0.0;
                            double? Tecnologa_de_Informacion_P1 = 0.0;
                            //Calculos Presupuesto de Ventas 1
                            foreach (var dto_venta in lst_reporte_ventas_presupuesto_1)
                            {
                                if (dto_venta.area_equivalente == "Comercial")
                                {
                                    Comercial_P1 = dto_venta.total;
                                }
                                else if (dto_venta.area_equivalente == "Logistica")
                                {
                                    Logistica_P1 = dto_venta.total;
                                }
                                else if (dto_venta.area_equivalente == "Compras")
                                {
                                    Compras_P1 = dto_venta.total;
                                }
                                else if (dto_venta.area_equivalente == "Contabilidad")
                                {
                                    Contabilidad_P1 = dto_venta.total;
                                }
                                else if (dto_venta.area_equivalente == "Creditos")
                                {
                                    Creditos_P1 = dto_venta.total;
                                }
                                else if (dto_venta.area_equivalente == "Gestion Humana")
                                {
                                    Gestion_Humana_P1 = dto_venta.total;
                                }
                                else if (dto_venta.area_equivalente == "Mejora Continua")
                                {
                                    Mejora_Continua_P1 = dto_venta.total;
                                }
                                else if (dto_venta.area_equivalente == "Administracion")
                                {
                                    Administracion_P1 = dto_venta.total;
                                }
                                else if (dto_venta.area_equivalente == "Tecnologia de Informacion (TI)")
                                {
                                    Tecnologa_de_Informacion_P1 = dto_venta.total;
                                }

                            }

                            //Se Calcula la Venta Neta =VentaBruta-Bonificaciones-Descuentos
                            var Gastos_de_Distribuidoras_P1 = Logistica_P1 + Comercial_P1;
                            var Gastos_de_Administracion_P1 = Administracion_P1 + Compras_P1 + Contabilidad_P1 + Creditos_P1 + Gestion_Humana_P1 + Mejora_Continua_P1 + Tecnologa_de_Informacion_P1;

                            //Se Calcula el total de la Venta
                            var Costo_Operativo = Convert.ToDouble(Gastos_de_Administracion_P1) + Convert.ToDouble(Gastos_de_Distribuidoras_P1);

                            lst_indicador_costo_operativo.Add(new IndicadorCostoOperativoViewModel()
                            {
                                pais = dto.pais.nombre,
                                anio = dto.anio,
                                valor = Math.Round(Costo_Operativo / 1000000, 2),
                                creciminento = 0.0
                            });
                        }

                        if (lst_indicador_costo_operativo.Count() == 0)
                        {
                            lst_indicador_costo_operativo.Add(new IndicadorCostoOperativoViewModel()
                            {
                                pais = "--N/A--",
                                anio = 0,
                                valor = 0.0,
                                creciminento=0.0
                            });
                        }
                    }

                    lst_indicador_costo_operativo = lst_indicador_costo_operativo.OrderBy(d => d.anio).Take(3).ToList();
                    for (var i = 1; i < lst_indicador_costo_operativo.Count; i++)
                    {
                        lst_indicador_costo_operativo[i].creciminento = ((Convert.ToDouble(lst_indicador_costo_operativo[i].valor) - Convert.ToDouble(lst_indicador_costo_operativo[i - 1].valor)) / Convert.ToDouble(lst_indicador_costo_operativo[i].valor)) * 100;
                    }
                    oR.result = 1;
                    oR.data = lst_indicador_costo_operativo;
                }
            }
            catch (Exception ex)
            {
                oR.message = ex.ToString();
            }
            return oR;
        }

        [HttpPut]
        public Reply ListaResultadoNeto([FromBody] IndicadoresViewModel model)
        {
            Reply oR = new Reply();
            oR.result = 0;
            if (!Verify(model.token, model.IDUsuario))
            {
                oR.message = "No autorizado, idenfiquese para realizar las peticiones";
                return oR;
            }
            try
            {

                using (PresupuestosPruebaEntities db = new PresupuestosPruebaEntities())
                {
                    var codigo_pais = db.paises.Where(d => d.id == model.id_pais).First();
                    var codigo_pais_valor = codigo_pais.nombre;

                    List<IndicadorResultadoNetoViewModel> lst_indicador_resultado_neto = new List<IndicadorResultadoNetoViewModel>();
                    //Pais CEAM
                    if (model.id_pais == 1)
                    {
                        var presupuestos_gastos = (from ta in db.presupuestos
                                                   where ta.estado == 1
                                                   select ta.anio
                                                           ).Distinct().ToList();

                        //Se seleccionan todos los presupuestos de Ventas del País Seleeccionado

                        foreach (var dto in presupuestos_gastos)
                        {


                                var lst_venta = new List<sp_valores_presupuesto_ventas_Result>();
                                //var lst_reporte_ventas_presupuesto_1 = db.sp_valores_presupuesto_ventas_periodos(dto.id, tipo_cambio_presupuesto_1, "1,2,3,4,5,6,7,8,9,10,11,12");

                                //Si el tipo de Reporte es 0 Buscar el reporte para el presupuesto y país seleccionado
                                if (model.id_tipo_datos_1 == 0)
                                {
                                    var listado_presupuesto_ventas_anio = db.presupuestos.Where(d => d.anio == dto && d.estado == 1 && d.id_tipo_presupuesto == 3).ToList();
                                    var string_presupuesto_ventas_1 = "";
                                    foreach (var dto_p in listado_presupuesto_ventas_anio)
                                    {
                                        var id_str = Convert.ToString(dto_p.id);
                                        string_presupuesto_ventas_1 = string_presupuesto_ventas_1 + id_str + ",";
                                    }

                                if (string_presupuesto_ventas_1.Count() > 0)
                                {
                                    string_presupuesto_ventas_1 = string_presupuesto_ventas_1.Remove(string_presupuesto_ventas_1.Length - 1);
                                }

                                lst_venta = db.sp_valores_presupuesto_ventas_periodos_CEAM(string_presupuesto_ventas_1, "1,2,3,4,5,6,7,8,9,10,11,12").ToList();
                                }
                                //Valida si busca el Ejecutado
                                if (model.id_tipo_datos_1 == 1)
                                {
                                    lst_venta = db.total_Ejecutado_Por_Cuentas_de_Ventas("ceam", dto, "1,2,3,4,5,6,7,8,9,10,11,12", 1, "HIDRISAGE,ICLOS,MEDIHEALTH,PANALAB,POEN,ROE,ROWE", "ALMIRAL,SPEN,FERSON,GRA,GRH").ToList();
                                }

                                var Venta_Bruta_Propios = 0.0;
                                var Bonificaciones_Propios = 0.0;
                                var Descuentos_Propios = 0.0;
                                var Devoluciones_Propios = 0.0;

                                var Venta_Bruta_Representados = 0.0;
                                var Bonificaciones_Representados = 0.0;
                                var Descuentos_Reperesentado = 0.0;
                                var Devoluciones_Representados = 0.0;
                                var CostoVentasLaboratiosRepresentados = 0.0;
                                var Margen_de_Contribución_Free_P1 = 0.0;
                                //Calculos Presupuesto de Ventas 1
                                foreach (var dto_venta in lst_venta)
                                {

                                    //Venta Bruta
                                    if (dto_venta.codigo_linea == "Laboratorios Propios" && dto_venta.cuenta_contable == "41101")
                                    {
                                        Venta_Bruta_Propios = Math.Abs(Convert.ToDouble(dto_venta.total));
                                    }
                                    if (dto_venta.codigo_linea == "Laboratorios Representados" && dto_venta.cuenta_contable == "41101")
                                    {
                                        Venta_Bruta_Representados = Math.Abs(Convert.ToDouble(dto_venta.total));
                                    }

                                    //Bonificaciones
                                    if (dto_venta.codigo_linea == "Laboratorios Propios" && dto_venta.cuenta_contable == "41203")
                                    {
                                        Bonificaciones_Propios = Math.Abs(Convert.ToDouble(dto_venta.total));
                                    }
                                    if (dto_venta.codigo_linea == "Laboratorios Representados" && dto_venta.cuenta_contable == "41203")
                                    {
                                        Bonificaciones_Representados = Math.Abs(Convert.ToDouble(dto_venta.total));
                                    }

                                    //Descuentos
                                    if (dto_venta.codigo_linea == "Laboratorios Propios" && dto_venta.cuenta_contable == "41202")
                                    {
                                        Descuentos_Propios = Math.Abs(Convert.ToDouble(dto_venta.total));
                                    }
                                    if (dto_venta.codigo_linea == "Laboratorios Representados" && dto_venta.cuenta_contable == "41202")
                                    {
                                        Descuentos_Reperesentado = Math.Abs(Convert.ToDouble(dto_venta.total));
                                    }

                                    //Devoluciones
                                    if (dto_venta.codigo_linea == "Laboratorios Propios" && dto_venta.cuenta_contable == "41201")
                                    {
                                        Devoluciones_Propios = Math.Abs(Convert.ToDouble(dto_venta.total));
                                    }
                                    if (dto_venta.codigo_linea == "Laboratorios Representados" && dto_venta.cuenta_contable == "41201")
                                    {
                                        Devoluciones_Representados = Math.Abs(Convert.ToDouble(dto_venta.total));
                                    }

                                    //Calculo del Margen de Contribucion Terceros
                                    if (dto_venta.codigo_linea == "Laboratorios Representados" && dto_venta.cuenta_contable == "51101")
                                    {
                                        CostoVentasLaboratiosRepresentados = Math.Abs(Convert.ToDouble(dto_venta.total));
                                    }
                                    if (dto_venta.codigo_linea == "Laboratorios Propios" && dto_venta.cuenta_contable == "91110")
                                    {
                                        Margen_de_Contribución_Free_P1 = Math.Abs(Convert.ToDouble(dto_venta.total));
                                    }
                                }

                                //Se Calcula la Venta Neta =VentaBruta-Bonificaciones-Descuentos
                                var Venta_Neta_Laboratorios_Propios_1 = Venta_Bruta_Propios - Descuentos_Propios - Devoluciones_Propios - Bonificaciones_Propios;
                                var Venta_Laboratorios_Representados_P1 = Venta_Bruta_Representados - Descuentos_Reperesentado - Bonificaciones_Representados - Devoluciones_Representados;
                                var Margen_de_Contribución_Tercero_P1 = Venta_Laboratorios_Representados_P1 - CostoVentasLaboratiosRepresentados;
                                var Total_Utilidad_Bruta_P1 = Margen_de_Contribución_Tercero_P1 + Margen_de_Contribución_Free_P1;
                                //Se Calcula el total de la Venta
                                var Venta_Local_P1 = Convert.ToDouble(Venta_Neta_Laboratorios_Propios_1) + Convert.ToDouble(Venta_Laboratorios_Representados_P1);


                                //Se buscan los valores de la Venta del Presupuesto
                                var lst_areas_equivalente = new List<sp_areas_equivalentes_valores_presupuesto_Result>();
                                //var lst_reporte_ventas_presupuesto_1 = db.sp_valores_presupuesto_ventas_periodos(dto.id, tipo_cambio_presupuesto_1, "1,2,3,4,5,6,7,8,9,10,11,12");

                                //Si el tipo de Reporte es 0 Buscar el reporte para el presupuesto y país seleccionado
                                if (model.id_tipo_datos_1 == 0)
                                {
                                var listado_presupuesto_ventas_anio = db.presupuestos.Where(d => d.anio == dto && d.estado == 1 && d.id_tipo_presupuesto == 1).ToList();
                                var string_presupuesto_ventas_1 = "";
                                foreach (var dto_p in listado_presupuesto_ventas_anio)
                                {
                                    var id_str = Convert.ToString(dto_p.id);
                                    string_presupuesto_ventas_1 = string_presupuesto_ventas_1 + id_str + ",";
                                }

                                if (string_presupuesto_ventas_1.Count() > 0)
                                {
                                    string_presupuesto_ventas_1 = string_presupuesto_ventas_1.Remove(string_presupuesto_ventas_1.Length - 1);
                                }

                                lst_areas_equivalente = db.sp_areas_equivalentes_valores_presupuesto_periodos_CEAM(string_presupuesto_ventas_1, "1,2,3,4,5,6,7,8,9,10,11,12").ToList();
                                }
                                //Valida si busca el Ejecutado
                                if (model.id_tipo_datos_1 == 1)
                                {
                                    lst_areas_equivalente = db.total_Ejecutado_Por_Area_Equivalente("ceam", dto, "1,2,3,4,5,6,7,8,9,10,11,12", 1).ToList();
                                }

                                double? Comercial_P1 = 0.0;
                                double? Logistica_P1 = 0.0;
                                double? Compras_P1 = 0.0;
                                double? Contabilidad_P1 = 0.0;

                                double? Creditos_P1 = 0.0;
                                double? Gestion_Humana_P1 = 0.0;
                                double? Mejora_Continua_P1 = 0.0;
                                double? Administracion_P1 = 0.0;
                                double? Tecnologa_de_Informacion_P1 = 0.0;
                                //Calculos Presupuesto de Ventas 1
                                foreach (var dto_area in lst_areas_equivalente)
                                {
                                    if (dto_area.area_equivalente == "Comercial")
                                    {
                                        Comercial_P1 = dto_area.total;
                                    }
                                    else if (dto_area.area_equivalente == "Logistica")
                                    {
                                        Logistica_P1 = dto_area.total;
                                    }
                                    else if (dto_area.area_equivalente == "Compras")
                                    {
                                        Compras_P1 = dto_area.total;
                                    }
                                    else if (dto_area.area_equivalente == "Contabilidad")
                                    {
                                        Contabilidad_P1 = dto_area.total;
                                    }
                                    else if (dto_area.area_equivalente == "Creditos")
                                    {
                                        Creditos_P1 = dto_area.total;
                                    }
                                    else if (dto_area.area_equivalente == "Gestion Humana")
                                    {
                                        Gestion_Humana_P1 = dto_area.total;
                                    }
                                    else if (dto_area.area_equivalente == "Mejora Continua")
                                    {
                                        Mejora_Continua_P1 = dto_area.total;
                                    }
                                    else if (dto_area.area_equivalente == "Administracion")
                                    {
                                        Administracion_P1 = dto_area.total;
                                    }
                                    else if (dto_area.area_equivalente == "Tecnologia de Informacion (TI)")
                                    {
                                        Tecnologa_de_Informacion_P1 = dto_area.total;
                                    }

                                }

                                //Se Calcula la Venta Neta =VentaBruta-Bonificaciones-Descuentos
                                var Gastos_de_Distribuidoras_P1 = Logistica_P1 + Comercial_P1;
                                var Gastos_de_Administracion_P1 = Administracion_P1 + Compras_P1 + Contabilidad_P1 + Creditos_P1 + Gestion_Humana_P1 + Mejora_Continua_P1 + Tecnologa_de_Informacion_P1;

                                //Se Calcula el total de la Venta

                                var lst_reporte_1_rubros = new List<sp_rubro_corporativos_valores_presupuesto_Result>();

                                if (model.id_tipo_datos_1 == 0)
                                {
                                var listado_presupuesto_ventas_anio = db.presupuestos.Where(d => d.anio == dto && d.estado == 1 && d.id_tipo_presupuesto == 1).ToList();
                                var string_presupuesto_ventas_1 = "";
                                foreach (var dto_p in listado_presupuesto_ventas_anio)
                                {
                                    var id_str = Convert.ToString(dto_p.id);
                                    string_presupuesto_ventas_1 = string_presupuesto_ventas_1 + id_str + ",";
                                }
                                if (string_presupuesto_ventas_1.Count() > 0)
                                {
                                    string_presupuesto_ventas_1 = string_presupuesto_ventas_1.Remove(string_presupuesto_ventas_1.Length - 1);
                                }
                               
                                lst_reporte_1_rubros = db.sp_rubro_corporativos_valores_presupuesto_periodos_CEAM(string_presupuesto_ventas_1, "1,2,3,4,5,6,7,8,9,10,11,12").ToList();
                                }
                                //Valida si busca el Ejecutado
                                if (model.id_tipo_datos_1 == 1)
                                {
                                    lst_reporte_1_rubros = db.total_Ejecutado_Por_Rubro_Corporativo("ceam", dto, "1,2,3,4,5,6,7,8,9,10,11,12", 1).ToList();
                                }

                                var Intereses_Ganados_P1 = 0.0;
                                var Intereses_Perdidos_P1 = 0.0;
                                var Otros_Resultados_Financieros_P1 = 0.0;
                                var Result_Ejercicios_Anteriores_P1 = 0.0;

                                var Resultados_Diversos_P1 = 0.0;
                                var Gan__por_Conversion_P1 = 0.0;
                                var Impuesto_a_la_Renta_P1 = 0.0;

                                foreach (var dto2 in lst_reporte_1_rubros)
                                {
                                    if (dto2.rubro == "C06")
                                    {
                                        Intereses_Ganados_P1 = Convert.ToDouble(dto2.total * -1);
                                    }
                                    else if (dto2.rubro == "C07")
                                    {
                                        Intereses_Perdidos_P1 = Convert.ToDouble(dto2.total * -1);
                                    }
                                    else if (dto2.rubro == "C08")
                                    {
                                        Otros_Resultados_Financieros_P1 = Convert.ToDouble(dto2.total * -1);
                                    }
                                    else if (dto2.rubro == "C09")
                                    {
                                        Result_Ejercicios_Anteriores_P1 = Convert.ToDouble(dto2.total * -1);
                                    }
                                    else if (dto2.rubro == "C10")
                                    {
                                        Resultados_Diversos_P1 = Convert.ToDouble(dto2.total * -1);
                                    }
                                    else if (dto2.rubro == "C11")
                                    {
                                        Gan__por_Conversion_P1 = Convert.ToDouble(dto2.total * -1);
                                    }
                                    else if (dto2.rubro == "C12")
                                    {
                                        Impuesto_a_la_Renta_P1 = Convert.ToDouble(dto2.total * -1);
                                    }
                                }
                                var Resultado_de_Explotacion_P1 = Total_Utilidad_Bruta_P1 - Gastos_de_Administracion_P1 - Gastos_de_Distribuidoras_P1;
                                var Resultados_Financieros_P1 = Intereses_Ganados_P1 + Intereses_Perdidos_P1 + Otros_Resultados_Financieros_P1;
                                var Resultado_antes_de_impuestos_P1 = Resultado_de_Explotacion_P1 + Resultados_Financieros_P1 + Result_Ejercicios_Anteriores_P1 + Resultados_Diversos_P1 + Gan__por_Conversion_P1;
                                var Resultado_Neto_P1 = Resultado_antes_de_impuestos_P1 + Impuesto_a_la_Renta_P1;

                                lst_indicador_resultado_neto.Add(new IndicadorResultadoNetoViewModel()
                                {
                                    pais = "CEAM",
                                    anio = dto,
                                    valor = Math.Round(Convert.ToDouble(Resultado_Neto_P1) / 1000000, 2),
                                    creciminento = 0.0,
                                });
                            }

                            if (lst_indicador_resultado_neto.Count() == 0)
                            {
                                lst_indicador_resultado_neto.Add(new IndicadorResultadoNetoViewModel()
                                {
                                    pais = "--N/A--",
                                    anio = 0,
                                    valor = 0.0,
                                    creciminento = 0.0,
                                });
                            }
                        }

                    //Pais Espeficico
                    else
                    {
                        //Se seleccionan todos los presupuestos de Ventas del País Seleeccionado
                        var presupuestos_gastos = db.presupuestos.Where(d => d.estado == 1 && d.id_tipo_presupuesto == 1 && d.id_pais == model.id_pais).ToList();

                        foreach (var dto in presupuestos_gastos)
                        {

                            double tipo_cambio_presupuesto_1 = 1;
                            var lst_moneda_presupuesto_1 = db.monedas_presupuesto.Where(d => d.id_presupuesto_asignado == dto.id).First();
                            tipo_cambio_presupuesto_1 = Convert.ToDouble(lst_moneda_presupuesto_1.tasa_cambio_presupuesto);
                            var anio_presupuesto_1 = dto.anio;
                            var id_pais_presupuesto_1 = dto.id_pais;


                            var lst_venta = new List<sp_valores_presupuesto_ventas_Result>();
                            //var lst_reporte_ventas_presupuesto_1 = db.sp_valores_presupuesto_ventas_periodos(dto.id, tipo_cambio_presupuesto_1, "1,2,3,4,5,6,7,8,9,10,11,12");

                            //Si el tipo de Reporte es 0 Buscar el reporte para el presupuesto y país seleccionado
                            if (model.id_tipo_datos_1 == 0)
                            {
                                var lst_presupuesto_ventas_1 = db.presupuestos.Where(d => d.anio == anio_presupuesto_1 && d.id_pais == id_pais_presupuesto_1 && d.id_tipo_presupuesto == 3 && d.estado == 1).First();
                                lst_venta = db.sp_valores_presupuesto_ventas_periodos(lst_presupuesto_ventas_1.id, tipo_cambio_presupuesto_1, "1,2,3,4,5,6,7,8,9,10,11,12").ToList();
                            }
                            //Valida si busca el Ejecutado
                            if (model.id_tipo_datos_1 == 1)
                            {
                                var datos_presupuesto = db.presupuestos.Where(d => d.id == dto.id).First();
                                var ejer = datos_presupuesto.anio;
                                lst_venta = db.total_Ejecutado_Por_Cuentas_de_Ventas(codigo_pais_valor, ejer, "1,2,3,4,5,6,7,8,9,10,11,12", 1,"HIDRISAGE,ICLOS,MEDIHEALTH,PANALAB,POEN,ROE,ROWE", "ALMIRAL,SPEN,FERSON,GRA,GRH").ToList();
                            }



                            var Venta_Bruta_Propios = 0.0;
                            var Bonificaciones_Propios = 0.0;
                            var Descuentos_Propios = 0.0;
                            var Devoluciones_Propios = 0.0;

                            var Venta_Bruta_Representados = 0.0;
                            var Bonificaciones_Representados = 0.0;
                            var Descuentos_Reperesentado = 0.0;
                            var Devoluciones_Representados = 0.0;
                            var CostoVentasLaboratiosRepresentados = 0.0;
                            var Margen_de_Contribución_Free_P1 = 0.0;
                            //Calculos Presupuesto de Ventas 1
                            foreach (var dto_venta in lst_venta)
                            {

                                //Venta Bruta
                                if (dto_venta.codigo_linea == "Laboratorios Propios" && dto_venta.cuenta_contable == "41101")
                                {
                                    Venta_Bruta_Propios = Math.Abs(Convert.ToDouble(dto_venta.total));
                                }
                                if (dto_venta.codigo_linea == "Laboratorios Representados" && dto_venta.cuenta_contable == "41101")
                                {
                                    Venta_Bruta_Representados = Math.Abs(Convert.ToDouble(dto_venta.total));
                                }

                                //Bonificaciones
                                if (dto_venta.codigo_linea == "Laboratorios Propios" && dto_venta.cuenta_contable == "41203")
                                {
                                    Bonificaciones_Propios = Math.Abs(Convert.ToDouble(dto_venta.total));
                                }
                                if (dto_venta.codigo_linea == "Laboratorios Representados" && dto_venta.cuenta_contable == "41203")
                                {
                                    Bonificaciones_Representados = Math.Abs(Convert.ToDouble(dto_venta.total));
                                }

                                //Descuentos
                                if (dto_venta.codigo_linea == "Laboratorios Propios" && dto_venta.cuenta_contable == "41202")
                                {
                                    Descuentos_Propios = Math.Abs(Convert.ToDouble(dto_venta.total));
                                }
                                if (dto_venta.codigo_linea == "Laboratorios Representados" && dto_venta.cuenta_contable == "41202")
                                {
                                    Descuentos_Reperesentado = Math.Abs(Convert.ToDouble(dto_venta.total));
                                }

                                //Devoluciones
                                if (dto_venta.codigo_linea == "Laboratorios Propios" && dto_venta.cuenta_contable == "41201")
                                {
                                    Devoluciones_Propios = Math.Abs(Convert.ToDouble(dto_venta.total));
                                }
                                if (dto_venta.codigo_linea == "Laboratorios Representados" && dto_venta.cuenta_contable == "41201")
                                {
                                    Devoluciones_Representados = Math.Abs(Convert.ToDouble(dto_venta.total));
                                }

                                //Calculo del Margen de Contribucion Terceros
                                if (dto_venta.codigo_linea == "Laboratorios Representados" && dto_venta.cuenta_contable == "51101")
                                {
                                    CostoVentasLaboratiosRepresentados = Math.Abs(Convert.ToDouble(dto_venta.total));
                                }
                                if (dto_venta.codigo_linea == "Laboratorios Propios" && dto_venta.cuenta_contable == "91110")
                                {
                                    Margen_de_Contribución_Free_P1 = Math.Abs(Convert.ToDouble(dto_venta.total));
                                }
                            }

                            //Se Calcula la Venta Neta =VentaBruta-Bonificaciones-Descuentos
                            var Venta_Neta_Laboratorios_Propios_1 = Venta_Bruta_Propios - Descuentos_Propios - Devoluciones_Propios - Bonificaciones_Propios;
                            var Venta_Laboratorios_Representados_P1 = Venta_Bruta_Representados - Descuentos_Reperesentado - Bonificaciones_Representados - Devoluciones_Representados;
                            var Margen_de_Contribución_Tercero_P1 = Venta_Laboratorios_Representados_P1 - CostoVentasLaboratiosRepresentados;
                            var Total_Utilidad_Bruta_P1 = Margen_de_Contribución_Tercero_P1 + Margen_de_Contribución_Free_P1;
                            //Se Calcula el total de la Venta
                            var Venta_Local_P1 = Convert.ToDouble(Venta_Neta_Laboratorios_Propios_1) + Convert.ToDouble(Venta_Laboratorios_Representados_P1);


                            //Se buscan los valores de la Venta del Presupuesto
                            var lst_areas_equivalente = new List<sp_areas_equivalentes_valores_presupuesto_Result>();
                            //var lst_reporte_ventas_presupuesto_1 = db.sp_valores_presupuesto_ventas_periodos(dto.id, tipo_cambio_presupuesto_1, "1,2,3,4,5,6,7,8,9,10,11,12");

                            //Si el tipo de Reporte es 0 Buscar el reporte para el presupuesto y país seleccionado
                            if (model.id_tipo_datos_1 == 0)
                            {
                                lst_areas_equivalente = db.sp_areas_equivalentes_valores_presupuesto_periodos(dto.id, tipo_cambio_presupuesto_1, "1,2,3,4,5,6,7,8,9,10,11,12").ToList();
                            }
                            //Valida si busca el Ejecutado
                            if (model.id_tipo_datos_1 == 1)
                            {
                                var datos_presupuesto = db.presupuestos.Where(d => d.id == dto.id).First();
                                var ejer = datos_presupuesto.anio;
                                lst_areas_equivalente = db.total_Ejecutado_Por_Area_Equivalente(codigo_pais_valor, ejer, "1,2,3,4,5,6,7,8,9,10,11,12", 1).ToList();
                            }

                            double? Comercial_P1 = 0.0;
                            double? Logistica_P1 = 0.0;
                            double? Compras_P1 = 0.0;
                            double? Contabilidad_P1 = 0.0;

                            double? Creditos_P1 = 0.0;
                            double? Gestion_Humana_P1 = 0.0;
                            double? Mejora_Continua_P1 = 0.0;
                            double? Administracion_P1 = 0.0;
                            double? Tecnologa_de_Informacion_P1 = 0.0;
                            //Calculos Presupuesto de Ventas 1
                            foreach (var dto_area in lst_areas_equivalente)
                            {
                                if (dto_area.area_equivalente == "Comercial")
                                {
                                    Comercial_P1 = dto_area.total;
                                }
                                else if (dto_area.area_equivalente == "Logistica")
                                {
                                    Logistica_P1 = dto_area.total;
                                }
                                else if (dto_area.area_equivalente == "Compras")
                                {
                                    Compras_P1 = dto_area.total;
                                }
                                else if (dto_area.area_equivalente == "Contabilidad")
                                {
                                    Contabilidad_P1 = dto_area.total;
                                }
                                else if (dto_area.area_equivalente == "Creditos")
                                {
                                    Creditos_P1 = dto_area.total;
                                }
                                else if (dto_area.area_equivalente == "Gestion Humana")
                                {
                                    Gestion_Humana_P1 = dto_area.total;
                                }
                                else if (dto_area.area_equivalente == "Mejora Continua")
                                {
                                    Mejora_Continua_P1 = dto_area.total;
                                }
                                else if (dto_area.area_equivalente == "Administracion")
                                {
                                    Administracion_P1 = dto_area.total;
                                }
                                else if (dto_area.area_equivalente == "Tecnologia de Informacion (TI)")
                                {
                                    Tecnologa_de_Informacion_P1 = dto_area.total;
                                }

                            }

                            //Se Calcula la Venta Neta =VentaBruta-Bonificaciones-Descuentos
                            var Gastos_de_Distribuidoras_P1 = Logistica_P1 + Comercial_P1;
                            var Gastos_de_Administracion_P1 = Administracion_P1 + Compras_P1 + Contabilidad_P1 + Creditos_P1 + Gestion_Humana_P1 + Mejora_Continua_P1 + Tecnologa_de_Informacion_P1;

                            //Se Calcula el total de la Venta

                            var lst_reporte_1_rubros = new List<sp_rubro_corporativos_valores_presupuesto_Result>();

                            if (model.id_tipo_datos_1 == 0)
                            {
                                lst_reporte_1_rubros = db.sp_rubro_corporativos_valores_presupuesto_periodos(dto.id, tipo_cambio_presupuesto_1, "1,2,3,4,5,6,7,8,9,10,11,12").ToList();
                            }
                            //Valida si busca el Ejecutado
                            if (model.id_tipo_datos_1 == 1)
                            {
                                var datos_presupuesto = db.presupuestos.Where(d => d.id == dto.id).First();
                                var ejer = datos_presupuesto.anio;
                                lst_reporte_1_rubros = db.total_Ejecutado_Por_Rubro_Corporativo(codigo_pais_valor, ejer, "1,2,3,4,5,6,7,8,9,10,11,12", 1).ToList();
                            }

                            var Intereses_Ganados_P1 = 0.0;
                            var Intereses_Perdidos_P1 = 0.0;
                            var Otros_Resultados_Financieros_P1 = 0.0;
                            var Result_Ejercicios_Anteriores_P1 = 0.0;

                            var Resultados_Diversos_P1 = 0.0;
                            var Gan__por_Conversion_P1 = 0.0;
                            var Impuesto_a_la_Renta_P1 = 0.0;

                            foreach (var dto2 in lst_reporte_1_rubros)
                            {
                                if (dto2.rubro == "C06")
                                {
                                    Intereses_Ganados_P1 = Convert.ToDouble(dto2.total * -1);
                                }
                                else if (dto2.rubro == "C07")
                                {
                                   Intereses_Perdidos_P1 = Convert.ToDouble(dto2.total * -1);
                                }
                                else if (dto2.rubro == "C08")
                                {
                                   Otros_Resultados_Financieros_P1 = Convert.ToDouble(dto2.total * -1);
                                }
                                else if (dto2.rubro == "C09")
                                {
                                  Result_Ejercicios_Anteriores_P1 = Convert.ToDouble(dto2.total * -1);
                                }
                                else if (dto2.rubro == "C10")
                                {
                                  Resultados_Diversos_P1 = Convert.ToDouble(dto2.total * -1);
                                }
                                else if (dto2.rubro == "C11")
                                {
                                   Gan__por_Conversion_P1 = Convert.ToDouble(dto2.total * -1);
                                }
                                else if (dto2.rubro == "C12")
                                {
                                  Impuesto_a_la_Renta_P1 = Convert.ToDouble(dto2.total * -1);
                                }
                            }
                            var Resultado_de_Explotacion_P1 = Total_Utilidad_Bruta_P1 - Gastos_de_Administracion_P1 - Gastos_de_Distribuidoras_P1;
                            var Resultados_Financieros_P1 = Intereses_Ganados_P1 + Intereses_Perdidos_P1 +Otros_Resultados_Financieros_P1;
                            var Resultado_antes_de_impuestos_P1 =  Resultado_de_Explotacion_P1 +Resultados_Financieros_P1 + Result_Ejercicios_Anteriores_P1 + Resultados_Diversos_P1 + Gan__por_Conversion_P1;
                            var Resultado_Neto_P1 = Resultado_antes_de_impuestos_P1 + Impuesto_a_la_Renta_P1;

                            lst_indicador_resultado_neto.Add(new IndicadorResultadoNetoViewModel()
                            {
                                pais = dto.pais.nombre,
                                anio = dto.anio,
                                valor = Math.Round(Convert.ToDouble(Resultado_Neto_P1) / 1000000, 2),
                                creciminento = 0.0,
                            });
                        }

                        if (lst_indicador_resultado_neto.Count() == 0)
                        {
                            lst_indicador_resultado_neto.Add(new IndicadorResultadoNetoViewModel()
                            {
                                pais = "--N/A--",
                                anio = 0,
                                valor = 0.0,
                                creciminento = 0.0,
                            });
                        }
                    }
                    lst_indicador_resultado_neto = lst_indicador_resultado_neto.OrderBy(d => d.anio).Take(3).ToList();
                    for (var i = 1; i < lst_indicador_resultado_neto.Count; i++)
                    {
                        lst_indicador_resultado_neto[i].creciminento = ((Convert.ToDouble(lst_indicador_resultado_neto[i].valor) - Convert.ToDouble(lst_indicador_resultado_neto[i - 1].valor)) / Convert.ToDouble(lst_indicador_resultado_neto[i].valor)) * 100;
                    }
                    oR.result = 1;
                    oR.data = lst_indicador_resultado_neto;
                }
            }
            catch (Exception ex)
            {
                oR.message = ex.ToString();
            }
            return oR;
        }
    }
}
