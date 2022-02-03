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
    public class TipoPresupuestosController : BaseController
    {
        [HttpPost]
        public Reply Agregar([FromBody] TipoPresupuestosViewModel model)
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
                    tipo_presupuestos oTipoPresupuesto = new tipo_presupuestos();

                    oTipoPresupuesto.nombre = model.nombre;
                    oTipoPresupuesto.descripcion = model.descripcion;
                    oTipoPresupuesto.estado = 1;
                    db.tipo_presupuestos.Add(oTipoPresupuesto);
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
        public Reply Editar([FromBody] TipoPresupuestosViewModel model)
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
                    tipo_presupuestos oTipoPresupuesto = db.tipo_presupuestos.Find(model.id);

                    oTipoPresupuesto.nombre = model.nombre;
                    oTipoPresupuesto.descripcion = model.descripcion;
                    oTipoPresupuesto.estado = 1;

                    db.Entry(oTipoPresupuesto).State = System.Data.Entity.EntityState.Modified;
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
