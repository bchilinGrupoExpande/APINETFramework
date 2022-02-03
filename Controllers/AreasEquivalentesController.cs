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
    public class AreasEquivalentesController : BaseController
    {
        [HttpPost]
        public Reply Agregar([FromBody] AreasEquivalentesViewModel model)
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
                    areas_equivalentes oAreasEquivalentes = new areas_equivalentes();

                    oAreasEquivalentes.nombre = model.nombre;
                    oAreasEquivalentes.descripcion = model.descripcion;
                    oAreasEquivalentes.estado = 1;
                    db.areas_equivalentes.Add(oAreasEquivalentes);
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
        public Reply Editar([FromBody] AreasEquivalentesViewModel model)
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
                    areas_equivalentes oAreasEquivalentes = db.areas_equivalentes.Find(model.id);

                    oAreasEquivalentes.nombre = model.nombre;
                    oAreasEquivalentes.descripcion = model.descripcion;
                    oAreasEquivalentes.estado = 1;

                    db.Entry(oAreasEquivalentes).State = System.Data.Entity.EntityState.Modified;
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
