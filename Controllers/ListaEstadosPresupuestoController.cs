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
    public class ListaEstadosPresupuestoController : BaseController
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
                    if (ParametroConversion == 0)
                    {
                        List<EstadosPresupuestoViewModel> lst = (from d in db.estados_presupuesto
                                                          where d.estado == 1
                                                          select new EstadosPresupuestoViewModel
                                                          {
                                                              id = d.id,
                                                              nombre = d.nombre,
                                                              comentarios = d.comentarios,
                                                              estado = d.estado,
                                                          }).ToList();
                        oR.result = 1;
                        oR.data = lst;

                    }
                    else
                    {
                        List<EstadosPresupuestoViewModel> lst = (from d in db.estados_presupuesto
                                                                 where d.estado == 1 & d.id == ParametroConversion
                                                          select new EstadosPresupuestoViewModel
                                                          {
                                                              id = d.id,
                                                              nombre = d.nombre,
                                                              comentarios = d.comentarios,
                                                              estado = d.estado,
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
