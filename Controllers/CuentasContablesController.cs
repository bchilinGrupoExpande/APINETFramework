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
    public class CuentasContablesController : BaseController
    {
        [HttpPost]
        public Reply Agregar([FromBody] CuentasContablesViewModel model)
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
                    cuentas_contables oCuentasContables = new cuentas_contables();

                    oCuentasContables.codigo = model.codigo;
                    oCuentasContables.descripcion = model.descripcion;
                    oCuentasContables.id_tipo_cuenta = model.id_tipo_cuenta;
                    oCuentasContables.id_pais = model.id_pais;
                    oCuentasContables.estado = 1;
                    db.cuentas_contables.Add(oCuentasContables);
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
        public Reply Editar([FromBody] CuentasContablesViewModel model)
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
                    cuentas_contables oCuentasContables = db.cuentas_contables.Find(model.id);

                    oCuentasContables.codigo = model.codigo;
                    oCuentasContables.descripcion = model.descripcion;
                    oCuentasContables.id_tipo_cuenta = model.id_tipo_cuenta;
                    oCuentasContables.id_pais = model.id_pais;
                    oCuentasContables.estado = 1;

                    db.Entry(oCuentasContables).State = System.Data.Entity.EntityState.Modified;
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
