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
    public class RubrosContablesCorporativosController : BaseController
    {
        [HttpPost]
        public Reply Agregar([FromBody] RubrosContablesCorporativosViewModel model)
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
                    rubros_contables_corporativos oRubrosContablesCorporativos = new rubros_contables_corporativos();

                    oRubrosContablesCorporativos.codigo = model.codigo;
                    oRubrosContablesCorporativos.id_concepto_corporativo = model.id_concepto_corporativo;
                    oRubrosContablesCorporativos.descripcion = model.descripcion;
                    oRubrosContablesCorporativos.estado = 1;
                    db.rubros_contables_corporativos.Add(oRubrosContablesCorporativos);
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
        public Reply Editar([FromBody] RubrosContablesCorporativosViewModel model)
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
                    rubros_contables_corporativos oRubrosContablesCorporativos = db.rubros_contables_corporativos.Find(model.id);

                    oRubrosContablesCorporativos.codigo = model.codigo;
                    oRubrosContablesCorporativos.id_concepto_corporativo = model.id_concepto_corporativo;
                    oRubrosContablesCorporativos.descripcion = model.descripcion;
                    oRubrosContablesCorporativos.estado = 1;
                    db.rubros_contables_corporativos.Add(oRubrosContablesCorporativos);

                    db.Entry(oRubrosContablesCorporativos).State = System.Data.Entity.EntityState.Modified;
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
