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
    public class ListaRolesUsuariosController : BaseController
    {
        [HttpPut]
        public Reply Eliminar([FromBody] RolesUsuariosViewModel model)
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
                    roles_usuarios oRolesUsuarios = db.roles_usuarios.Find(model.id);
                    oRolesUsuarios.estado = 0;
                    db.Entry(oRolesUsuarios).State = System.Data.Entity.EntityState.Modified;
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
                        List<RolesUsuariosViewModel> lst = (from d in db.roles_usuarios
                                                               where d.estado == 1
                                                               select new RolesUsuariosViewModel
                                                               {
                                                                   id = d.id,
                                                                   nombre = d.nombre,
                                                                   descripcion = d.descripcion,
                                                                   estado = d.estado,
                                                               }).ToList();
                        oR.result = 1;
                        oR.data = lst;

                    }
                    else
                    {
                        List<RolesUsuariosViewModel> lst = (from d in db.roles_usuarios
                                                            where d.estado == 1 & d.id == ParametroConversion
                                                               select new RolesUsuariosViewModel
                                                               {
                                                                   id = d.id,
                                                                   nombre = d.nombre,
                                                                   descripcion = d.descripcion,
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
