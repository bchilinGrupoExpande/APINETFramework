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
    public class ValoresPresupuestoController : BaseController
    {
        [HttpPost]
        public Reply Agregar([FromBody] ValoresPresupuestoViewModel model)
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
                    var id_moneda_presupusto = 1;

                    var lst = db.presupuestos.Where(d => d.id == model.id_presupuesto).ToList();

                    foreach (var dto in lst)
                    {
                        id_moneda_presupusto = dto.id_moneda;
                    }


                    valores_presupuesto oValoresPresupuesto = new valores_presupuesto();

                    oValoresPresupuesto.id_presupuesto = model.id_presupuesto;
                    oValoresPresupuesto.id_estructura_contable = model.id_estructura_contable;
                    oValoresPresupuesto.id_moneda = id_moneda_presupusto;
                    oValoresPresupuesto.valor_presupuestado = 0;
                    oValoresPresupuesto.periodo_1 = 0;
                    oValoresPresupuesto.periodo_2 = 0;
                    oValoresPresupuesto.periodo_3 = 0;
                    oValoresPresupuesto.periodo_4 = 0;
                    oValoresPresupuesto.periodo_5 = 0;
                    oValoresPresupuesto.periodo_6 = 0;
                    oValoresPresupuesto.periodo_7 = 0;
                    oValoresPresupuesto.periodo_8 = 0;
                    oValoresPresupuesto.periodo_9 = 0;
                    oValoresPresupuesto.periodo_10 = 0;
                    oValoresPresupuesto.periodo_11 = 0;
                    oValoresPresupuesto.periodo_12 = 0;
                    oValoresPresupuesto.valor_presupuestado = 0;
                    oValoresPresupuesto.fecha_creacion_valor_presupuesto = DateTime.Now.ToString();
                    oValoresPresupuesto.comentarios = model.comentarios;
                    oValoresPresupuesto.estado = 1;
                    db.valores_presupuesto.Add(oValoresPresupuesto);
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
        public Reply Editar([FromBody] ValoresPresupuestoViewModel model)
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
                    valores_presupuesto oValoresPresupuesto = db.valores_presupuesto.Find(model.id);

                    oValoresPresupuesto.periodo_1 = model.periodo_1;
                    oValoresPresupuesto.periodo_2 = model.periodo_2;
                    oValoresPresupuesto.periodo_3 = model.periodo_3;
                    oValoresPresupuesto.periodo_4 = model.periodo_4;
                    oValoresPresupuesto.periodo_5 = model.periodo_5;
                    oValoresPresupuesto.periodo_6 = model.periodo_6;
                    oValoresPresupuesto.periodo_7 = model.periodo_7;
                    oValoresPresupuesto.periodo_8 = model.periodo_8;
                    oValoresPresupuesto.periodo_9 = model.periodo_9;
                    oValoresPresupuesto.periodo_10 = model.periodo_10;
                    oValoresPresupuesto.periodo_11 = model.periodo_11;
                    oValoresPresupuesto.periodo_12 = model.periodo_12;
                    oValoresPresupuesto.valor_presupuestado = Convert.ToDouble(model.periodo_1 + model.periodo_2 + model.periodo_3 + model.periodo_4 + model.periodo_5 + model.periodo_6 + model.periodo_7 + model.periodo_8 + model.periodo_9 + model.periodo_10 + model.periodo_11 + model.periodo_12);


                    db.Entry(oValoresPresupuesto).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    oR.data = "[]";
                    oR.result = oValoresPresupuesto.id_presupuesto;

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
