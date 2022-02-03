using APIPresupuestos.Models;
using ConectarDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIPresupuestos.Controllers
{
    public class ListaRubrosContablesCorporativosController : BaseController
    {
        [HttpPut]
        public Reply Eliminar([FromBody] RubrosContablesCorporativosViewModel model)
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
                    rubros_contables_corporativos oRubrosCorporativosContables = db.rubros_contables_corporativos.Find(model.id);
                    oRubrosCorporativosContables.estado = 0;
                    db.Entry(oRubrosCorporativosContables).State = System.Data.Entity.EntityState.Modified;
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
                        List<RubrosContablesCorporativosViewModel> lst = (from d in db.rubros_contables_corporativos
                                                          where d.estado == 1
                                                          select new RubrosContablesCorporativosViewModel
                                                          {
                                                              id = d.id,
                                                              codigo = d.codigo,
                                                              id_concepto_corporativo = d.id_concepto_corporativo,
                                                              descripcion = d.descripcion,
                                                              estado = d.estado,
                                                              nombre_concepto_corporativo=d.conceptos_contables_corporativos.codigo
                                                          }).ToList();
                        oR.result = 1;
                        oR.data = lst;

                    }
                    else
                    {
                        List<RubrosContablesCorporativosViewModel> lst = (from d in db.rubros_contables_corporativos
                                                                          where d.estado == 1 & d.id == ParametroConversion
                                                          select new RubrosContablesCorporativosViewModel
                                                          {
                                                              id = d.id,
                                                              codigo = d.codigo,
                                                              id_concepto_corporativo = d.id_concepto_corporativo,
                                                              descripcion = d.descripcion,
                                                              estado = d.estado,
                                                              nombre_concepto_corporativo = d.conceptos_contables_corporativos.codigo
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
