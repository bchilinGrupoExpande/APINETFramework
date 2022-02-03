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
    public class ListaPaisesController : BaseController
    {
        [HttpPut]
        public Reply Eliminar([FromBody] PaisesViewModel model)
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
                    pais oPais = db.paises.Find(model.id);
                    oPais.estado = 0;
                    db.Entry(oPais).State = System.Data.Entity.EntityState.Modified;
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
                oR.result = 0;
                return oR;
            }
            try
            {
                var ParametroConversion = Convert.ToInt32(model.Parametro);
               
                using (PresupuestosPruebaEntities db = new PresupuestosPruebaEntities())
                {
                    var dto_usuario = db.usuarios.Where(d=>d.id==model.IDUsuario).First();
                    var user_pais = dto_usuario.id_pais;

                    if (ParametroConversion == 0)
                    {
                        if (user_pais == 1)
                        {
                            List<PaisesViewModel> lst = (from d in db.paises
                                                         where d.estado == 1
                                                         select new PaisesViewModel
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
                            List<PaisesViewModel> lst = (from d in db.paises
                                                         where d.estado == 1 && d.id== user_pais
                                                         select new PaisesViewModel
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
                    else
                    {
                        List<PaisesViewModel> lst = (from d in db.paises
                                                     where d.estado == 1 & d.id == ParametroConversion
                                                     select new PaisesViewModel
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
