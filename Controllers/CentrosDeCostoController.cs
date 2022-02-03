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
    public class CentrosDeCostoController : BaseController
    {
        [HttpPost]
        public Reply Agregar([FromBody]CentrosDeCostoViewModel model)
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
                    centros_de_costo oCentrosDeCosto = new centros_de_costo();

                    oCentrosDeCosto.codigo = model.codigo;
                    oCentrosDeCosto.descripcion = model.descripcion;
                    oCentrosDeCosto.id_pais = model.id_pais;
                    oCentrosDeCosto.estado = 1;

                    db.centros_de_costo.Add(oCentrosDeCosto);
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
        public Reply Editar([FromBody] CentrosDeCostoViewModel model)
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
                    centros_de_costo oCentrosDeCosto = db.centros_de_costo.Find(model.id);

                    oCentrosDeCosto.codigo = model.codigo;
                    oCentrosDeCosto.descripcion = model.descripcion;
                    oCentrosDeCosto.id_pais = model.id_pais;
                    oCentrosDeCosto.estado = 1;

                    db.Entry(oCentrosDeCosto).State = System.Data.Entity.EntityState.Modified;
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
