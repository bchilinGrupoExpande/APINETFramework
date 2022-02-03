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
    public class HomeController : BaseController
    {
        [HttpPost]
        public Reply SesionActiva([FromBody]SecurityViewModel model)
        {
            Reply oR = new Reply();
            oR.result = 0;

            if (!Verify(model.token, model.IDUsuario))
            {
                oR.message = "No";
                return oR;
            }
            oR.message = "Si";
            oR.data = "";
            oR.result = 1;
            return oR;
        }



        [HttpPut]
        public Reply CerrarSesion([FromBody] SecurityViewModel model)
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
                    var lst = db.usuarios.Where(d => d.id == model.IDUsuario && d.token == model.token && d.estado == 1);
                    usuario oUsuario = lst.First();
                    oUsuario.sesion_activa = 0;
                    db.Entry(oUsuario).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    oR.data = "Ha Cerrado Sesión";
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
