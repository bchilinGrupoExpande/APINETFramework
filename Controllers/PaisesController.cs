using APIPresupuestos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ConectarDatos;

namespace APIPresupuestos.Controllers
{
    public class PaisesController : BaseController
    {
       

        [HttpPost]
        public Reply Agregar([FromBody] PaisesViewModel model)
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
                    pais oPais = new pais();

                    oPais.nombre = model.nombre;
                    oPais.descripcion = model.descripcion;
                    oPais.estado = 1;
                    db.paises.Add(oPais);
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
        public Reply Editar([FromBody]PaisesViewModel model)
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

                    oPais.nombre = model.nombre;
                    oPais.descripcion = model.descripcion;
                    oPais.estado = 1;

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

      


    }
}
