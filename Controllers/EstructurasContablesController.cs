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
    public class EstructurasContablesController : BaseController
    {
        [HttpPost]
        public Reply Agregar([FromBody]EstructurasContablesViewModel model)
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
                    estructuras_contables oEstructurasContables = new estructuras_contables();

                    oEstructurasContables.id_cuenta_contable = model.id_cuenta_contable;
                    oEstructurasContables.id_area = model.id_area;
                    oEstructurasContables.id_centro_costos = model.id_centro_costos;
                    oEstructurasContables.id_linea = model.id_linea;
                    oEstructurasContables.id_rubro_corporativo = model.id_rubro_corporativo;
                    oEstructurasContables.id_rubro_corporativo = model.id_rubro_corporativo;
                    oEstructurasContables.comentarios = model.comentarios;
                    oEstructurasContables.id_pais = model.id_pais;
                    oEstructurasContables.estado = 1;
                    db.estructuras_contables.Add(oEstructurasContables);
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
        public Reply Editar([FromBody] EstructurasContablesViewModel model)
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
                    estructuras_contables oEstructurasContables = db.estructuras_contables.Find(model.id);

                    oEstructurasContables.id_cuenta_contable = model.id_cuenta_contable;
                    oEstructurasContables.id_area = model.id_area;
                    oEstructurasContables.id_centro_costos = model.id_centro_costos;
                    oEstructurasContables.id_linea = model.id_linea;
                    oEstructurasContables.id_rubro_corporativo = model.id_rubro_corporativo;
                    oEstructurasContables.id_rubro_corporativo = model.id_rubro_corporativo;
                    oEstructurasContables.comentarios = model.comentarios;
                    oEstructurasContables.id_pais = model.id_pais;
                    oEstructurasContables.estado = 1;

                    db.Entry(oEstructurasContables).State = System.Data.Entity.EntityState.Modified;
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
