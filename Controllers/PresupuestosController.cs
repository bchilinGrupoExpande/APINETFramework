using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APIPresupuestos.Models;
using ConectarDatos;
using System.Data.Entity.Core.Objects;

namespace APIPresupuestos.Controllers
{
    public class PresupuestosController : BaseController
    {
        [HttpPost]
        public Reply Agregar([FromBody] PresupuestosViewModel model)
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
                presupuesto oPresupuestos = new presupuesto();

                using (PresupuestosPruebaEntities db = new PresupuestosPruebaEntities())
                {

                    if (db.presupuestos.Where(d=>d.estado==1 && d.id_pais==model.id_pais && d.id_tipo_presupuesto == model.id_tipo_presupuesto && d.anio==model.anio).Count() >= 1)
                    {
                        oR.data = "[]";
                        oR.result = 0;
                        oR.message = "Ya existe este tipo presupuesto para el Año y País Seleccionado, por favor intente de nuevo";
                        return oR;
                    }
                    else
                    {
                        oPresupuestos.codigo = model.codigo;
                        oPresupuestos.nombre = model.nombre;
                        oPresupuestos.anio = model.anio;
                        oPresupuestos.cantidad_periodos = model.cantidad_periodos;
                        oPresupuestos.id_estado_actual_presupuesto = model.id_estado_actual_presupuesto;
                        oPresupuestos.id_pais = model.id_pais;
                        oPresupuestos.id_moneda = model.id_moneda;
                        oPresupuestos.id_usuario_creacion = 2;
                        oPresupuestos.id_tipo_presupuesto = model.id_tipo_presupuesto;
                        oPresupuestos.fecha_creacion = DateTime.Now.ToString();
                        oPresupuestos.comentarios = model.comentarios;
                        oPresupuestos.estado = 1;
                        db.presupuestos.Add(oPresupuestos);
                        db.SaveChanges();
                        oR.message = "Se ha creado el Presupuesto Correctamente";
                        oR.data = "[]";
                        oR.result = 1;
                    }

                
                }
                using (PresupuestosPruebaEntities db2 = new PresupuestosPruebaEntities())
                {
                    var tipo_cambio = 0.0;
                    var id_moneda = 1;
                    var moneda = db2.monedas_presupuesto.Where(d => d.id == model.id_moneda & d.estado == 1).ToList();
                    foreach (var dto in moneda)
                    {
                        monedas_presupuesto oMoneda = db2.monedas_presupuesto.Find(dto.id);
                        oMoneda.estado_moneda = "Asignada a un Presupuesto";
                        oMoneda.id_presupuesto_asignado = oPresupuestos.id;
                        tipo_cambio = dto.tasa_cambio_presupuesto;
                        id_moneda = dto.id;
                        db2.Entry(oMoneda).State = System.Data.Entity.EntityState.Modified;
                        db2.SaveChanges();
                    }

                    monedas_ejecutado oMonedasEjecutado = new monedas_ejecutado();
                    var pais = db2.paises.Where(d => d.id == model.id_pais).First();
                    var nombre_pais = pais.nombre;
                    for (int i = 1; i < 12; i++)
                    {

                            if (db2.monedas_ejecutado.Where(d=>d.Anio==oPresupuestos.anio && d.Pais== nombre_pais && d.Mes==i).Count()>0)
                           {

                            }else
                               {
                                    oMonedasEjecutado.Anio = oPresupuestos.anio;
                                    oMonedasEjecutado.Pais = nombre_pais;
                                    oMonedasEjecutado.Mes = i;
                                    oMonedasEjecutado.TipoCambio = Convert.ToDecimal(tipo_cambio);
                                    db2.monedas_ejecutado.Add(oMonedasEjecutado);
                                    db2.SaveChanges();
            
                              }
                        }

                }

                using (PresupuestosPruebaEntities db3 = new PresupuestosPruebaEntities())
                {

                    var lst = db3.estructuras_contables.Where(d => d.id_pais == 1 && d.estado == 1 && d.id_tipo_estructura_contable==model.id_tipo_presupuesto).ToList();

                    valores_presupuesto oValoresPresupuesto = new valores_presupuesto();
                   
                    //Recorremos todas las estructuras contables y las asignamos

                        foreach (var dto in lst)
                        {
                                oValoresPresupuesto.id_presupuesto = oPresupuestos.id;
                                oValoresPresupuesto.id_estructura_contable = dto.id;
                                oValoresPresupuesto.id_moneda = model.id_moneda;
                                oValoresPresupuesto.valor_presupuestado = 0;
                                oValoresPresupuesto.periodo_1 = 0;
                                oValoresPresupuesto.periodo_2 = 0;
                                oValoresPresupuesto.periodo_3 = 0;
                                oValoresPresupuesto.periodo_4 = 0;
                                oValoresPresupuesto.periodo_5 = 0;
                                oValoresPresupuesto.periodo_6 = 0;
                                oValoresPresupuesto.periodo_7 = 0;
                                oValoresPresupuesto.periodo_8 = 0;
                                oValoresPresupuesto.periodo_9 = 0;
                                oValoresPresupuesto.periodo_10 = 0;
                                oValoresPresupuesto.periodo_11 = 0;
                                oValoresPresupuesto.periodo_12 = 0;
                                oValoresPresupuesto.valor_presupuestado = 0;
                                oValoresPresupuesto.fecha_creacion_valor_presupuesto = DateTime.Now.ToString();
                                oValoresPresupuesto.comentarios = "Creado Automaticamente";
                                oValoresPresupuesto.estado = 1;
                                db3.valores_presupuesto.Add(oValoresPresupuesto);
                                db3.SaveChanges();
                        }
                        
                    
                }
                
            }
            catch (Exception ex)
            {
                oR.message = "Ocurrió un error en el servidor, intenta más tarde";
            }
            return oR;
        }

        [HttpPut]
        public Reply Editar([FromBody] PresupuestosViewModel model)
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
                    presupuesto oPresupuestos = db.presupuestos.Find(model.id);

                    oPresupuestos.codigo = model.codigo;
                    oPresupuestos.nombre = model.nombre;
                    oPresupuestos.id_estado_actual_presupuesto = model.id_estado_actual_presupuesto;
                    oPresupuestos.id_tipo_presupuesto = model.id_tipo_presupuesto;
                    oPresupuestos.comentarios = model.comentarios;
                    oPresupuestos.estado = 1;

                    db.Entry(oPresupuestos).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    oR.data = "[]";
                    oR.result = 1;

                }
            }
            catch (Exception ex)
            {
                oR.message = "Ocurrió un error en el servidor, intenta más tarde";
            }
            return oR;
        }
    }
}
