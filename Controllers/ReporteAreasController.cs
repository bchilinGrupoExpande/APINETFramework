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
    public class ReporteAreasController : BaseController
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

                    ReporteAreasCorporativasViewModel reportes_areas = new ReporteAreasCorporativasViewModel();

                    //Si viene 1 es en Dolares y si viene 0 no
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
                      
                        foreach(var dto in lst_moneda_presupuesto_1)
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
                        reportes_areas.Nombre_Presupuesto_1 = "Presupuesto CEAM " + model.anio_1;
                    }

                    if (model.id_presupuesto_2 is int valueOfB && model.id_presupuesto_2 != 0)
                    {
                         lst_presupuesto_2 = db.presupuestos.Where(d => d.id == model.id_presupuesto_2).ToList();
                    }
                    else
                    {
                        reportes_areas.Nombre_Presupuesto_2 = "Presupuesto CEAM " + model.anio_2;
                    }


                    var anio_presupuesto_1 = 0;
                    var anio_presupuesto_2 = 0;
                    var id_pais_presupuesto_1 = 0;
                    var id_pais_presupuesto_2 = 0;


                    foreach (var dto in lst_presupuesto_1)
                    {
                        if (model.id_tipo_reporte == 0)
                        {
                            reportes_areas.Nombre_Presupuesto_1 = dto.nombre;
                        }
                     
                        anio_presupuesto_1 = dto.anio;
                        id_pais_presupuesto_1 = dto.id_pais;
                    }
                    foreach (var dto in lst_presupuesto_2)
                    {
                        if (model.id_tipo_reporte == 0)
                        {
                            reportes_areas.Nombre_Presupuesto_2 = dto.nombre;
                        }
                        anio_presupuesto_2 = dto.anio;
                        id_pais_presupuesto_2 = dto.id_pais;
                    }

                    var lst_presupuesto_ventas_1 = new presupuesto();
                    var lst_presupuesto_ventas_2 = new presupuesto();

                    var id_presupuesto_ventas_1 = 0;
                    var id_presupuesto_ventas_2 = 0;


                    if (db.presupuestos.Where(d => d.anio == anio_presupuesto_1 && d.id_pais == id_pais_presupuesto_1 && d.id_tipo_presupuesto == 3 && d.estado == 1).Count()>0)
                    {
                        lst_presupuesto_ventas_1 = db.presupuestos.Where(d => d.anio == anio_presupuesto_1 && d.id_pais == id_pais_presupuesto_1 && d.id_tipo_presupuesto == 3 && d.estado == 1).First();
                        id_presupuesto_ventas_1 = lst_presupuesto_ventas_1.id;
                    }


                    if (db.presupuestos.Where(d => d.anio == anio_presupuesto_2 && d.id_pais == id_pais_presupuesto_2 && d.id_tipo_presupuesto == 3 && d.estado == 1).Count()>0)
                    {
                        lst_presupuesto_ventas_2 = db.presupuestos.Where(d => d.anio == anio_presupuesto_2 && d.id_pais == id_pais_presupuesto_2 && d.id_tipo_presupuesto == 3 && d.estado == 1).First();
                        id_presupuesto_ventas_2 = lst_presupuesto_ventas_2.id;
                    }

                    var dtos_ = lst_presupuesto_ventas_1.id;
                    

                    if (lst_presupuesto_ventas_1.id!=0)
                    {
                        id_presupuesto_ventas_1 = lst_presupuesto_ventas_1.id;
                    }
                    if (lst_presupuesto_ventas_2.id!=0)
                    {
                        id_presupuesto_ventas_2 = lst_presupuesto_ventas_2.id;
                    }
                    //Se buscan los valores de la Venta del Presupuesto

                    var lst_reporte_ventas_presupuesto_1 = new List<sp_valores_presupuesto_ventas_Result>();
                    var lst_reporte_ventas_presupuesto_2 = new List<sp_valores_presupuesto_ventas_Result>();


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
                    else if(model.id_tipo_reporte==1)
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
                    reportes_areas.Margen_de_Contribución_Free_P1 = 0.0;
                    reportes_areas.Margen_de_Contribución_Free_P2 = 0.0;

                    //Calculos Presupuesto de Ventas 1
                    foreach (var dto in lst_reporte_ventas_presupuesto_1)
                    {
                        //Venta Bruta
                        if (dto.codigo_linea== "Laboratorios Propios" && dto.cuenta_contable == "41101")
                        {
                            Venta_Bruta_Propios = Venta_Bruta_Propios + Math.Abs(Convert.ToDouble(dto.total));
                        }
                        if (dto.codigo_linea == "Laboratorios Representados" && dto.cuenta_contable == "41101")
                        {
                            Venta_Bruta_Representados = Venta_Bruta_Representados + Math.Abs(Convert.ToDouble(dto.total));
                        }

                        //Bonificaciones
                        if (dto.codigo_linea == "Laboratorios Propios" && dto.cuenta_contable == "41203")
                        {
                            Bonificaciones_Propios = Bonificaciones_Propios + Math.Abs(Convert.ToDouble(dto.total));
                        }
                        if (dto.codigo_linea == "Laboratorios Representados" && dto.cuenta_contable == "41203")
                        {
                            Bonificaciones_Representados = Bonificaciones_Representados + Math.Abs(Convert.ToDouble(dto.total));
                        }

                        //Descuentos
                        if (dto.codigo_linea == "Laboratorios Propios" && dto.cuenta_contable == "41202")
                        {
                            Descuentos_Propios = Descuentos_Propios + Math.Abs(Convert.ToDouble(dto.total));
                        }
                        if (dto.codigo_linea == "Laboratorios Representados" && dto.cuenta_contable == "41202")
                        {
                            Descuentos_Reperesentado = Descuentos_Reperesentado + Math.Abs(Convert.ToDouble(dto.total));
                        }

                        //Devoluciones
                        if (dto.codigo_linea == "Laboratorios Propios" && dto.cuenta_contable == "41201")
                        {
                            Devoluciones_Propios = Devoluciones_Propios + Math.Abs(Convert.ToDouble(dto.total));
                        }
                        if (dto.codigo_linea == "Laboratorios Representados" && dto.cuenta_contable == "41201")
                        {
                            Devoluciones_Representados = Devoluciones_Representados + Math.Abs(Convert.ToDouble(dto.total));
                        }

                        //Se Calcula el FEE de los Laboratorios Propios
                        if (dto.codigo_linea == "Laboratorios Propios" && dto.cuenta_contable == "91110")
                        {
                            reportes_areas.Margen_de_Contribución_Free_P1 = reportes_areas.Margen_de_Contribución_Free_P1 + Math.Abs(Convert.ToDouble(dto.total));
                        }

                        //Calculo del Margen de Contribucion Terceros
                        if (dto.codigo_linea == "Laboratorios Representados" && dto.cuenta_contable == "51101")
                        {
                            CostoVentasLaboratiosRepresentados = CostoVentasLaboratiosRepresentados + Math.Abs(Convert.ToDouble(dto.total));
                        }
                       

                       
                    }

                    //Se Calcula la Venta Neta =VentaBruta-Bonificaciones-Descuentos
                    reportes_areas.Venta_Neta_Laboratorios_Propios_1 = Venta_Bruta_Propios - Descuentos_Propios - Devoluciones_Propios - Bonificaciones_Propios;
                    reportes_areas.Venta_Laboratorios_Representados_P1 = Venta_Bruta_Representados - Descuentos_Reperesentado - Bonificaciones_Representados - Devoluciones_Representados;
                    reportes_areas.Margen_de_Contribución_Tercero_P1 = reportes_areas.Venta_Laboratorios_Representados_P1 - CostoVentasLaboratiosRepresentados;
                    reportes_areas.Total_Utilidad_Bruta_P1 = reportes_areas.Margen_de_Contribución_Tercero_P1 + reportes_areas.Margen_de_Contribución_Free_P1;

                    //Se Calcula el total de la Venta
                    reportes_areas.Venta_Local_P1 = Convert.ToDouble(reportes_areas.Venta_Neta_Laboratorios_Propios_1) + Convert.ToDouble(reportes_areas.Venta_Laboratorios_Representados_P1);


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
                            Venta_Bruta_Propios = Venta_Bruta_Propios + Math.Abs(Convert.ToDouble(dto.total));
                        }
                        if (dto.codigo_linea == "Laboratorios Representados" && dto.cuenta_contable == "41101")
                        {
                            Venta_Bruta_Representados = Venta_Bruta_Representados + Math.Abs(Convert.ToDouble(dto.total));
                        }

                        //Bonificaciones
                        if (dto.codigo_linea == "Laboratorios Propios" && dto.cuenta_contable == "41203")
                        {
                            Bonificaciones_Propios = Bonificaciones_Propios + Math.Abs(Convert.ToDouble(dto.total));
                        }
                        if (dto.codigo_linea == "Laboratorios Representados" && dto.cuenta_contable == "41203")
                        {
                            Bonificaciones_Representados = Bonificaciones_Representados + Math.Abs(Convert.ToDouble(dto.total));
                        }

                        //Descuentos
                        if (dto.codigo_linea == "Laboratorios Propios" && dto.cuenta_contable == "41202")
                        {
                            Descuentos_Propios = Descuentos_Propios + Math.Abs(Convert.ToDouble(dto.total));
                        }
                        if (dto.codigo_linea == "Laboratorios Representados" && dto.cuenta_contable == "41202")
                        {
                            Descuentos_Reperesentado = Descuentos_Reperesentado + Math.Abs(Convert.ToDouble(dto.total));
                        }

                        //Devoluciones
                        if (dto.codigo_linea == "Laboratorios Propios" && dto.cuenta_contable == "41201")
                        {
                            Devoluciones_Propios = Devoluciones_Propios + Math.Abs(Convert.ToDouble(dto.total));
                        }
                        if (dto.codigo_linea == "Laboratorios Representados" && dto.cuenta_contable == "41201")
                        {
                            Devoluciones_Representados = Devoluciones_Representados + Math.Abs(Convert.ToDouble(dto.total));
                        }

                        //Se Calcula el FEE de los Laboratorios Propios
                        if (dto.codigo_linea == "Laboratorios Propios" && dto.cuenta_contable == "91110")
                        {
                            reportes_areas.Margen_de_Contribución_Free_P2 = reportes_areas.Margen_de_Contribución_Free_P2 + Math.Abs(Convert.ToDouble(dto.total));
                        }

                        //Calculo del Margen de Contribucion Terceros
                        if (dto.codigo_linea == "Laboratorios Representados" && dto.cuenta_contable == "51101")
                        {
                            CostoVentasLaboratiosRepresentados = CostoVentasLaboratiosRepresentados + Math.Abs(Convert.ToDouble(dto.total));
                        }

                    }

                    //Se Calcula la Venta Neta =VentaBruta-Bonificaciones-Descuentos
                    reportes_areas.Venta_Neta_Laboratorios_Propios_P2 = Venta_Bruta_Propios - Descuentos_Propios - Devoluciones_Propios - Bonificaciones_Propios;
                    reportes_areas.Venta_Laboratorios_Representados_P2 = Venta_Bruta_Representados - Descuentos_Reperesentado - Bonificaciones_Representados - Devoluciones_Representados;
                    reportes_areas.Margen_de_Contribución_Tercero_P2 = reportes_areas.Venta_Laboratorios_Representados_P2 - CostoVentasLaboratiosRepresentados;

                    reportes_areas.Total_Utilidad_Bruta_P2 = reportes_areas.Margen_de_Contribución_Tercero_P2 + reportes_areas.Margen_de_Contribución_Free_P2;

                    //Se Calcula el total de la Venta
                    reportes_areas.Venta_Local_P2 = Convert.ToDouble(reportes_areas.Venta_Neta_Laboratorios_Propios_P2) + Convert.ToDouble(reportes_areas.Venta_Laboratorios_Representados_P2);

                    //Se divide por 1000 la venta
                    reportes_areas.Venta_Neta_Laboratorios_Propios_1 = reportes_areas.Venta_Neta_Laboratorios_Propios_1 / 1000;
                    reportes_areas.Venta_Laboratorios_Representados_P1 = reportes_areas.Venta_Laboratorios_Representados_P1 / 1000;
                    reportes_areas.Venta_Local_P1 = reportes_areas.Venta_Local_P1 / 1000;
                    reportes_areas.Margen_de_Contribución_Free_P1 = reportes_areas.Margen_de_Contribución_Free_P1 / 1000;
                    reportes_areas.Margen_de_Contribución_Tercero_P1 = reportes_areas.Margen_de_Contribución_Tercero_P1 / 1000;
                    reportes_areas.Total_Utilidad_Bruta_P1 = reportes_areas.Total_Utilidad_Bruta_P1 / 1000;

                    reportes_areas.Venta_Neta_Laboratorios_Propios_P2 = reportes_areas.Venta_Neta_Laboratorios_Propios_P2 / 1000;
                    reportes_areas.Venta_Laboratorios_Representados_P2 = reportes_areas.Venta_Laboratorios_Representados_P2 / 1000;
                    reportes_areas.Venta_Local_P2 = reportes_areas.Venta_Local_P2 / 1000;
                    reportes_areas.Margen_de_Contribución_Free_P2 = reportes_areas.Margen_de_Contribución_Free_P2 / 1000;
                    reportes_areas.Margen_de_Contribución_Tercero_P2 = reportes_areas.Margen_de_Contribución_Tercero_P2 / 1000;
                    reportes_areas.Total_Utilidad_Bruta_P2 = reportes_areas.Total_Utilidad_Bruta_P2 / 1000;


                    //Se encuentran los Valores Presupuestados
                    var lst_reporte_1 = new List<sp_areas_equivalentes_valores_presupuesto_Result>();
                    var lst_reporte_2 = new List<sp_areas_equivalentes_valores_presupuesto_Result>();

                    if (model.id_tipo_reporte == 0)
                    {
                        if (model.id_tipo_datos_1 == 0)
                        {
                            lst_reporte_1 = db.sp_areas_equivalentes_valores_presupuesto_periodos(model.id_presupuesto_1, tipo_cambio_presupuesto_1, model.listado_periodos_1).ToList();
                        }
                        if (model.id_tipo_datos_2 == 0)
                        {
                            lst_reporte_2 = db.sp_areas_equivalentes_valores_presupuesto_periodos(model.id_presupuesto_2, tipo_cambio_presupuesto_2, model.listado_periodos_2).ToList();
                        }
                        if (model.id_tipo_datos_1 == 1)
                        {
                            var datos_presupuesto = db.presupuestos.Where(d => d.id == model.id_presupuesto_1).First();
                            var codigo_pais = datos_presupuesto.pais.nombre;
                            var ejer = datos_presupuesto.anio;

                            lst_reporte_1 = db.total_Ejecutado_Por_Area_Equivalente(codigo_pais, ejer, model.listado_periodos_1, model.id_tipo_cambio).ToList();
                        }
                        if (model.id_tipo_datos_2 == 1)
                        {
                            var datos_presupuesto = db.presupuestos.Where(d => d.id == model.id_presupuesto_2).First();
                            var codigo_pais = datos_presupuesto.pais.nombre;
                            var ejer = datos_presupuesto.anio;

                            lst_reporte_2 = db.total_Ejecutado_Por_Area_Equivalente(codigo_pais, ejer, model.listado_periodos_2, model.id_tipo_cambio).ToList();
                        }
                    }else if (model.id_tipo_reporte == 1)
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
                           

                            lst_reporte_1 = db.sp_areas_equivalentes_valores_presupuesto_periodos_CEAM(string_presupuesto_ventas_1, model.listado_periodos_1).ToList();
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


                            lst_reporte_2 = db.sp_areas_equivalentes_valores_presupuesto_periodos_CEAM(string_presupuesto_ventas_2, model.listado_periodos_2).ToList();
                        }
                        if (model.id_tipo_datos_1 == 1)
                        {
                            lst_reporte_1 = db.total_Ejecutado_Por_Area_Equivalente("ceam", model.anio_1, model.listado_periodos_1, id_tipo_cambio).ToList();
                        }
                        if (model.id_tipo_datos_2 == 1)
                        {
                            lst_reporte_2 = db.total_Ejecutado_Por_Area_Equivalente("ceam", model.anio_2, model.listado_periodos_2, id_tipo_cambio).ToList();
                        }
                    }

                    reportes_areas.Comercial_P1 = 0.0;
                    reportes_areas.Logistica_P1 = 0.0;
                    reportes_areas.Compras_P1 = 0.0;
                    reportes_areas.Contabilidad_P1 = 0.0;
                    reportes_areas.Creditos_P1 = 0.0;
                    reportes_areas.Gestion_Humana_P1 = 0.0;
                    reportes_areas.Mejora_Continua_P1 = 0.0;
                    reportes_areas.Administracion_P1 = 0.0;
                    reportes_areas.TI_CEAM_P1 = 0.0;
                    reportes_areas.Tecnologa_de_Informacion_P1 = 0.0;
                    reportes_areas.Gastos_Regionales_P1 = 0.0;

                    reportes_areas.Comercial_P2 = 0.0;
                    reportes_areas.Logistica_P2 = 0.0;
                    reportes_areas.Compras_P2 = 0.0;
                    reportes_areas.Contabilidad_P2 = 0.0;
                    reportes_areas.Creditos_P2 = 0.0;
                    reportes_areas.Gestion_Humana_P2 = 0.0;
                    reportes_areas.Mejora_Continua_P2 = 0.0;
                    reportes_areas.Administracion_P2 = 0.0;
                    reportes_areas.TI_CEAM_P2 = 0.0;
                    reportes_areas.Tecnologa_de_Informacion_P2 = 0.0;
                    reportes_areas.Gastos_Regionales_P2 = 0.0;
                    //Calculos Presupuesto de Gastos 1
                    foreach (var dto in lst_reporte_1)
                    {
                        if (dto.area_equivalente == "Comercial")
                        {
                            reportes_areas.Comercial_P1 = reportes_areas.Comercial_P1 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.area_equivalente == "Logistica")
                        {
                            reportes_areas.Logistica_P1 = reportes_areas.Logistica_P1+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.area_equivalente == "Compras")
                        {
                            reportes_areas.Compras_P1 = reportes_areas.Compras_P1+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.area_equivalente == "Contabilidad")
                        {
                            reportes_areas.Contabilidad_P1 = reportes_areas.Contabilidad_P1+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.area_equivalente == "Creditos")
                        {
                            reportes_areas.Creditos_P1 = reportes_areas.Creditos_P1+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.area_equivalente == "Gestion Humana")
                        {
                            reportes_areas.Gestion_Humana_P1 = reportes_areas.Gestion_Humana_P1+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.area_equivalente == "Mejora Continua")
                        {
                            reportes_areas.Mejora_Continua_P1 = reportes_areas.Mejora_Continua_P1+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.area_equivalente == "Administracion")
                        {
                            reportes_areas.Administracion_P1 = reportes_areas.Administracion_P1+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.area_equivalente == "Tecnologia de Informacion (TI)")
                        {
                            reportes_areas.Tecnologa_de_Informacion_P1 = reportes_areas.Tecnologa_de_Informacion_P1+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.area_equivalente == "TI CEAM")
                        {
                            reportes_areas.TI_CEAM_P1 = reportes_areas.TI_CEAM_P1 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.area_equivalente == "Gastos Regionales")
                        {
                            reportes_areas.Gastos_Regionales_P1 = reportes_areas.Gastos_Regionales_P1+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                       
                    }

                    //Calculos Presupuesto de Gastos 2
                    foreach (var dto in lst_reporte_2)
                    {
                        if (dto.area_equivalente == "Comercial")
                        {
                            reportes_areas.Comercial_P2 = reportes_areas.Comercial_P2+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.area_equivalente == "Logistica")
                        {
                            reportes_areas.Logistica_P2 = reportes_areas.Logistica_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.area_equivalente == "Compras")
                        {
                            reportes_areas.Compras_P2 = reportes_areas.Compras_P2+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.area_equivalente == "Contabilidad")
                        {
                            reportes_areas.Contabilidad_P2 = reportes_areas.Contabilidad_P2+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.area_equivalente == "Creditos")
                        {
                            reportes_areas.Creditos_P2 = reportes_areas.Creditos_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.area_equivalente == "Gestion Humana")
                        {
                            reportes_areas.Gestion_Humana_P2 = reportes_areas.Gestion_Humana_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.area_equivalente == "Mejora Continua")
                        {
                            reportes_areas.Mejora_Continua_P2 = reportes_areas.Mejora_Continua_P2+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.area_equivalente == "Administracion")
                        {
                            reportes_areas.Administracion_P2 = reportes_areas.Administracion_P2+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.area_equivalente == "Tecnologia de Informacion (TI)")
                        {
                            reportes_areas.Tecnologa_de_Informacion_P2 = reportes_areas.Tecnologa_de_Informacion_P2+ Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.area_equivalente == "TI CEAM")
                        {
                            reportes_areas.TI_CEAM_P2 = reportes_areas.TI_CEAM_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                        else if (dto.area_equivalente == "Gastos Regionales")
                        {
                            reportes_areas.Gastos_Regionales_P2 = reportes_areas.Gastos_Regionales_P2 + Math.Abs(Convert.ToDouble(dto.total)) / 1000;
                        }
                    }

                    //Se Calculan los totales de Administracion y de la Distribuidoras
                    reportes_areas.Gastos_de_Distribuidoras_P1 = reportes_areas.Logistica_P1 + reportes_areas.Comercial_P1;
                    reportes_areas.Gastos_de_Distribuidoras_P2 = reportes_areas.Logistica_P2 + reportes_areas.Comercial_P2;

                    reportes_areas.Gastos_de_Administracion_P1 = reportes_areas.Administracion_P1+ reportes_areas.Compras_P1 + reportes_areas.Contabilidad_P1+reportes_areas.Creditos_P1+reportes_areas.Gestion_Humana_P1+reportes_areas.Mejora_Continua_P1+reportes_areas.Tecnologa_de_Informacion_P1;
                    reportes_areas.Gastos_de_Administracion_P2 = reportes_areas.Administracion_P2 + reportes_areas.Compras_P2 + reportes_areas.Contabilidad_P2 + reportes_areas.Creditos_P2 + reportes_areas.Gestion_Humana_P2 + reportes_areas.Mejora_Continua_P2 + reportes_areas.Tecnologa_de_Informacion_P2;

                    reportes_areas.Resultado_de_Explotacion_P1 = reportes_areas.Total_Utilidad_Bruta_P1 - reportes_areas.Gastos_de_Administracion_P1 - reportes_areas.Gastos_de_Distribuidoras_P1;
                    reportes_areas.Resultado_de_Explotacion_P2 = reportes_areas.Total_Utilidad_Bruta_P2 - reportes_areas.Gastos_de_Administracion_P2 - reportes_areas.Gastos_de_Distribuidoras_P2;

                    var lst_reporte_1_rubros = new List<sp_rubro_corporativos_valores_presupuesto_Result>();
                    var lst_reporte_2_rubros = new List<sp_rubro_corporativos_valores_presupuesto_Result>();


                    if (model.id_tipo_reporte == 0)
                    {
                        if (model.id_tipo_datos_1 == 0)
                        {
                            lst_reporte_1_rubros = db.sp_rubro_corporativos_valores_presupuesto_periodos(model.id_presupuesto_1, tipo_cambio_presupuesto_1, model.listado_periodos_1).ToList();
                        }
                        if (model.id_tipo_datos_2 == 0)
                        {
                            lst_reporte_2_rubros = db.sp_rubro_corporativos_valores_presupuesto_periodos(model.id_presupuesto_2, tipo_cambio_presupuesto_2, model.listado_periodos_2).ToList();
                        }
                        if (model.id_tipo_datos_1 == 1)
                        {
                            var datos_presupuesto = db.presupuestos.Where(d => d.id == model.id_presupuesto_1).First();
                            var codigo_pais = datos_presupuesto.pais.nombre;
                            var ejer = datos_presupuesto.anio;

                            lst_reporte_1_rubros = db.total_Ejecutado_Por_Rubro_Corporativo(codigo_pais, ejer, model.listado_periodos_1, model.id_tipo_cambio).ToList();
                        }
                        if (model.id_tipo_datos_2 == 1)
                        {
                            var datos_presupuesto = db.presupuestos.Where(d => d.id == model.id_presupuesto_2).First();
                            var codigo_pais = datos_presupuesto.pais.nombre;
                            var ejer = datos_presupuesto.anio;

                            lst_reporte_2_rubros = db.total_Ejecutado_Por_Rubro_Corporativo(codigo_pais, ejer, model.listado_periodos_2, model.id_tipo_cambio).ToList();
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
                            
                            lst_reporte_1_rubros = db.sp_rubro_corporativos_valores_presupuesto_periodos_CEAM(string_presupuesto_ventas_1, model.listado_periodos_1).ToList();
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

                          
                            lst_reporte_2_rubros = db.sp_rubro_corporativos_valores_presupuesto_periodos_CEAM(string_presupuesto_ventas_2, model.listado_periodos_2).ToList();
                        }
                        if (model.id_tipo_datos_1 == 1)
                        {
                            lst_reporte_1_rubros = db.total_Ejecutado_Por_Rubro_Corporativo("ceam", model.anio_1, model.listado_periodos_1, model.id_tipo_cambio).ToList();
                        }
                        if (model.id_tipo_datos_2 == 1)
                        {
                            lst_reporte_2_rubros = db.total_Ejecutado_Por_Rubro_Corporativo("ceam", model.anio_2, model.listado_periodos_2, model.id_tipo_cambio).ToList();
                        }
                    }


                    reportes_areas.Intereses_Ganados_P1 = 0;
                    reportes_areas.Intereses_Ganados_P2 = 0;
                    reportes_areas.Intereses_Perdidos_P1 = 0;
                    reportes_areas.Intereses_Perdidos_P2 = 0;
                    reportes_areas.Otros_Resultados_Financieros_P1 = 0;
                    reportes_areas.Otros_Resultados_Financieros_P2 = 0;
                    reportes_areas.Result_Ejercicios_Anteriores_P1 = 0;
                    reportes_areas.Result_Ejercicios_Anteriores_P2 = 0;
                    reportes_areas.Resultados_Diversos_P1 = 0;
                    reportes_areas.Resultados_Diversos_P2 = 0;
                    reportes_areas.Gan__por_Conversion_P1 = 0;
                    reportes_areas.Gan__por_Conversion_P2 = 0;
                    reportes_areas.Impuesto_a_la_Renta_P1 = 0;
                    reportes_areas.Impuesto_a_la_Renta_P2 = 0;

                    foreach (var dto in lst_reporte_1_rubros)
                    {
                        if (dto.rubro == "C06")
                        {
                            reportes_areas.Intereses_Ganados_P1 = reportes_areas.Intereses_Ganados_P1+ dto.total*-1 / 1000; ;
                        }
                        else if (dto.rubro == "C07")
                        {
                            reportes_areas.Intereses_Perdidos_P1 = reportes_areas.Intereses_Perdidos_P1+ dto.total*-1 / 1000; ;
                        }
                        else if (dto.rubro == "C08")
                        {
                            reportes_areas.Otros_Resultados_Financieros_P1 = reportes_areas.Otros_Resultados_Financieros_P1+dto.total * -1 / 1000; ;
                        }
                        else if (dto.rubro == "C09")
                        {
                            reportes_areas.Result_Ejercicios_Anteriores_P1 = reportes_areas.Result_Ejercicios_Anteriores_P1+ dto.total / 1000; ;
                        }
                        else if (dto.rubro == "C10")
                        {
                            reportes_areas.Resultados_Diversos_P1 = reportes_areas.Resultados_Diversos_P1 + dto.total*-1 / 1000; ;
                        }
                        else if (dto.rubro == "C11")
                        {
                            reportes_areas.Gan__por_Conversion_P1 = reportes_areas.Gan__por_Conversion_P1+ dto.total*-1 / 1000; ;
                        }
                        else if (dto.rubro == "C12")
                        {
                            reportes_areas.Impuesto_a_la_Renta_P1 = reportes_areas.Impuesto_a_la_Renta_P1+dto.total * -1 / 1000; ;
                        }
                    }

                    foreach (var dto in lst_reporte_2_rubros)
                    {
                        if (dto.rubro == "C06")
                        {
                            reportes_areas.Intereses_Ganados_P2 = reportes_areas.Intereses_Ganados_P2+dto.total*-1 / 1000; ;
                        }
                        else if (dto.rubro == "C07")
                        {
                            reportes_areas.Intereses_Perdidos_P2 = reportes_areas.Intereses_Perdidos_P2 + dto.total*-1 / 1000; ;
                        }
                        else if (dto.rubro == "C08")
                        {
                            reportes_areas.Otros_Resultados_Financieros_P2 = reportes_areas.Otros_Resultados_Financieros_P2 + dto.total*-1 / 1000; ;
                        }
                        else if (dto.rubro == "C09")
                        {
                            reportes_areas.Result_Ejercicios_Anteriores_P2 = reportes_areas.Result_Ejercicios_Anteriores_P2+ dto.total / 1000; ;
                        }
                        else if (dto.rubro == "C10")
                        {
                            reportes_areas.Resultados_Diversos_P2 = reportes_areas.Resultados_Diversos_P2 +dto.total*-1 / 1000; ;
                        }
                        else if (dto.rubro == "C11")
                        {
                            reportes_areas.Gan__por_Conversion_P2 = reportes_areas.Gan__por_Conversion_P2+ dto.total * -1 / 1000; ;
                        }
                        else if (dto.rubro == "C12")
                        {
                            reportes_areas.Impuesto_a_la_Renta_P2 = reportes_areas.Impuesto_a_la_Renta_P2+ dto.total * -1 / 1000; ;
                        }
                    }

                    reportes_areas.Resultados_Financieros_P1 = reportes_areas.Intereses_Ganados_P1 + reportes_areas.Intereses_Perdidos_P1 + reportes_areas.Otros_Resultados_Financieros_P1;
                    reportes_areas.Resultados_Financieros_P2 = reportes_areas.Intereses_Ganados_P2 + reportes_areas.Intereses_Perdidos_P2 + reportes_areas.Otros_Resultados_Financieros_P2;

                    reportes_areas.Resultado_antes_de_impuestos_P1 = reportes_areas.Resultado_de_Explotacion_P1+ reportes_areas.Resultados_Financieros_P1 + reportes_areas.Result_Ejercicios_Anteriores_P1 + reportes_areas.Resultados_Diversos_P1 + reportes_areas.Gan__por_Conversion_P1;
                    reportes_areas.Resultado_antes_de_impuestos_P2 = reportes_areas.Resultado_de_Explotacion_P2 + reportes_areas.Resultados_Financieros_P2 + reportes_areas.Result_Ejercicios_Anteriores_P2 + reportes_areas.Resultados_Diversos_P2 + reportes_areas.Gan__por_Conversion_P2;

                    reportes_areas.Resultado_Neto_P1 = reportes_areas.Resultado_antes_de_impuestos_P1 + reportes_areas.Impuesto_a_la_Renta_P1;
                    reportes_areas.Resultado_Neto_P2 = reportes_areas.Resultado_antes_de_impuestos_P2 + reportes_areas.Impuesto_a_la_Renta_P2;

                    oR.result = 1;
                    oR.data = reportes_areas;

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
