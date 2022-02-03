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
    public class AreasController : BaseController
    {
        [HttpPost]
        public Reply Agregar([FromBody] AreasViewModel model)
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
                    area oAreas = new area();

                    oAreas.codigo = model.codigo;
                    oAreas.descripcion = model.descripcion;
                    oAreas.id_pais = model.id_pais;
                    oAreas.id_area_equivalente = model.id_area_equivalente;
                    oAreas.estado = 1;

                    db.areas.Add(oAreas);
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

        [HttpPut]
        public Reply Editar([FromBody] AreasViewModel model)
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

                    oAreas.codigo = model.codigo;
                    oAreas.descripcion = model.descripcion;
                    oAreas.id_pais = model.id_pais;
                    oAreas.id_area_equivalente = model.id_area_equivalente;
                    oAreas.estado = 1;

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
    
    }
}
