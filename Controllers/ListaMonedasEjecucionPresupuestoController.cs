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
    public class ListaMonedasEjecucionPresupuestoController : BaseController
    {
        [HttpPost]
        public Reply Listar([FromBody] SecurityViewModel model)
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
                            List<MonedasEjecucionPresupuestoViewModel> lst = (from d in db.monedas_ejecutado
                                                              select new MonedasEjecucionPresupuestoViewModel
                                                              {
                                                                  id = d.id,
                                                                  Pais = d.Pais,
                                                                  Mes = d.Mes,
                                                                  Anio = d.Anio,
                                                                  TipoCambio=d.TipoCambio,
                                                           
                                                              }).ToList();
                        oR.result = 1;
                        oR.data = lst;

                    }
                    else
                    {
                        List<MonedasEjecucionPresupuestoViewModel> lst = (from d in db.monedas_ejecutado
                                                              where  d.id == ParametroConversion
                                                              select new MonedasEjecucionPresupuestoViewModel
                                                              {
                                                                  id = d.id,
                                                                  Pais = d.Pais,
                                                                  Mes = d.Mes,
                                                                  Anio = d.Anio,
                                                                  TipoCambio = d.TipoCambio,
                                          
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
        public Reply ListarMonedasPresupuesto([FromBody] SecurityViewModel model)
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
                        List<MonedasEjecucionPresupuestoViewModel> lst = (from d in db.monedas_ejecutado
                                                                          select new MonedasEjecucionPresupuestoViewModel
                                                                          {
                                                                              id = d.id,
                                                                              Pais = d.Pais,
                                                                              Mes = d.Mes,
                                                                              Anio = d.Anio,
                                                                              TipoCambio = d.TipoCambio,
                                                                   
                                                                          }).ToList();
                        oR.result = 1;
                        oR.data = lst;

                    }
                    else
                    {
                        var pais_ = db.paises.Where(d => d.id == model.id_pais && d.estado == 1).First();

                        List<MonedasEjecucionPresupuestoViewModel> lst = (from d in db.monedas_ejecutado
                                                                          where d.Anio == ParametroConversion &&
                                                                          d.Pais==pais_.nombre

                                                                          select new MonedasEjecucionPresupuestoViewModel
                                                                          {
                                                                              id = d.id,
                                                                              Pais = d.Pais,
                                                                              Mes = d.Mes,
                                                                              Anio = d.Anio,
                                                                              TipoCambio = d.TipoCambio,
                                                                              id_moneda_presupuesto = d.id
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
