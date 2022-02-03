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
    public class ListaMonedasPresupuestoSinAsignarController : BaseController
    {
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
                    var dto_usuario = db.usuarios.Where(d => d.id == model.IDUsuario).First();
                    var user_pais = dto_usuario.id_pais;

                    if (ParametroConversion == 0)
                    {
                        if (user_pais == 1)
                        {
                            List<MonedasPresupuestoViewModel> lst = (from d in db.monedas_presupuesto
                                                                     where d.estado == 1 &
                                                                     d.estado_moneda != "Asignada a un Presupuesto"
                                                                     select new MonedasPresupuestoViewModel
                                                                     {
                                                                         id = d.id,
                                                                         codigo_moneda = d.codigo_moneda,
                                                                         id_pais = d.id_pais,
                                                                         comentarios = d.comentarios,
                                                                         tasa_cambio_presupuesto = d.tasa_cambio_presupuesto,
                                                                         estado = d.estado,
                                                                         estado_moneda = d.estado_moneda,
                                                                         nombre_pais = d.pais.nombre,
                                                                     }).ToList();
                            oR.result = 1;
                            oR.data = lst;
                        }
                        else
                        {
                            List<MonedasPresupuestoViewModel> lst = (from d in db.monedas_presupuesto
                                                                     where d.estado == 1 &
                                                                     d.estado_moneda != "Asignada a un Presupuesto" && d.id_pais==user_pais
                                                                     select new MonedasPresupuestoViewModel
                                                                     {
                                                                         id = d.id,
                                                                         codigo_moneda = d.codigo_moneda,
                                                                         id_pais = d.id_pais,
                                                                         comentarios = d.comentarios,
                                                                         tasa_cambio_presupuesto = d.tasa_cambio_presupuesto,
                                                                         estado = d.estado,
                                                                         estado_moneda = d.estado_moneda,
                                                                         nombre_pais = d.pais.nombre,
                                                                     }).ToList();
                            oR.result = 1;
                            oR.data = lst;
                        }
                       

                    }
                    else
                    {
                        List<MonedasPresupuestoViewModel> lst = (from d in db.monedas_presupuesto
                                                                 where d.estado == 1 & d.id == ParametroConversion
                                                                 select new MonedasPresupuestoViewModel
                                                                 {
                                                                     id = d.id,
                                                                     codigo_moneda = d.codigo_moneda,
                                                                     id_pais = d.id_pais,
                                                                     comentarios = d.comentarios,
                                                                     tasa_cambio_presupuesto = d.tasa_cambio_presupuesto,
                                                                     estado = d.estado,
                                                                     nombre_pais = d.pais.nombre,
                                                                     estado_moneda = d.estado_moneda,
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
