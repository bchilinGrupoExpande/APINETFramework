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
    public class ReporteRubrosCorporativosController : BaseController
    {
        [HttpPost]
        public Reply Listar([FromBody] ReportesPresupuestoViewModel model)
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

                    ReporteRubrosCorporativosViewModel reportes_rubros = new ReporteRubrosCorporativosViewModel();

                    var id_tipo_cambio = Convert.ToInt32(model.Parametro);

                    double tipo_cambio_presupuesto_1 = 1;
                    double tipo_cambio_presupuesto_2 = 1;

                    if (id_tipo_cambio == 0)
                    {

                    }
                    else
                    {
                        var lst_moneda_presupuesto_1 = db.monedas_presupuesto.Where(d => d.id_presupuesto_asignado == model.id_presupuesto_1);
                        var lst_moneda_presupuesto_2 = db.monedas_presupuesto.Where(d => d.id_presupuesto_asignado == model.id_presupuesto_2);

                        foreach (var dto in lst_moneda_presupuesto_1)
                        {
                            tipo_cambio_presupuesto_1 = dto.tasa_cambio_presupuesto;
                        }
                        foreach (var dto in lst_moneda_presupuesto_2)
                        {
                            tipo_cambio_presupuesto_2 = dto.tasa_cambio_presupuesto;
                        }
                    }

                    var lst_presupuesto_1 = new List<presupuesto>();
                    var lst_presupuesto_2 = new List<presupuesto>();

                    if (model.id_presupuesto_1 is int valueOfA && model.id_presupuesto_1 != 0)
                    {
                        lst_presupuesto_1 = db.presupuestos.Where(d => d.id == model.id_presupuesto_1).ToList();
                    }
                    else
                    {
                        reportes_rubros.Nombre_Presupuesto_1 = "Presupuesto CEAM " + model.anio_1;
                    }

                    if (model.id_presupuesto_2 is int valueOfB && model.id_presupuesto_2 != 0)
                    {
                        lst_presupuesto_2 = db.presupuestos.Where(d => d.id == model.id_presupuesto_2).ToList();
                    }
                    else
                    {
                        reportes_rubros.Nombre_Presupuesto_2 = "Presupuesto CEAM " + model.anio_2;
                    }


                    var anio_presupuesto_1 = 0;
                    var anio_presupuesto_2 = 0;
                    var id_pais_presupuesto_1 = 0;
                    var id_pais_presupuesto_2 = 0;


                    foreach (var dto in lst_presupuesto_1)
                    {
                        if (model.id_tipo_reporte == 0)
                        {
                            reportes_rubros.Nombre_Presupuesto_1 = dto.nombre;
                        }

                        anio_presupuesto_1 = dto.anio;
                        id_pais_presupuesto_1 = dto.id_pais;
                    }
                    foreach (var dto in lst_presupuesto_2)
                    {
                        if (model.id_tipo_reporte == 0)
                        {
                            reportes_rubros.Nombre_Presupuesto_2 = dto.nombre;
                        }
                        anio_presupuesto_2 = dto.anio;
                        id_pais_presupuesto_2 = dto.id_pais;
                    }

                    var lst_presupuesto_ventas_1 = new presupuesto();
                    var lst_presupuesto_ventas_2 = new presupuesto();
                    var id_presupuesto_ventas_1 = 0;
                    var id_presupuesto_ventas_2 = 0;


                    if (model.id_presupuesto_1 is int valueOfC && model.id_presupuesto_1 != 0)
                    {
                        lst_presupuesto_ventas_1 = db.presupuestos.Where(d => d.anio == anio_presupuesto_1 && d.id_pais == id_pais_presupuesto_1 && d.id_tipo_presupuesto == 3 && d.estado == 1).First();
                        id_presupuesto_ventas_1 = lst_presupuesto_ventas_1.id;
                    }


                    if (model.id_presupuesto_2 is int valueOfD && model.id_presupuesto_2 != 0)
                    {
                        lst_presupuesto_ventas_2 = db.presupuestos.Where(d => d.anio == anio_presupuesto_2 && d.id_pais == id_pais_presupuesto_2 && d.id_tipo_presupuesto == 3 && d.estado == 1).First();
                        id_presupuesto_ventas_2 = lst_presupuesto_ventas_2.id;
                    }



                    //Se buscan los valores de la Venta del Presupuesto

                    var lst_reporte_ventas_presupuesto_1 = new List<sp_valores_presupuesto_ventas_Result>();
                    var lst_reporte_ventas_presupuesto_2 = new List<sp_valores_presupuesto_ventas_Result>();

                    //Si el tipo de Reporte es 0 Buscar el reporte para el presupuesto y país seleccionado
                    //Si el tipo de Reporte es 0 Buscar el reporte para el presupuesto y país seleccionado
                    if (model.id_tipo_reporte == 0)
                    {
                        //Validar si busca el Presupuestado
                        if (model.id_tipo_datos_1 == 0)
                        {
                                lst_reporte_ventas_presupuesto_1 = db.sp_valores_presupuesto_ventas_periodos(id_presupuesto_ventas_1, tipo_cambio_presupuesto_1, model.listado_periodos_1).ToList();
                        }
                        if (model.id_tipo_datos_2 == 0)
                        {
                            lst_reporte_ventas_presupuesto_2 = db.sp_valores_presupuesto_ventas_periodos(id_presupuesto_ventas_2, tipo_cambio_presupuesto_2, model.listado_periodos_2).ToList();
                        }
                        //Valida si busca el Ejecutado
                        if (model.id_tipo_datos_1 == 1)
                        {

                            var id_tipo_cambios = Convert.ToInt32(model.Parametro);
                            var datos_presupuesto = db.presupuestos.Where(d => d.id == model.id_presupuesto_1).First();
                            var codigo_pais = datos_presupuesto.pais.nombre;
                            var ejer = datos_presupuesto.anio;

                            lst_reporte_ventas_presupuesto_1 = db.total_Ejecutado_Por_Cuentas_de_Ventas(codigo_pais, ejer, model.listado_periodos_1, id_tipo_cambios, "HIDRISAGE,ICLOS,MEDIHEALTH,PANALAB,POEN,ROE,ROWE", "ALMIRAL,SPEN,FERSON,GRA,GRH").ToList();
                        }
                        if (model.id_tipo_datos_2 == 1)
                        {
                            var id_tipo_cambios = Convert.ToInt32(model.Parametro);
                            var datos_presupuesto = db.presupuestos.Where(d => d.id == model.id_presupuesto_2).First();
                            var codigo_pais = datos_presupuesto.pais.nombre;
                            var ejer = datos_presupuesto.anio;

                            lst_reporte_ventas_presupuesto_2 = db.total_Ejecutado_Por_Cuentas_de_Ventas(codigo_pais, ejer, model.listado_periodos_2, id_tipo_cambios, "HIDRISAGE,ICLOS,MEDIHEALTH,PANALAB,POEN,ROE,ROWE", "ALMIRAL,SPEN,FERSON,GRA,GRH").ToList();
                        }
                    }
                    //Si el tipo de Reporte es 1 Buscar el reporte CEAM para el Anio Seleccionado
                    else if (model.id_tipo_reporte == 1)
                    {
                        //Validar si busca el Presupuestado
                        if (model.id_tipo_datos_1 == 0)
                        {
                            var listado_presupuesto_ventas_anio = db.presupuestos.Where(d => d.anio == model.anio_1 && d.estado == 1 && d.id_tipo_presupuesto == 3).ToList();
                            var string_presupuesto_ventas_1 = "";
                            foreach (var dto_p in listado_presupuesto_ventas_anio)
                            {
                                var id_str = Convert.ToString(dto_p.id);
                                string_presupuesto_ventas_1 = string_presupuesto_ventas_1 + id_str + ",";
                            }

                            if (listado_presupuesto_ventas_anio.Count() >= 1)
                            {
                                string_presupuesto_ventas_1 = string_presupuesto_ventas_1.Remove(string_presupuesto_ventas_1.Length - 1);
                            }

                            lst_reporte_ventas_presupuesto_1 = db.sp_valores_presupuesto_ventas_periodos_CEAM(string_presupuesto_ventas_1, model.listado_periodos_1).ToList();
                        }
                        if (model.id_tipo_datos_2 == 0)
                        {
                            var listado_presupuesto_ventas_anio = db.presupuestos.Where(d => d.anio == model.anio_2 && d.estado == 1 && d.id_tipo_presupuesto == 3).ToList();
                            var string_presupuesto_ventas_2 = "";
                            foreach (var dto_p in listado_presupuesto_ventas_anio)
                            {
                                var id_str = Convert.ToString(dto_p.id);
                                string_presupuesto_ventas_2 = string_presupuesto_ventas_2 + id_str + ",";
                            }
                            if (listado_presupuesto_ventas_anio.Count() >= 1)
                            {
                                string_presupuesto_ventas_2 = string_presupuesto_ventas_2.Remove(string_presupuesto_ventas_2.Length - 1);
                            }

                            lst_reporte_ventas_presupuesto_2 = db.sp_valores_presupuesto_ventas_periodos_CEAM(string_presupuesto_ventas_2, model.listado_periodos_2).ToList();
                        }
                        //Valida si busca el Ejecutado
                        if (model.id_tipo_datos_1 == 1)
                        {
                            lst_reporte_ventas_presupuesto_1 = db.total_Ejecutado_Por_Cuentas_de_Ventas("ceam", model.anio_1, model.listado_periodos_1, id_tipo_cambio, "HIDRISAGE,ICLOS,MEDIHEALTH,PANALAB,POEN,ROE,ROWE", "ALMIRAL,SPEN,FERSON,GRA,GRH").ToList();
                        }
                        if (model.id_tipo_datos_2 == 1)
                        {
                            lst_reporte_ventas_presupuesto_2 = db.total_Ejecutado_Por_Cuentas_de_Ventas("ceam", model.anio_2, model.listado_periodos_2, id_tipo_cambio, "HIDRISAGE,ICLOS,MEDIHEALTH,PANALAB,POEN,ROE,ROWE", "ALMIRAL,SPEN,FERSON,GRA,GRH").ToList();
                        }
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
                    foreach (var dto in lst_reporte_ventas_presupuesto_1)
                    {
                        //Venta Bruta
                        if (dto.codigo_linea == "Laboratorios Propios" && dto.cuenta_contable == "41101")
                        {
                            Venta_Bruta_Propios = Venta_Bruta_Propios+ Math.Abs(Convert.ToDouble(dto.total))/1000;
                        }
                        if (dto.codigo_linea == "Laboratorios Representados" && dto.cuenta_contable == "41101")
                        {
                            Venta_Bruta_Representados = Venta_Bruta_Representados+ Math.Abs(Convert.ToDouble(dto.total))/1000;
                        }

                        //Bonificaciones
                        if (dto.codigo_linea == "Laboratorios Propios" && dto.cuenta_contable == "41203")
                        {
                            Bonificaciones_Propios = Bonificaciones_Propios + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        if (dto.codigo_linea == "Laboratorios Representados" && dto.cuenta_contable == "41203")
                        {
                            Bonificaciones_Representados = Bonificaciones_Representados + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }

                        //Descuentos
                        if (dto.codigo_linea == "Laboratorios Propios" && dto.cuenta_contable == "41202")
                        {
                            Descuentos_Propios = Descuentos_Propios + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        if (dto.codigo_linea == "Laboratorios Representados" && dto.cuenta_contable == "41202")
                        {
                            Descuentos_Reperesentado = Descuentos_Reperesentado + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }

                        //Devoluciones
                        if (dto.codigo_linea == "Laboratorios Propios" && dto.cuenta_contable == "41201")
                        {
                            Devoluciones_Propios = Devoluciones_Propios + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        if (dto.codigo_linea == "Laboratorios Representados" && dto.cuenta_contable == "41201")
                        {
                            Devoluciones_Representados = Devoluciones_Representados + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }

                        //Calculo del Margen de Contribucion Terceros
                        if (dto.codigo_linea == "Laboratorios Representados" && dto.cuenta_contable == "51101")
                        {
                            CostoVentasLaboratiosRepresentados = CostoVentasLaboratiosRepresentados + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }



                    }

                    //Se Calcula la Venta Neta =VentaBruta-Bonificaciones-Descuentos
                    reportes_rubros.Venta_Neta_Laboratorios_Propios_1 = Venta_Bruta_Propios - Descuentos_Propios - Devoluciones_Propios - Bonificaciones_Propios;
                    reportes_rubros.Venta_Laboratorios_Representados_P1 = Venta_Bruta_Representados - Descuentos_Reperesentado - Bonificaciones_Representados - Devoluciones_Representados;


                    //Se Calcula el total de la Venta
                    reportes_rubros.Venta_Local_P1 = Convert.ToDouble(reportes_rubros.Venta_Neta_Laboratorios_Propios_1) + Convert.ToDouble(reportes_rubros.Venta_Laboratorios_Representados_P1);

                    Venta_Bruta_Propios = 0.0;
                    Bonificaciones_Propios = 0.0;
                    Descuentos_Propios = 0.0;
                    Devoluciones_Propios = 0.0;

                    Venta_Bruta_Representados = 0.0;
                    Bonificaciones_Representados = 0.0;
                    Descuentos_Reperesentado = 0.0;
                    Devoluciones_Representados = 0.0;
                    CostoVentasLaboratiosRepresentados = 0.0;

                    //Calculos Presupuesto de Ventas 2
                    foreach (var dto in lst_reporte_ventas_presupuesto_2)
                    {

                        //Venta Bruta
                        if (dto.codigo_linea == "Laboratorios Propios" && dto.cuenta_contable == "41101")
                        {
                            Venta_Bruta_Propios = Venta_Bruta_Propios + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        if (dto.codigo_linea == "Laboratorios Representados" && dto.cuenta_contable == "41101")
                        {
                            Venta_Bruta_Representados = Venta_Bruta_Representados + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }

                        //Bonificaciones
                        if (dto.codigo_linea == "Laboratorios Propios" && dto.cuenta_contable == "41203")
                        {
                            Bonificaciones_Propios = Bonificaciones_Propios + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        if (dto.codigo_linea == "Laboratorios Representados" && dto.cuenta_contable == "41203")
                        {
                            Bonificaciones_Representados = Bonificaciones_Representados + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }

                        //Descuentos
                        if (dto.codigo_linea == "Laboratorios Propios" && dto.cuenta_contable == "41202")
                        {
                            Descuentos_Propios = Descuentos_Propios + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        if (dto.codigo_linea == "Laboratorios Representados" && dto.cuenta_contable == "41202")
                        {
                            Descuentos_Reperesentado = Descuentos_Reperesentado + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }

                        //Devoluciones
                        if (dto.codigo_linea == "Laboratorios Propios" && dto.cuenta_contable == "41201")
                        {
                            Devoluciones_Propios = Devoluciones_Propios + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        if (dto.codigo_linea == "Laboratorios Representados" && dto.cuenta_contable == "41201")
                        {
                            Devoluciones_Representados = Devoluciones_Representados + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }


                        //Calculo del Margen de Contribucion Terceros
                        if (dto.codigo_linea == "Laboratorios Representados" && dto.cuenta_contable == "51101")
                        {
                            CostoVentasLaboratiosRepresentados = CostoVentasLaboratiosRepresentados + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }

                    }

                    //Se Calcula la Venta Neta =VentaBruta-Bonificaciones-Descuentos
                    reportes_rubros.Venta_Neta_Laboratorios_Propios_P2 = Venta_Bruta_Propios - Descuentos_Propios - Devoluciones_Propios - Bonificaciones_Propios;
                    reportes_rubros.Venta_Laboratorios_Representados_P2 = Venta_Bruta_Representados - Descuentos_Reperesentado - Bonificaciones_Representados - Devoluciones_Representados;

                    //Se Calcula el total de la Venta
                    reportes_rubros.Venta_Local_P2 = Convert.ToDouble(reportes_rubros.Venta_Neta_Laboratorios_Propios_P2) + Convert.ToDouble(reportes_rubros.Venta_Laboratorios_Representados_P2);

                    var lst_reporte_1 = new List<sp_rubro_corporativos_valores_presupuesto_Result>();
                    var lst_reporte_2 = new List<sp_rubro_corporativos_valores_presupuesto_Result>();

                    if (model.id_tipo_reporte == 0)
                    {
                        if (model.id_tipo_datos_1 == 0)
                        {
                            lst_reporte_1 = db.sp_rubro_corporativos_valores_presupuesto_periodos(model.id_presupuesto_1, tipo_cambio_presupuesto_1, model.listado_periodos_1).ToList();
                        }
                        if (model.id_tipo_datos_2 == 0)
                        {
                            lst_reporte_2 = db.sp_rubro_corporativos_valores_presupuesto_periodos(model.id_presupuesto_2, tipo_cambio_presupuesto_2, model.listado_periodos_2).ToList();
                        }
                        if (model.id_tipo_datos_1 == 1)
                        {
                            var datos_presupuesto = db.presupuestos.Where(d => d.id == model.id_presupuesto_1).First();
                            var codigo_pais = datos_presupuesto.pais.nombre;
                            var ejer = datos_presupuesto.anio;

                            lst_reporte_1 = db.total_Ejecutado_Por_Rubro_Corporativo(codigo_pais, ejer, model.listado_periodos_1, id_tipo_cambio).ToList();
                        }
                        if (model.id_tipo_datos_2 == 1)
                        {
                            var datos_presupuesto = db.presupuestos.Where(d => d.id == model.id_presupuesto_2).First();
                            var codigo_pais = datos_presupuesto.pais.nombre;
                            var ejer = datos_presupuesto.anio;

                            lst_reporte_2 = db.total_Ejecutado_Por_Rubro_Corporativo(codigo_pais, ejer, model.listado_periodos_2, id_tipo_cambio).ToList();
                        }
                    }
                    else if (model.id_tipo_reporte == 1)
                    {
                        if (model.id_tipo_datos_1 == 0)
                        {
                          
                            var listado_presupuesto_ventas_anio = db.presupuestos.Where(d => d.anio == model.anio_1 && d.estado == 1 && d.id_tipo_presupuesto == model.id_tipo_presupuesto_1).ToList();
                            var string_presupuesto_ventas_1 = "";
                            foreach (var dto_p in listado_presupuesto_ventas_anio)
                            {
                                var id_str = Convert.ToString(dto_p.id);
                                string_presupuesto_ventas_1 = string_presupuesto_ventas_1 + id_str + ",";
                            }

                            if (listado_presupuesto_ventas_anio.Count() >= 1)
                            {
                                string_presupuesto_ventas_1 = string_presupuesto_ventas_1.Remove(string_presupuesto_ventas_1.Length - 1);
                            }
                          
                            lst_reporte_1 = db.sp_rubro_corporativos_valores_presupuesto_periodos_CEAM(string_presupuesto_ventas_1, model.listado_periodos_1).ToList();
                        }
                        if (model.id_tipo_datos_2 == 0)
                        {
                            var listado_presupuesto_ventas_anio = db.presupuestos.Where(d => d.anio == model.anio_2 && d.estado == 1 && d.id_tipo_presupuesto == model.id_tipo_presupuesto_2).ToList();
                            var string_presupuesto_ventas_2 = "";
                            foreach (var dto_p in listado_presupuesto_ventas_anio)
                            {
                                var id_str = Convert.ToString(dto_p.id);
                                string_presupuesto_ventas_2 = string_presupuesto_ventas_2 + id_str + ",";
                            }
                            if (listado_presupuesto_ventas_anio.Count() >= 1)
                            {
                                string_presupuesto_ventas_2 = string_presupuesto_ventas_2.Remove(string_presupuesto_ventas_2.Length - 1);
                            }
                          
                            lst_reporte_2 = db.sp_rubro_corporativos_valores_presupuesto_periodos_CEAM(string_presupuesto_ventas_2, model.listado_periodos_2).ToList();
                        }
                        if (model.id_tipo_datos_1 == 1)
                        {
                            lst_reporte_1 = db.total_Ejecutado_Por_Rubro_Corporativo("ceam", model.anio_1, model.listado_periodos_1, model.id_tipo_cambio).ToList();
                        }
                        if (model.id_tipo_datos_2 == 1)
                        {
                            lst_reporte_2 = db.total_Ejecutado_Por_Rubro_Corporativo("ceam", model.anio_2, model.listado_periodos_2, model.id_tipo_cambio).ToList();
                        }
                    }


                    reportes_rubros.Gastos_Variables_P1 = 0.0;
                    reportes_rubros.Gastos_Variables_P2 = 0.0;
                    reportes_rubros.Gastos_Fijos_P1 = 0.0;
                    reportes_rubros.Gastos_Fijos_P2 = 0.0;
                    reportes_rubros.Gastos_Administracion_Distribuidora_P1 = 0.0;
                    reportes_rubros.Gastos_Administracion_Distribuidora_P2 = 0.0;
                    reportes_rubros.Gastos_Administracion_Laboratorio_P1 = 0.0;
                    reportes_rubros.Gastos_Administracion_Laboratorio_P2 = 0.0;
                    reportes_rubros.C10_P1 = 0.0;
                    reportes_rubros.C10_P2 = 0.0;
                    reportes_rubros.C04_1_1_P1 = 0.0;
                    reportes_rubros.C04_1_2_P1 = 0.0;
                    reportes_rubros.C04_1_3_P1 = 0.0;
                    reportes_rubros.C04_1_4_P1 = 0.0;
                    reportes_rubros.C04_1_5_P1 = 0.0;
                    reportes_rubros.C04_1_6_P1 = 0.0;
                    reportes_rubros.C04_2_1_P1 = 0.0;
                    reportes_rubros.C04_2_2_P1 = 0.0;
                    reportes_rubros.C04_2_3_P1 = 0.0;
                    reportes_rubros.C04_2_4_P1 = 0.0;
                    reportes_rubros.C04_2_5_P1 = 0.0;
                    reportes_rubros.C04_2_6_P1 = 0.0;
                    reportes_rubros.C04_2_7_P1 = 0.0;
                    reportes_rubros.C04_2_8_P1 = 0.0;
                    reportes_rubros.C05_2_1_P1 = 0.0;
                    reportes_rubros.C05_2_2_P1 = 0.0;
                    reportes_rubros.C05_2_3_P1 = 0.0;
                    reportes_rubros.C05_2_4_P1 = 0.0;
                    reportes_rubros.C05_2_5_P1 = 0.0;
                    reportes_rubros.C05_2_6_P1 = 0.0;
                    reportes_rubros.C05_2_7_P1 = 0.0;
                    reportes_rubros.C06_P1 = 0.0;
                    reportes_rubros.C07_P1 = 0.0;
                    reportes_rubros.C08_P1 = 0.0;
                    reportes_rubros.C11_P1 = 0.0;
                    reportes_rubros.C04_1_1_P2 = 0.0;
                    reportes_rubros.C04_1_2_P2 = 0.0;
                    reportes_rubros.C04_1_3_P2 = 0.0;
                    reportes_rubros.C04_1_4_P2 = 0.0;
                    reportes_rubros.C04_1_5_P2 = 0.0;
                    reportes_rubros.C04_1_6_P2 = 0.0;
                    reportes_rubros.C04_2_1_P2 = 0.0;
                    reportes_rubros.C04_2_2_P2 = 0.0;
                    reportes_rubros.C04_2_3_P2 = 0.0;
                    reportes_rubros.C04_2_4_P2 = 0.0;
                    reportes_rubros.C04_2_5_P2 = 0.0;
                    reportes_rubros.C04_2_6_P2 = 0.0;
                    reportes_rubros.C04_2_7_P2 = 0.0;
                    reportes_rubros.C04_2_8_P2 = 0.0;
                    reportes_rubros.C05_2_1_P2 = 0.0;
                    reportes_rubros.C05_2_2_P2 = 0.0;
                    reportes_rubros.C05_2_3_P2 = 0.0;
                    reportes_rubros.C05_2_4_P2 = 0.0;
                    reportes_rubros.C05_2_5_P2 = 0.0;
                    reportes_rubros.C05_2_6_P2 = 0.0;
                    reportes_rubros.C05_2_7_P2 = 0.0;
                    reportes_rubros.C06_P2 = 0.0;
                    reportes_rubros.C07_P2 = 0.0;
                    reportes_rubros.C08_P2 = 0.0;
                    reportes_rubros.C11_P2 = 0.0;

                    foreach (var dto in lst_reporte_1)
                    {
                        if (dto.rubro == "C04.1.1")
                        {
                            reportes_rubros.C04_1_1_P1 = reportes_rubros.C04_1_1_P1  +Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Variables_P1= reportes_rubros.Gastos_Variables_P1 + dto.total/1000;
                        }
                        else if (dto.rubro == "C04.1.2")
                        {
                           reportes_rubros.C04_1_2_P1 = reportes_rubros.C04_1_2_P1 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Variables_P1 = reportes_rubros.Gastos_Variables_P1 + dto.total / 1000;
                        }
                        else if (dto.rubro == "C04.1.3")
                        {
                           reportes_rubros.C04_1_3_P1 = reportes_rubros.C04_1_3_P1+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Variables_P1 = reportes_rubros.Gastos_Variables_P1 + dto.total / 1000;
                        }
                        else if (dto.rubro == "C04.1.4")
                        {
                           reportes_rubros.C04_1_4_P1 = reportes_rubros.C04_1_4_P1+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Variables_P1 = reportes_rubros.Gastos_Variables_P1 + dto.total / 1000;
                        }
                        else if (dto.rubro == "C04.1.5")
                        {
                           reportes_rubros.C04_1_5_P1 = reportes_rubros.C04_1_5_P1+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Variables_P1 = reportes_rubros.Gastos_Variables_P1 + dto.total / 1000;
                        }
                        else if (dto.rubro == "C04.1.6")
                        {
                           reportes_rubros.C04_1_6_P1 = reportes_rubros.C04_1_6_P1 + Math.Abs(Convert.ToDouble(dto.total))/1000;
                            reportes_rubros.Gastos_Variables_P1 = reportes_rubros.Gastos_Variables_P1 + dto.total / 1000;
                        }
                        else if (dto.rubro == "C04.2.1")
                        {
                           reportes_rubros.C04_2_1_P1 = reportes_rubros.C04_2_1_P1 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Fijos_P1 = reportes_rubros.Gastos_Fijos_P1 + dto.total / 1000;
                        }
                        else if (dto.rubro == "C04.2.2")
                        {
                           reportes_rubros.C04_2_2_P1 = reportes_rubros.C04_2_2_P1+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Fijos_P1 = reportes_rubros.Gastos_Fijos_P1 + dto.total / 1000;
                        }
                        else if (dto.rubro == "C04.2.3")
                        {
                           reportes_rubros.C04_2_3_P1 = reportes_rubros.C04_2_3_P1 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Fijos_P1 = reportes_rubros.Gastos_Fijos_P1 + dto.total / 1000;
                        }
                        else if (dto.rubro == "C04.2.4")
                        {
                           reportes_rubros.C04_2_4_P1 = reportes_rubros.C04_2_4_P1+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Fijos_P1 = reportes_rubros.Gastos_Fijos_P1 + dto.total / 1000;
                        }
                        else if (dto.rubro == "C04.2.5")
                        {
                           reportes_rubros.C04_2_5_P1 = reportes_rubros.C04_2_5_P1 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Fijos_P1 = reportes_rubros.Gastos_Fijos_P1 + dto.total / 1000;
                        }
                        else if (dto.rubro == "C04.2.6")
                        {
                           reportes_rubros.C04_2_6_P1 = reportes_rubros.C04_2_6_P1 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Fijos_P1 = reportes_rubros.Gastos_Fijos_P1 + dto.total / 1000;
                        }
                        else if (dto.rubro == "C04.2.7")
                        {
                           reportes_rubros.C04_2_7_P1 = reportes_rubros.C04_2_7_P1 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Fijos_P1 = reportes_rubros.Gastos_Fijos_P1 + dto.total / 1000;
                        }
                        else if (dto.rubro == "C04.2.8")
                        {
                           reportes_rubros.C04_2_8_P1 = reportes_rubros.C04_2_8_P1 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Fijos_P1 = reportes_rubros.Gastos_Fijos_P1 + dto.total / 1000;
                        }
                    
                        else if (dto.rubro == "C05.2.1")
                        {
                            reportes_rubros.C05_2_1_P1 = reportes_rubros.C05_2_1_P1+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Administracion_Distribuidora_P1 = reportes_rubros.Gastos_Administracion_Distribuidora_P1 + dto.total / 1000;
                        }
                        else if (dto.rubro == "C05.2.2")
                        {
                           reportes_rubros.C05_2_2_P1 = reportes_rubros.C05_2_2_P1 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Administracion_Distribuidora_P1 = reportes_rubros.Gastos_Administracion_Distribuidora_P1 + dto.total / 1000;
                        }
                        else if (dto.rubro == "C05.2.3")
                        {
                           reportes_rubros.C05_2_3_P1 = reportes_rubros.C05_2_3_P1 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Administracion_Distribuidora_P1 = reportes_rubros.Gastos_Administracion_Distribuidora_P1 + dto.total / 1000;
                        }
                        else if (dto.rubro == "C05.2.4")
                        {
                           reportes_rubros.C05_2_4_P1 = reportes_rubros.C05_2_4_P1+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Administracion_Distribuidora_P1 = reportes_rubros.Gastos_Administracion_Distribuidora_P1 + dto.total / 1000;
                        }
                        else if (dto.rubro == "C05.2.5")
                        {
                           reportes_rubros.C05_2_5_P1 = reportes_rubros.C05_2_5_P1+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Administracion_Distribuidora_P1 = reportes_rubros.Gastos_Administracion_Distribuidora_P1 + dto.total / 1000;
                        }
                        else if (dto.rubro == "C05.2.6")
                        {
                           reportes_rubros.C05_2_6_P1 = reportes_rubros.C05_2_6_P1+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Administracion_Distribuidora_P1 = reportes_rubros.Gastos_Administracion_Distribuidora_P1 + dto.total / 1000;
                        }
                        else if (dto.rubro == "C05.2.7")
                        {
                           reportes_rubros.C05_2_7_P1 = reportes_rubros.C05_2_7_P1 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Administracion_Distribuidora_P1 = reportes_rubros.Gastos_Administracion_Distribuidora_P1 + dto.total / 1000;
                        }
                     
                        else if (dto.rubro == "C10")
                        {
                           reportes_rubros.C10_P1 = reportes_rubros.C10_P1 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C06")
                        {
                           reportes_rubros.C06_P1 = reportes_rubros.C06_P1 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C07")
                        {
                           reportes_rubros.C07_P1 = reportes_rubros.C07_P1 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C08")
                        {
                           reportes_rubros.C08_P1 = reportes_rubros.C08_P1 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C09")
                        {
                           reportes_rubros.C11_P1 = reportes_rubros.C11_P1 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                    }

                    foreach (var dto in lst_reporte_2)
                    {
                        if (dto.rubro == "C04.1.1")
                        {
                            reportes_rubros.C04_1_1_P2 = reportes_rubros.C04_1_1_P2+Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Variables_P2 = reportes_rubros.Gastos_Variables_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C04.1.2")
                        {
                            reportes_rubros.C04_1_2_P2 = reportes_rubros.C04_1_2_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Variables_P2 = reportes_rubros.Gastos_Variables_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C04.1.3")
                        {
                            reportes_rubros.C04_1_3_P2 = reportes_rubros.C04_1_3_P2+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Variables_P2 = reportes_rubros.Gastos_Variables_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C04.1.4")
                        {
                            reportes_rubros.C04_1_4_P2 = reportes_rubros.C04_1_4_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Variables_P2 = reportes_rubros.Gastos_Variables_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C04.1.5")
                        {
                            reportes_rubros.C04_1_5_P2 = reportes_rubros.C04_1_5_P2 +Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Variables_P2 = reportes_rubros.Gastos_Variables_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C04.1.6")
                        {
                            reportes_rubros.C04_1_6_P2 = reportes_rubros.C04_1_6_P2+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Variables_P2 = reportes_rubros.Gastos_Variables_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C04.2.1")
                        {
                            reportes_rubros.C04_2_1_P2 = reportes_rubros.C04_2_1_P2 +Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Fijos_P2 = reportes_rubros.Gastos_Fijos_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C04.2.2")
                        {
                            reportes_rubros.C04_2_2_P2 = reportes_rubros.C04_2_2_P2 +Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Fijos_P2 = reportes_rubros.Gastos_Fijos_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C04.2.3")
                        {
                            reportes_rubros.C04_2_3_P2 = reportes_rubros.C04_2_3_P2 +Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Fijos_P2 = reportes_rubros.Gastos_Fijos_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C04.2.4")
                        {
                            reportes_rubros.C04_2_4_P2 = reportes_rubros.C04_2_4_P2 +Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Fijos_P2 = reportes_rubros.Gastos_Fijos_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C04.2.5")
                        {
                            reportes_rubros.C04_2_5_P2 = reportes_rubros.C04_2_5_P2+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Fijos_P2 = reportes_rubros.Gastos_Fijos_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C04.2.6")
                        {
                            reportes_rubros.C04_2_6_P2 = reportes_rubros.C04_2_6_P2 +Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Fijos_P2 = reportes_rubros.Gastos_Fijos_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C04.2.7")
                        {
                            reportes_rubros.C04_2_7_P2 = reportes_rubros.C04_2_7_P2+Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Fijos_P2 = reportes_rubros.Gastos_Fijos_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C04.2.8")
                        {
                            reportes_rubros.C04_2_8_P2 = reportes_rubros.C04_2_8_P2 +Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Fijos_P2 = reportes_rubros.Gastos_Fijos_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }

                        else if (dto.rubro == "C05.2.1")
                        {
                            reportes_rubros.C05_2_1_P2 = reportes_rubros.C05_2_1_P2 +Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Administracion_Distribuidora_P2 = reportes_rubros.Gastos_Administracion_Distribuidora_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C05.2.2")
                        {
                            reportes_rubros.C05_2_2_P2 = reportes_rubros.C05_2_2_P2+Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Administracion_Distribuidora_P2 = reportes_rubros.Gastos_Administracion_Distribuidora_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C05.2.3")
                        {
                            reportes_rubros.C05_2_3_P2 = reportes_rubros.C05_2_3_P2 +Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Administracion_Distribuidora_P2 = reportes_rubros.Gastos_Administracion_Distribuidora_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C05.2.4")
                        {
                            reportes_rubros.C05_2_4_P2 = reportes_rubros.C05_2_4_P2 +Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Administracion_Distribuidora_P2 = reportes_rubros.Gastos_Administracion_Distribuidora_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C05.2.5")
                        {
                            reportes_rubros.C05_2_5_P2 = reportes_rubros.C05_2_5_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Administracion_Distribuidora_P2 = reportes_rubros.Gastos_Administracion_Distribuidora_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C05.2.6")
                        {
                            reportes_rubros.C05_2_6_P2 = reportes_rubros.C05_2_6_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Administracion_Distribuidora_P2 = reportes_rubros.Gastos_Administracion_Distribuidora_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C05.2.7")
                        {
                            reportes_rubros.C05_2_7_P2 = reportes_rubros.C05_2_7_P2+Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                            reportes_rubros.Gastos_Administracion_Distribuidora_P2 = reportes_rubros.Gastos_Administracion_Distribuidora_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }

                        else if (dto.rubro == "C10")
                        {
                            reportes_rubros.C10_P2 = reportes_rubros.C10_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C06")
                        {
                            reportes_rubros.C06_P2 = reportes_rubros.C06_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C07")
                        {
                            reportes_rubros.C07_P2 = reportes_rubros.C07_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C08")
                        {
                            reportes_rubros.C08_P2 = reportes_rubros.C08_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.rubro == "C09")
                        {
                            reportes_rubros.C11_P2 = reportes_rubros.C11_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                    }

                    //Calculos de totales
                    reportes_rubros.Total_Gastos_De_Distribuidoras_P1 = reportes_rubros.Gastos_Variables_P1 + reportes_rubros.Gastos_Fijos_P1;
                    reportes_rubros.Total_Gastos_De_Distribuidoras_P2 = reportes_rubros.Gastos_Variables_P2 + reportes_rubros.Gastos_Fijos_P2;

                    reportes_rubros.Total_Gastos_De_Adminsitracion_P1 = reportes_rubros.Gastos_Administracion_Distribuidora_P1 + reportes_rubros.Gastos_Administracion_Laboratorio_P1;
                    reportes_rubros.Total_Gastos_De_Adminsitracion_P2 = reportes_rubros.Gastos_Administracion_Distribuidora_P2 + reportes_rubros.Gastos_Administracion_Laboratorio_P2;

                    reportes_rubros.Total_Gastos_P1 = reportes_rubros.Total_Gastos_De_Adminsitracion_P1 + reportes_rubros.Total_Gastos_De_Distribuidoras_P1;
                    reportes_rubros.Total_Gastos_P2 = reportes_rubros.Total_Gastos_De_Adminsitracion_P2 + reportes_rubros.Total_Gastos_De_Distribuidoras_P2;

                    oR.result = 1;
                    oR.data =reportes_rubros;



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
