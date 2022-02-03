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
    public class ListaAreasController : BaseController
    {
        [HttpPut]
        public Reply Eliminar([FromBody] AreasViewModel model)
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
                    area oAreas = db.areas.Find(model.id);
                    oAreas.estado = 0;
                    db.Entry(oAreas).State = System.Data.Entity.EntityState.Modified;
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
                            List<AreasViewModel> lst = (from d in db.areas
                                                        where d.estado == 1
                                                        select new AreasViewModel
                                                        {
                                                            id = d.id,
                                                            codigo = d.codigo,
                                                            id_pais = d.id_pais,
                                                            id_area_equivalente = d.id_area_equivalente,
                                                            descripcion = d.descripcion,
                                                            estado = d.estado,
                                                            nombre_pais = d.pais.nombre,
                                                            nombre_area_equivalente = d.areas_equivalentes.nombre,
                                                        }).ToList();
                            oR.result = 1;
                            oR.data = lst;
                        }
                        else
                        {
                            List<AreasViewModel> lst = (from d in db.areas
                                                        where d.estado == 1 && d.id_pais==user_pais
                                                        select new AreasViewModel
                                                        {
                                                            id = d.id,
                                                            codigo = d.codigo,
                                                            id_pais = d.id_pais,
                                                            id_area_equivalente = d.id_area_equivalente,
                                                            descripcion = d.descripcion,
                                                            estado = d.estado,
                                                            nombre_pais = d.pais.nombre,
                                                            nombre_area_equivalente = d.areas_equivalentes.nombre,
                                                        }).ToList();
                            oR.result = 1;
                            oR.data = lst;
                        }
                           

                    }
                    else
                    {
                        List<AreasViewModel> lst = (from d in db.areas
                                                    where d.estado == 1 & d.id == ParametroConversion
                                                             select new AreasViewModel
                                                             {
                                                                 id = d.id,
                                                                 codigo = d.codigo,
                                                                 id_pais = d.id_pais,
                                                                 id_area_equivalente = d.id_area_equivalente,
                                                                 descripcion = d.descripcion,
                                                                 estado = d.estado,
                                                                 nombre_pais = d.pais.nombre,
                                                                 nombre_area_equivalente = d.areas_equivalentes.nombre,
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
