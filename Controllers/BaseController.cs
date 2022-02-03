using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ConectarDatos;

namespace APIPresupuestos.Controllers
{
    public class BaseController : ApiController
    {
        public bool Verify(string token, int? IDUsuario)
        {
            using (PresupuestosPruebaEntities db = new PresupuestosPruebaEntities())
            {
                if (db.usuarios.Where(d => d.token == token && d.estado == 1 && d.id == IDUsuario).Count() > 0)
                    return true;
            }
            return false;
        }


    }
}
