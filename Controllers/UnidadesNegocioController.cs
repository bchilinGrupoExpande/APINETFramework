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
    public class UnidadesNegocioController : BaseController
    {
        [HttpPost]
        public Reply Agregar([FromBody] UnidadesNegocioViewModel model)
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
                    unidades_negocio oUnidadNegocio = new unidades_negocio();

                    oUnidadNegocio.nombre = model.nombre;
                    oUnidadNegocio.descripcion = model.descripcion;
                    oUnidadNegocio.estado = 1;
                    db.unidades_negocio.Add(oUnidadNegocio);
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
        public Reply Editar([FromBody] UnidadesNegocioViewModel model)
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
                    unidades_negocio oUnidadNegocio = db.unidades_negocio.Find(model.id);

                    oUnidadNegocio.nombre = model.nombre;
                    oUnidadNegocio.descripcion = model.descripcion;
                    oUnidadNegocio.estado = 1;

                    db.Entry(oUnidadNegocio).State = System.Data.Entity.EntityState.Modified;
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
