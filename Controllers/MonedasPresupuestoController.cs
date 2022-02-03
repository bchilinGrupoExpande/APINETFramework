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
    public class MonedasPresupuestoController : BaseController
    {
        [HttpPost]
        public Reply Agregar([FromBody] MonedasPresupuestoViewModel model)
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

                    monedas_presupuesto oMonedasPresupuesto = new monedas_presupuesto();
                                                                
                    oMonedasPresupuesto.codigo_moneda = model.codigo_moneda;
                    oMonedasPresupuesto.comentarios = model.comentarios;
                    oMonedasPresupuesto.id_pais = model.id_pais;
                    oMonedasPresupuesto.tasa_cambio_presupuesto = model.tasa_cambio_presupuesto;
                    oMonedasPresupuesto.estado = 1;
                    oMonedasPresupuesto.estado_moneda = "Pendiente de Asignar a un Presupuesto";

                    db.monedas_presupuesto.Add(oMonedasPresupuesto);
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
        public Reply Editar([FromBody] MonedasPresupuestoViewModel model)
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
                    monedas_presupuesto oMonedasPresupuesto = db.monedas_presupuesto.Find(model.id);

                    oMonedasPresupuesto.codigo_moneda = model.codigo_moneda;
                    oMonedasPresupuesto.comentarios = model.comentarios;
                    oMonedasPresupuesto.id_pais = model.id_pais;
                    oMonedasPresupuesto.tasa_cambio_presupuesto = model.tasa_cambio_presupuesto;
                    oMonedasPresupuesto.estado = 1;

                    db.Entry(oMonedasPresupuesto).State = System.Data.Entity.EntityState.Modified;
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
