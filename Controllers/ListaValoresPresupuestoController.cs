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
    public class ListaValoresPresupuestoController : BaseController
    {
        [HttpPut]
        public Reply Eliminar([FromBody] ValoresPresupuestoViewModel model)
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
                    valores_presupuesto oValoresPresupuesto = db.valores_presupuesto.Find(model.id);
                    oValoresPresupuesto.estado = 0;
                    db.Entry(oValoresPresupuesto).State = System.Data.Entity.EntityState.Modified;
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

        [HttpPost]
        public Reply Listar([FromBody] SecurityViewModel model)
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
                var ParametroConversion = Convert.ToInt32(model.Parametro);
                using (PresupuestosPruebaEntities db = new PresupuestosPruebaEntities())
                {
                    if (ParametroConversion == 0)
                    {
                        double tipo_cambio_presupuesto = 1;

                        if (model.id_tipo_cambio == 0)
                        {

                        }
                        else
                        {
                            var lst_moneda_presupuesto = db.monedas_presupuesto.Where(d => d.id_presupuesto_asignado == ParametroConversion);


                            foreach (var dto in lst_moneda_presupuesto)
                            {
                                tipo_cambio_presupuesto = dto.tasa_cambio_presupuesto;
                            }

                        }

                        List<ValoresPresupuestoViewModel> lst = (from d in db.valores_presupuesto
                                                                  where d.estado == 1
                                                                  select new ValoresPresupuestoViewModel
                                                                  {
                                                                      id = d.id,
                                                                      id_presupuesto = d.id_presupuesto,
                                                                      id_estructura_contable = d.id_estructura_contable,
                                                                      id_moneda = d.id_moneda,
                                                                      valor_presupuestado = d.valor_presupuestado / tipo_cambio_presupuesto,
                                                                      fecha_creacion_valor_presupuesto = d.fecha_creacion_valor_presupuesto,
                                                                      comentarios = d.comentarios,
                                                                      periodo_1 = d.periodo_1 / tipo_cambio_presupuesto,
                                                                      periodo_2 = d.periodo_2 / tipo_cambio_presupuesto,
                                                                      periodo_3 = d.periodo_3 / tipo_cambio_presupuesto,
                                                                      periodo_4 = d.periodo_4 / tipo_cambio_presupuesto,
                                                                      periodo_5 = d.periodo_5 / tipo_cambio_presupuesto,
                                                                      periodo_6 = d.periodo_6 / tipo_cambio_presupuesto,
                                                                      periodo_7 = d.periodo_7 / tipo_cambio_presupuesto,
                                                                      periodo_8 = d.periodo_8 / tipo_cambio_presupuesto,
                                                                      periodo_9 = d.periodo_9 / tipo_cambio_presupuesto,
                                                                      periodo_10 = d.periodo_10 / tipo_cambio_presupuesto,
                                                                      periodo_11 = d.periodo_11 / tipo_cambio_presupuesto,
                                                                      periodo_12 = d.periodo_12 / tipo_cambio_presupuesto,
                                                                      nombre_area =d.estructuras_contables.area.codigo,
                                                                      nombre_linea = d.estructuras_contables.linea.codigo,
                                                                      nombre_cuenta_contable = d.estructuras_contables.cuentas_contables.codigo,
                                                                      nombre_centro_costo = d.estructuras_contables.centros_de_costo.codigo,
                                                                      nombre_rubro_corporativo = d.estructuras_contables.rubros_contables_corporativos.codigo,
                                                                      estado = 1,
                                                                  }).ToList();
                        oR.result = 1;
                        oR.data = lst;

                    }
                    else
                    {
                        double tipo_cambio_presupuesto = 1;

                        if (model.id_tipo_cambio == 0)
                        {

                        }
                        else
                        {
                            var lst_moneda_presupuesto = db.monedas_presupuesto.Where(d => d.id_presupuesto_asignado == ParametroConversion);


                            foreach (var dto in lst_moneda_presupuesto)
                            {
                                tipo_cambio_presupuesto = dto.tasa_cambio_presupuesto;
                            }

                        }

                        List<ValoresPresupuestoViewModel> lst = (from d in db.valores_presupuesto
                                                                 where d.estado == 1 & d.id_presupuesto == ParametroConversion
          
                                                                  select new ValoresPresupuestoViewModel
                                                                  {
                                                                      id = d.id,
                                                                      id_presupuesto = d.id_presupuesto,
                                                                      id_estructura_contable = d.id_estructura_contable,
                                                                      id_moneda = d.id_moneda,
                                                                      valor_presupuestado = d.valor_presupuestado / tipo_cambio_presupuesto,
                                                                      fecha_creacion_valor_presupuesto = d.fecha_creacion_valor_presupuesto,
                                                                      comentarios = d.comentarios,
                                                                      estado = 1,
                                                                      periodo_1 = d.periodo_1/ tipo_cambio_presupuesto,
                                                                      periodo_2 = d.periodo_2/ tipo_cambio_presupuesto,
                                                                      periodo_3 = d.periodo_3/ tipo_cambio_presupuesto,
                                                                      periodo_4 = d.periodo_4 / tipo_cambio_presupuesto,
                                                                      periodo_5 = d.periodo_5 / tipo_cambio_presupuesto,
                                                                      periodo_6 = d.periodo_6 / tipo_cambio_presupuesto,
                                                                      periodo_7 = d.periodo_7 / tipo_cambio_presupuesto,
                                                                      periodo_8 = d.periodo_8 / tipo_cambio_presupuesto,
                                                                      periodo_9 = d.periodo_9 / tipo_cambio_presupuesto,
                                                                      periodo_10 = d.periodo_10 / tipo_cambio_presupuesto,
                                                                      periodo_11 = d.periodo_11 / tipo_cambio_presupuesto,
                                                                      periodo_12 = d.periodo_12 / tipo_cambio_presupuesto,
                                                                      nombre_area = d.estructuras_contables.area.codigo,
                                                                      nombre_linea = d.estructuras_contables.linea.codigo,
                                                                      nombre_cuenta_contable = d.estructuras_contables.cuentas_contables.codigo,
                                                                      nombre_centro_costo = d.estructuras_contables.centros_de_costo.codigo,
                                                                      nombre_rubro_corporativo = d.estructuras_contables.rubros_contables_corporativos.codigo,
                                                                  }).ToList();
                  
                        oR.result = 1;
                        oR.data = lst;


                    }
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
