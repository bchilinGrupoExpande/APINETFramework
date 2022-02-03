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
    public class ListaUsuariosController : BaseController
    {
        [HttpPut]
        public Reply Eliminar([FromBody]UsuariosViewModel model)
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
                    usuario oUsuario = db.usuarios.Find(model.id);
                    oUsuario.estado = 0;
                    db.Entry(oUsuario).State = System.Data.Entity.EntityState.Modified;
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
                            List<UsuariosViewModel> lst = (from d in db.usuarios
                                                           where d.estado == 1
                                                           select new UsuariosViewModel
                                                           {
                                                               id = d.id,
                                                               correo_electronico = d.correo_electronico,
                                                               comentarios = d.comentarios,

                                                               id_rol = d.id_rol,
                                                               id_pais = d.id_pais,
                                                               estado = d.estado,
                                                               nombre = d.nombre,
                                                               apellido = d.apellido,
                                                               nombre_completo = d.nombre + " " + d.apellido,
                                                               nombre_pais = d.pais.nombre,
                                                               nombre_rol = d.roles_usuarios.nombre,
                                                           }).ToList();
                            oR.result = 1;
                            oR.data = lst;
                        }
                        else
                        {
                            List<UsuariosViewModel> lst = (from d in db.usuarios
                                                           where d.estado == 1 && d.id_pais==user_pais
                                                           select new UsuariosViewModel
                                                           {
                                                               id = d.id,
                                                               correo_electronico = d.correo_electronico,
                                                               comentarios = d.comentarios,

                                                               id_rol = d.id_rol,
                                                               id_pais = d.id_pais,
                                                               estado = d.estado,
                                                               nombre = d.nombre,
                                                               apellido = d.apellido,
                                                               nombre_completo = d.nombre + " " + d.apellido,
                                                               nombre_pais = d.pais.nombre,
                                                               nombre_rol = d.roles_usuarios.nombre,
                                                           }).ToList();
                            oR.result = 1;
                            oR.data = lst;
                        }
                      

                    }
                    else
                    {
                        List<UsuariosViewModel> lst = (from d in db.usuarios
                                                             where d.estado == 1 & d.id == ParametroConversion
                                                             select new UsuariosViewModel
                                                             {
                                                                 id = d.id,
                                                                 correo_electronico = d.correo_electronico,
                                                                 comentarios = d.comentarios,
                                                               
                                                                 id_rol = d.id_rol,
                                                                 id_pais = d.id_pais,
                                                                 estado = d.estado,
                                                                 nombre = d.nombre,
                                                                 apellido = d.apellido,
                                                                 nombre_completo = d.nombre + " " + d.apellido,
                                                                 nombre_pais = d.pais.nombre,
                                                                 nombre_rol = d.roles_usuarios.nombre,
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
