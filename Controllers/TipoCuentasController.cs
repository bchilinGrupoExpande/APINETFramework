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
    public class TipoCuentasController : BaseController
    {


        [HttpPost]
        public Reply Agregar([FromBody] TipoCuentasViewModel model)
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
                    tipo_cuentas oTipoCuentas = new tipo_cuentas();

                    oTipoCuentas.nombre = model.nombre;
                    oTipoCuentas.descripcion = model.descripcion;
                    oTipoCuentas.estado = 1;
                    db.tipo_cuentas.Add(oTipoCuentas);
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
        public Reply Editar([FromBody] TipoCuentasViewModel model)
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
                    tipo_cuentas oTipoCuentas = db.tipo_cuentas.Find(model.id);

                    oTipoCuentas.nombre = model.nombre;
                    oTipoCuentas.descripcion = model.descripcion;
                    oTipoCuentas.estado = 1;

                    db.Entry(oTipoCuentas).State = System.Data.Entity.EntityState.Modified;
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
