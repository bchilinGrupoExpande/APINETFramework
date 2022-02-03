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
    public class LineasController : BaseController
    {
        [HttpPost]
        public Reply Agregar([FromBody] LineasViewModel model)
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
                    linea oLinea = new linea();

                    oLinea.codigo = model.codigo;
                    oLinea.descripcion = model.descripcion;
                    oLinea.id_pais = model.id_pais;
                    oLinea.id_unidad_negocio = model.id_unidad_negocio;
                    oLinea.estado = 1;

                    db.lineas.Add(oLinea);
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
        public Reply Editar([FromBody] LineasViewModel model)
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
                    linea oLinea = db.lineas.Find(model.id);

                    oLinea.codigo = model.codigo;
                    oLinea.descripcion = model.descripcion;
                    oLinea.id_pais = model.id_pais;
                    oLinea.id_unidad_negocio = model.id_unidad_negocio;
                    oLinea.estado = 1;

                    db.Entry(oLinea).State = System.Data.Entity.EntityState.Modified;
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
