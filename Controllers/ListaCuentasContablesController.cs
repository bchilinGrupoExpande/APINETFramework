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
    public class ListaCuentasContablesController : BaseController
    {
        [HttpPut]
        public Reply Eliminar([FromBody] CuentasContablesViewModel model)
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
                    cuentas_contables oCuentasContables = db.cuentas_contables.Find(model.id);
                    oCuentasContables.estado = 0;
                    db.Entry(oCuentasContables).State = System.Data.Entity.EntityState.Modified;
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
                    var dto_usuario = db.usuarios.Where(d => d.id == model.IDUsuario).First();
                    var user_pais = dto_usuario.id_pais;
                    if (ParametroConversion == 0)
                    {
                        if (user_pais == 1)
                        {
                            List<CuentasContablesViewModel> lst = (from d in db.cuentas_contables
                                                                   where d.estado == 1
                                                                   select new CuentasContablesViewModel
                                                                   {
                                                                       id = d.id,
                                                                       codigo = d.codigo,
                                                                       descripcion = d.descripcion,
                                                                       id_tipo_cuenta = d.id_tipo_cuenta,
                                                                       id_pais = d.id_pais,
                                                                       estado = d.estado,
                                                                       nombre_pais = d.pais.nombre,
                                                                       nombre_tipo_cuenta = d.tipo_cuentas.nombre,
                                                                   }).ToList();
                            oR.result = 1;
                            oR.data = lst;
                        }
                        else
                        {
                            List<CuentasContablesViewModel> lst = (from d in db.cuentas_contables
                                                                   where d.estado == 1 && d.id_pais==user_pais
                                                                   select new CuentasContablesViewModel
                                                                   {
                                                                       id = d.id,
                                                                       codigo = d.codigo,
                                                                       descripcion = d.descripcion,
                                                                       id_tipo_cuenta = d.id_tipo_cuenta,
                                                                       id_pais = d.id_pais,
                                                                       estado = d.estado,
                                                                       nombre_pais = d.pais.nombre,
                                                                       nombre_tipo_cuenta = d.tipo_cuentas.nombre,
                                                                   }).ToList();
                            oR.result = 1;
                            oR.data = lst;
                        }
                           

                    }
                    else
                    {
                        List<CuentasContablesViewModel> lst = (from d in db.cuentas_contables
                                                               where d.estado == 1 & d.id == ParametroConversion
                                                                             select new CuentasContablesViewModel
                                                                             {
                                                                                 id = d.id,
                                                                                 codigo = d.codigo,
                                                                                 descripcion = d.descripcion,
                                                                                 id_tipo_cuenta = d.id_tipo_cuenta,
                                                                                 id_pais = d.id_pais,
                                                                                 estado = d.estado,
                                                                                 nombre_pais = d.pais.nombre,
                                                                                 nombre_tipo_cuenta = d.tipo_cuentas.nombre,
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
