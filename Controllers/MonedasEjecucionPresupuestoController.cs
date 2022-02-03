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
    public class MonedasEjecucionPresupuestoController : BaseController
    {
        [HttpPut]
        public Reply Editar([FromBody] MonedasEjecucionPresupuestoViewModel model)
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
                    monedas_ejecutado oMonedas_Ejecutado = db.monedas_ejecutado.Find(model.id);

                    oMonedas_Ejecutado.TipoCambio = model.TipoCambio;

                    db.Entry(oMonedas_Ejecutado).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var pais_ = db.paises.Where(d => d.nombre == oMonedas_Ejecutado.Pais).First();

                    oR.data = oMonedas_Ejecutado.Anio.ToString();
                    oR.result = pais_.id;

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
