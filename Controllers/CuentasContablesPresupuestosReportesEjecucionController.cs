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
    public class CuentasContablesPresupuestosReportesEjecucionController : BaseController
    {
        [HttpPost]
        public Reply ListarCuentas([FromBody] SecurityViewModel model)
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
                var ParametroConversion = Convert.ToInt32(model.Parametro);
                using (PresupuestosPruebaEntities db = new PresupuestosPruebaEntities())
                {
                    if (ParametroConversion == 0)
                    {
                        List<CuentasContablesViewModel> lst = (from d in db.cuentas_contables
                                                               where d.estado == 1 & d.id_tipo_cuenta==1
                                                               select new CuentasContablesViewModel
                                                               {
                                                                   id = d.id,
                                                                   codigo = d.codigo+"-"+ d.descripcion,
                                                               }).ToList();
                        oR.result = 1;
                        oR.data = lst;

                    }
                    else
                    {
                        List<CuentasContablesViewModel> lst = (from d in db.cuentas_contables 
                                                               where d.estado == 1 & d.id == ParametroConversion & d.id_tipo_cuenta == 1
                                                               select new CuentasContablesViewModel
                                                               {
                                                                   id = d.id,
                                                                   codigo = d.codigo + "-" + d.descripcion,
                                                               }).ToList();
                        oR.result = 1;
                        oR.data = lst;


                    }
                }
            }
            catch (Exception ex)
            {
                oR.message = ex.ToString();
            }
            return oR;
        }

        [HttpPut]
        public Reply ListarPresupuestos([FromBody] SecurityViewModel model)
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
                var ParametroConversion = Convert.ToInt32(model.Parametro);
                using (PresupuestosPruebaEntities db = new PresupuestosPruebaEntities())
                {
                    if (ParametroConversion == 0)
                    {
                        List<PresupuestosViewModel> lst = (from d in db.presupuestos
                                                           where d.estado == 1 & d.id_tipo_presupuesto == 1
                                                           select new PresupuestosViewModel
                                                           {
                                                               id = d.id,
                                                               codigo = d.codigo+"-" + d.nombre,
                                                               estado = 1,
                                                           }).ToList();
                        oR.result = 1;
                        oR.data = lst;

                    }
                    else
                    {
                        List<PresupuestosViewModel> lst = (from d in db.presupuestos
                                                           where d.estado == 1 & d.id == ParametroConversion & d.id_tipo_presupuesto == 1
                                                           select new PresupuestosViewModel
                                                           {
                                                               id = d.id,
                                                               codigo = d.codigo + "-"+ d.nombre,
                                                               estado = 1,
                                                           }).ToList();
                        oR.result = 1;
                        oR.data = lst;


                    }
                }
            }
            catch (Exception ex)
            {
                oR.message = ex.ToString();
            }
            return oR;
        }
    }
}
