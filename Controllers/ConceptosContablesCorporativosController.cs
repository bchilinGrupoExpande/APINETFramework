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
    public class ConceptosContablesCorporativosController : BaseController
    {
        [HttpPost]
        public Reply Agregar([FromBody] ConceptosContablesCorporativosViewModel model)
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
                    conceptos_contables_corporativos oConceptosContablesCorporativos = new conceptos_contables_corporativos();

                    oConceptosContablesCorporativos.codigo = model.codigo;
                    oConceptosContablesCorporativos.descripcion = model.descripcion;
                    oConceptosContablesCorporativos.estado = 1;
                    db.conceptos_contables_corporativos.Add(oConceptosContablesCorporativos);
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
        public Reply Editar([FromBody] ConceptosContablesCorporativosViewModel model)
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
                    conceptos_contables_corporativos oConceptosContablesCorporativos = db.conceptos_contables_corporativos.Find(model.id);

                    oConceptosContablesCorporativos.codigo = model.codigo;
                    oConceptosContablesCorporativos.descripcion = model.descripcion;
                    oConceptosContablesCorporativos.estado = 1;

                    db.Entry(oConceptosContablesCorporativos).State = System.Data.Entity.EntityState.Modified;
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
