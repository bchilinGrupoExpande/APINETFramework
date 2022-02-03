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
    public class ListaEstructurasContablesController : BaseController
    {
        [HttpPut]
        public Reply Eliminar([FromBody] EstructurasContablesViewModel model)
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
                    oEstructurasContables.estado = 0;
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
                    var dto_usuario = db.usuarios.Where(d => d.id == model.IDUsuario).First();
                    var user_pais = dto_usuario.id_pais;
                    if (ParametroConversion == 0)
                    {
                        if (user_pais == 1)
                        {
                            List<EstructurasContablesViewModel> lst = (from d in db.estructuras_contables
                                                                       where d.estado == 1
                                                                       select new EstructurasContablesViewModel
                                                                       {
                                                                           id = d.id,
                                                                           id_cuenta_contable = d.id_cuenta_contable,
                                                                           nombre_centro_costo = d.centros_de_costo.codigo,
                                                                           nombre_cuenta_contable = d.cuentas_contables.codigo,
                                                                           nombre_linea = d.linea.codigo,
                                                                           nombre_pais = d.pais.nombre,
                                                                           nombre_area = d.area.codigo,
                                                                           nombre_rubro_corporativo = d.rubros_contables_corporativos.codigo,
                                                                           id_area = d.id_area,
                                                                           id_centro_costos = d.id_centro_costos,
                                                                           id_linea = d.id_linea,
                                                                           id_rubro_corporativo = d.id_rubro_corporativo,
                                                                           comentarios = d.comentarios,
                                                                           id_pais = d.id_pais,
                                                                           estado = d.estado,
                                                                           id_tipo_estructura_contable = d.id_tipo_estructura_contable,
                                                                           nombre_tipo_estructura_contable = d.tipo_presupuestos.nombre,

                                                                       }).ToList();
                            oR.result = 1;
                            oR.data = lst;
                        }
                        else
                        {
                            List<EstructurasContablesViewModel> lst = (from d in db.estructuras_contables
                                                                       where d.estado == 1 && d.id_pais==user_pais
                                                                       select new EstructurasContablesViewModel
                                                                       {
                                                                           id = d.id,
                                                                           id_cuenta_contable = d.id_cuenta_contable,
                                                                           nombre_centro_costo = d.centros_de_costo.codigo,
                                                                           nombre_cuenta_contable = d.cuentas_contables.codigo,
                                                                           nombre_linea = d.linea.codigo,
                                                                           nombre_pais = d.pais.nombre,
                                                                           nombre_area = d.area.codigo,
                                                                           nombre_rubro_corporativo = d.rubros_contables_corporativos.codigo,
                                                                           id_area = d.id_area,
                                                                           id_centro_costos = d.id_centro_costos,
                                                                           id_linea = d.id_linea,
                                                                           id_rubro_corporativo = d.id_rubro_corporativo,
                                                                           comentarios = d.comentarios,
                                                                           id_pais = d.id_pais,
                                                                           estado = d.estado,
                                                                           id_tipo_estructura_contable = d.id_tipo_estructura_contable,
                                                                           nombre_tipo_estructura_contable = d.tipo_presupuestos.nombre,

                                                                       }).ToList();
                            oR.result = 1;
                            oR.data = lst;
                        }
                      

                    }
                    else
                    {
                        List<EstructurasContablesViewModel> lst = (from d in db.estructuras_contables
                                                                               where d.estado == 1 & d.id == ParametroConversion
                                                                             select new EstructurasContablesViewModel
                                                                             {
                                                                                 id = d.id,
                                                                                 id_cuenta_contable = d.id_cuenta_contable,
                                                                                 id_area = d.id_area,
                                                                                 id_centro_costos = d.id_centro_costos,
                                                                                 id_linea = d.id_linea,
                                                                                 nombre_centro_costo = d.centros_de_costo.codigo,
                                                                                 nombre_cuenta_contable = d.cuentas_contables.codigo,
                                                                                 nombre_linea = d.linea.codigo,
                                                                                 nombre_pais = d.pais.nombre,
                                                                                 nombre_area=d.area.codigo,
                                                                                 nombre_rubro_corporativo = d.rubros_contables_corporativos.codigo,
                                                                                 id_rubro_corporativo = d.id_rubro_corporativo,
                                                                                 comentarios = d.comentarios,
                                                                                 id_pais = d.id_pais,
                                                                                 estado = d.estado,
                                                                                 id_tipo_estructura_contable = d.id_tipo_estructura_contable,
                                                                                 nombre_tipo_estructura_contable = d.tipo_presupuestos.nombre,
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
