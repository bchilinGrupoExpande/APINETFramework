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
    public class IndicadoresFeeVentasController : BaseController
    {
        [HttpPost]
        public Reply ListarVentas([FromBody] IndicadoresViewModel model)
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

                    List<IndicadorVentaViewModel> lst_indicador_ventas = new List<IndicadorVentaViewModel>();
                    //Pais CEAM
                    if (model.id_pais == 1)
                    {
                        var presupuestos_ventas = (from ta in db.presupuestos
                                                   where ta.estado == 1
                                                   select ta.anio
                                                           ).Distinct().ToList();

                        foreach (var dto in presupuestos_ventas)
                        {
                            //Se buscan los valores de la Venta del Presupuesto
                            var lst_reporte_ventas_presupuesto_1 = new List<sp_valores_presupuesto_ventas_Result>();
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

                                if (string_presupuesto_ventas_1.Count() >= 1)
                                {
                                    string_presupuesto_ventas_1 = string_presupuesto_ventas_1.Remove(string_presupuesto_ventas_1.Length - 1);
                                }

                                lst_reporte_ventas_presupuesto_1 = db.sp_valores_presupuesto_ventas_periodos_CEAM(string_presupuesto_ventas_1, "1,2,3,4,5,6,7,8,9,10,11,12").ToList();
                            }
                            //Valida si busca el Ejecutado
                            if (model.id_tipo_datos_1 == 1)
                            {
                                lst_reporte_ventas_presupuesto_1 = db.total_Ejecutado_Por_Cuentas_de_Ventas("ceam", dto, "1,2,3,4,5,6,7,8,9,10,11,12", 1, "HIDRISAGE,ICLOS,MEDIHEALTH,PANALAB,POEN,ROE,ROWE", "ALMIRAL,SPEN,FERSON,GRA,GRH").ToList();
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
                            //Calculos Presupuesto de Ventas 1
                            foreach (var dto_venta in lst_reporte_ventas_presupuesto_1)
                            {
                                //Venta Bruta
                                if (dto_venta.codigo_linea == "Laboratorios Propios" && dto_venta.cuenta_contable == "41101")
                                {
                                    Venta_Bruta_Propios = Convert.ToDouble(dto_venta.total);
                                }
                                if (dto_venta.codigo_linea == "Laboratorios Representados" && dto_venta.cuenta_contable == "41101")
                                {
                                    Venta_Bruta_Representados = Convert.ToDouble(dto_venta.total);
                                }

                                //Bonificaciones
                                if (dto_venta.codigo_linea == "Laboratorios Propios" && dto_venta.cuenta_contable == "41203")
                                {
                                    Bonificaciones_Propios = Convert.ToDouble(dto_venta.total);
                                }
                                if (dto_venta.codigo_linea == "Laboratorios Representados" && dto_venta.cuenta_contable == "41203")
                                {
                                    Bonificaciones_Representados = Convert.ToDouble(dto_venta.total);
                                }

                                //Descuentos
                                if (dto_venta.codigo_linea == "Laboratorios Propios" && dto_venta.cuenta_contable == "41202")
                                {
                                    Descuentos_Propios = Convert.ToDouble(dto_venta.total);
                                }
                                if (dto_venta.codigo_linea == "Laboratorios Representados" && dto_venta.cuenta_contable == "41202")
                                {
                                    Descuentos_Reperesentado = Convert.ToDouble(dto_venta.total);
                                }

                                //Devoluciones
                                if (dto_venta.codigo_linea == "Laboratorios Propios" && dto_venta.cuenta_contable == "41201")
                                {
                                    Devoluciones_Propios = Convert.ToDouble(dto_venta.total);
                                }
                                if (dto_venta.codigo_linea == "Laboratorios Representados" && dto_venta.cuenta_contable == "41201")
                                {
                                    Devoluciones_Representados = Convert.ToDouble(dto_venta.total);
                                }

                                //Calculo del Margen de Contribucion Terceros
                                if (dto_venta.codigo_linea == "Laboratorios Representados" && dto_venta.cuenta_contable == "51101")
                                {
                                    CostoVentasLaboratiosRepresentados = Convert.ToDouble(dto_venta.total);
                                }
                                //Se Calcula el FEE de los Laboratorios Propios
                            

                            }

                            //Se Calcula la Venta Neta =VentaBruta-Bonificaciones-Descuentos
                            var Venta_Neta_Laboratorios_Propios_1 = Venta_Bruta_Propios + Descuentos_Propios + Devoluciones_Propios + Bonificaciones_Propios;
                            var Venta_Laboratorios_Representados_P1 = Venta_Bruta_Representados + Descuentos_Reperesentado + Bonificaciones_Representados + Devoluciones_Representados;


                            //Se Calcula el total de la Venta
                            var Venta_Local_P1 = Convert.ToDouble(Venta_Neta_Laboratorios_Propios_1) + Convert.ToDouble(Venta_Laboratorios_Representados_P1);

                            lst_indicador_ventas.Add(new IndicadorVentaViewModel()
                            {
                                pais = "CEAM",
                                anio = dto,
                                valor = Math.Round(Venta_Local_P1 / 1000000, 2),
                                creciminento = 0.0,
                            });
                        }

                        if (lst_indicador_ventas.Count() == 0)
                        {
                            lst_indicador_ventas.Add(new IndicadorVentaViewModel()
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
                        var presupuestos_ventas = db.presupuestos.Where(d => d.estado == 1 && d.id_tipo_presupuesto == 3 && d.id_pais == model.id_pais).ToList();

                        foreach (var dto in presupuestos_ventas)
                        {

                            double tipo_cambio_presupuesto_1 = 1;
                            var lst_moneda_presupuesto_1 = db.monedas_presupuesto.Where(d => d.id_presupuesto_asignado == dto.id).First();
                            tipo_cambio_presupuesto_1 = lst_moneda_presupuesto_1.tasa_cambio_presupuesto;
                            var anio_presupuesto_1 = dto.anio;
                            var id_pais_presupuesto_1 = dto.id_pais;


                            //Se buscan los valores de la Venta del Presupuesto
                            var lst_reporte_ventas_presupuesto_1 = new List<sp_valores_presupuesto_ventas_Result>();
                            //var lst_reporte_ventas_presupuesto_1 = db.sp_valores_presupuesto_ventas_periodos(dto.id, tipo_cambio_presupuesto_1, "1,2,3,4,5,6,7,8,9,10,11,12");

                            //Si el tipo de Reporte es 0 Buscar el reporte para el presupuesto y país seleccionado
                                if (model.id_tipo_datos_1 == 0)
                                {
                                    lst_reporte_ventas_presupuesto_1 = db.sp_valores_presupuesto_ventas_periodos(dto.id, tipo_cambio_presupuesto_1, "1,2,3,4,5,6,7,8,9,10,11,12").ToList();
                                }
                                //Valida si busca el Ejecutado
                                if (model.id_tipo_datos_1 == 1)
                                {
                                    var datos_presupuesto = db.presupuestos.Where(d => d.id == dto.id).First();
                                    var ejer = datos_presupuesto.anio;
                                    lst_reporte_ventas_presupuesto_1 = db.total_Ejecutado_Por_Cuentas_de_Ventas(codigo_pais_valor, ejer, "1,2,3,4,5,6,7,8,9,10,11,12", 1, "HIDRISAGE,ICLOS,MEDIHEALTH,PANALAB,POEN,ROE,ROWE", "ALMIRAL,SPEN,FERSON,GRA,GRH").ToList();
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
                            //Calculos Presupuesto de Ventas 1
                            foreach (var dto_venta in lst_reporte_ventas_presupuesto_1)
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
                                    Bonificaciones_Propios = Convert.ToDouble(dto_venta.total);
                                }
                                if (dto_venta.codigo_linea == "Laboratorios Representados" && dto_venta.cuenta_contable == "41203")
                                {
                                    Bonificaciones_Representados = Convert.ToDouble(dto_venta.total);
                                }

                                //Descuentos
                                if (dto_venta.codigo_linea == "Laboratorios Propios" && dto_venta.cuenta_contable == "41202")
                                {
                                    Descuentos_Propios = Convert.ToDouble(dto_venta.total);
                                }
                                if (dto_venta.codigo_linea == "Laboratorios Representados" && dto_venta.cuenta_contable == "41202")
                                {
                                    Descuentos_Reperesentado = Convert.ToDouble(dto_venta.total);
                                }

                                //Devoluciones
                                if (dto_venta.codigo_linea == "Laboratorios Propios" && dto_venta.cuenta_contable == "41201")
                                {
                                    Devoluciones_Propios = Convert.ToDouble(dto_venta.total);
                                }
                                if (dto_venta.codigo_linea == "Laboratorios Representados" && dto_venta.cuenta_contable == "41201")
                                {
                                    Devoluciones_Representados = Convert.ToDouble(dto_venta.total);
                                }

                                //Calculo del Margen de Contribucion Terceros
                                if (dto_venta.codigo_linea == "Laboratorios Representados" && dto_venta.cuenta_contable == "51101")
                                {
                                    CostoVentasLaboratiosRepresentados = Convert.ToDouble(dto_venta.total);
                                }
                            }

                            //Se Calcula la Venta Neta =VentaBruta-Bonificaciones-Descuentos
                            var Venta_Neta_Laboratorios_Propios_1 = Venta_Bruta_Propios + Descuentos_Propios + Devoluciones_Propios + Bonificaciones_Propios;
                            var Venta_Laboratorios_Representados_P1 = Venta_Bruta_Representados + Descuentos_Reperesentado + Bonificaciones_Representados + Devoluciones_Representados;


                            //Se Calcula el total de la Venta
                            var Venta_Local_P1 = Convert.ToDouble(Venta_Neta_Laboratorios_Propios_1) + Convert.ToDouble(Venta_Laboratorios_Representados_P1);

                            lst_indicador_ventas.Add(new IndicadorVentaViewModel()
                            {
                                pais = dto.pais.nombre,
                                anio = dto.anio,
                                valor = Math.Round(Venta_Local_P1 / 1000000, 2),
                                creciminento = 0.0,
                            });
                        }

                        if (lst_indicador_ventas.Count() == 0)
                        {
                            lst_indicador_ventas.Add(new IndicadorVentaViewModel()
                            {
                                pais = "--N/A--",
                                anio = 0,
                                valor = 0.0,
                                creciminento=0.0,
                            });
                        }
                    }

                    lst_indicador_ventas = lst_indicador_ventas.OrderBy(d => d.anio).Take(3).ToList();
                    for (var i = 1; i < lst_indicador_ventas.Count; i++)
                    {
                        lst_indicador_ventas[i].creciminento = ((Convert.ToDouble(lst_indicador_ventas[i].valor)- Convert.ToDouble(lst_indicador_ventas[i-1].valor))/ Convert.ToDouble(lst_indicador_ventas[i].valor))*100;
                    }

                    oR.result = 1;
                    oR.data = lst_indicador_ventas;
                }
            }
            catch (Exception ex)
            {
                oR.message = ex.ToString();
            }
            return oR;
        }


        [HttpPut]
        public Reply ListarFee([FromBody] IndicadoresViewModel model)
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

                    List<IndicadorFeeViewModel> lst_indicador_fee = new List<IndicadorFeeViewModel>();
                    //Pais CEAM
                    if (model.id_pais == 1)
                    {
                        var presupuestos_ventas = (from ta in db.presupuestos
                                                          where ta.estado==1
                                                          select ta.anio
                                                          ).Distinct().ToList();

                        foreach (var dto in presupuestos_ventas)
                        {
                            //Se buscan los valores de la Venta del Presupuesto
                            var lst_reporte_ventas_presupuesto_1 = new List<sp_valores_presupuesto_ventas_Result>();
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

                                if (string_presupuesto_ventas_1.Count() >= 1)
                                {
                                    string_presupuesto_ventas_1 = string_presupuesto_ventas_1.Remove(string_presupuesto_ventas_1.Length - 1);
                                }

                                lst_reporte_ventas_presupuesto_1 = db.sp_valores_presupuesto_ventas_periodos_CEAM(string_presupuesto_ventas_1, "1,2,3,4,5,6,7,8,9,10,11,12").ToList();
                            }
                            //Valida si busca el Ejecutado
                            if (model.id_tipo_datos_1 == 1)
                            {
                                lst_reporte_ventas_presupuesto_1 = db.total_Ejecutado_Por_Cuentas_de_Ventas("ceam", dto, "1,2,3,4,5,6,7,8,9,10,11,12", 1, "HIDRISAGE,ICLOS,MEDIHEALTH,PANALAB,POEN,ROE,ROWE", "ALMIRAL,SPEN,FERSON,GRA,GRH").ToList();
                            }

                            var Margen_de_Contribución_Free = 0.0;

                            //Calculos Presupuesto de Ventas 1
                            foreach (var dto_venta in lst_reporte_ventas_presupuesto_1)
                            {
                                //Se Calcula el FEE de los Laboratorios Propios
                                if (dto_venta.codigo_linea == "Laboratorios Propios" && dto_venta.cuenta_contable == "91110")
                                {
                                    Margen_de_Contribución_Free = Math.Abs(Convert.ToInt32(dto_venta.total));
                                }
                            }

                            lst_indicador_fee.Add(new IndicadorFeeViewModel()
                            {
                                pais = "CEAM",
                                anio = dto,
                                valor = Math.Round(Margen_de_Contribución_Free / 1000000, 2),
                                creciminento = 0.0,
                            });
                        }

                        if (lst_indicador_fee.Count() == 0)
                        {
                            lst_indicador_fee.Add(new IndicadorFeeViewModel()
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
                        var presupuestos_ventas = db.presupuestos.Where(d => d.estado == 1 && d.id_tipo_presupuesto == 3 && d.id_pais == model.id_pais).ToList();

                        foreach (var dto in presupuestos_ventas)
                        {

                            double tipo_cambio_presupuesto_1 = 1;
                            var lst_moneda_presupuesto_1 = db.monedas_presupuesto.Where(d => d.id_presupuesto_asignado == dto.id).First();
                            tipo_cambio_presupuesto_1 = lst_moneda_presupuesto_1.tasa_cambio_presupuesto;
                            var anio_presupuesto_1 = dto.anio;
                            var id_pais_presupuesto_1 = dto.id_pais;


                            //Se buscan los valores de la Venta del Presupuesto
                            var lst_reporte_ventas_presupuesto_1 = new List<sp_valores_presupuesto_ventas_Result>();
                            //var lst_reporte_ventas_presupuesto_1 = db.sp_valores_presupuesto_ventas_periodos(dto.id, tipo_cambio_presupuesto_1, "1,2,3,4,5,6,7,8,9,10,11,12");

                            //Si el tipo de Reporte es 0 Buscar el reporte para el presupuesto y país seleccionado
                            if (model.id_tipo_datos_1 == 0)
                            {
                                lst_reporte_ventas_presupuesto_1 = db.sp_valores_presupuesto_ventas_periodos(dto.id, tipo_cambio_presupuesto_1, "1,2,3,4,5,6,7,8,9,10,11,12").ToList();
                            }
                            //Valida si busca el Ejecutado
                            if (model.id_tipo_datos_1 == 1)
                            {
                                var datos_presupuesto = db.presupuestos.Where(d => d.id == dto.id).First();
                                var ejer = datos_presupuesto.anio;
                                lst_reporte_ventas_presupuesto_1 = db.total_Ejecutado_Por_Cuentas_de_Ventas(codigo_pais_valor, ejer, "1,2,3,4,5,6,7,8,9,10,11,12", 1, "HIDRISAGE,ICLOS,MEDIHEALTH,PANALAB,POEN,ROE,ROWE", "ALMIRAL,SPEN,FERSON,GRA,GRH").ToList();
                            }

                            double Margen_de_Contribución_Free = 0.0;

                            //Calculos Presupuesto de Ventas 1
                            foreach (var dto_venta in lst_reporte_ventas_presupuesto_1)
                            {
                                //Se Calcula el FEE de los Laboratorios Propios
                                if (dto_venta.codigo_linea == "Laboratorios Propios" && dto_venta.cuenta_contable == "91110")
                                {
                                    Margen_de_Contribución_Free = Math.Abs(Convert.ToInt32(dto_venta.total));
                                }
                            }

                            lst_indicador_fee.Add(new IndicadorFeeViewModel()
                            {
                                pais = dto.pais.nombre,
                                anio = dto.anio,
                                valor = Math.Round(Margen_de_Contribución_Free / 1000000, 2),
                                creciminento = 0.0,
                            });
                        }

                        if (lst_indicador_fee.Count() == 0)
                        {
                            lst_indicador_fee.Add(new IndicadorFeeViewModel()
                            {
                                pais = "--N/A--",
                                anio = 0,
                                valor = 0.0,
                                creciminento = 0.0,
                            });
                        }
                    }
                    lst_indicador_fee = lst_indicador_fee.OrderBy(d => d.anio).Take(3).ToList();
                    for (var i = 1; i < lst_indicador_fee.Count; i++)
                    {
                        lst_indicador_fee[i].creciminento = ((Convert.ToDouble(lst_indicador_fee[i].valor) - Convert.ToDouble(lst_indicador_fee[i - 1].valor)) / Convert.ToDouble(lst_indicador_fee[i].valor)) * 100;
                    }
                    oR.result = 1;
                    oR.data = lst_indicador_fee;
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
