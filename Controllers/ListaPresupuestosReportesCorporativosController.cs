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
    public class ListaPresupuestosReportesCorporativosController : BaseController
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
                    var dto_usuario = db.usuarios.Where(d => d.id == model.IDUsuario).First();
                    var user_pais = dto_usuario.id_pais;

                    int[] ids = new int[] { 1,4};

                    if (ParametroConversion == 0)
                    {
                        if (user_pais == 1)
                        {
                            List<PresupuestosViewModel> lst = (from d in db.presupuestos
                                                               where d.estado == 1 && ids.Contains(d.id_tipo_presupuesto)
                                                               select new PresupuestosViewModel
                                                               {
                                                                   id = d.id,
                                                                   codigo = d.codigo,
                                                                   nombre = d.nombre,
                                                                   anio = d.anio,
                                                                   cantidad_periodos = d.cantidad_periodos,
                                                                   id_estado_actual_presupuesto = d.id_estado_actual_presupuesto,
                                                                   id_pais = d.id_pais,
                                                                   id_moneda = d.id_moneda,
                                                                   id_usuario_creacion = d.id_usuario_creacion,
                                                                   id_tipo_presupuesto = d.id_tipo_presupuesto,
                                                                   fecha_creacion = d.fecha_creacion,
                                                                   comentarios = d.comentarios,
                                                                   nombre_moneda = d.monedas_presupuesto1.codigo_moneda,
                                                                   nombre_pais = d.pais.nombre,
                                                                   nombre_tipo_presupuesto = d.tipo_presupuestos.nombre,
                                                                   estado = 1,
                                                               }).ToList();
                            oR.result = 1;
                            oR.data = lst;
                        }
                        else
                        {
                            List<PresupuestosViewModel> lst = (from d in db.presupuestos
                                                               where d.estado == 1 && ids.Contains(d.id_tipo_presupuesto)
                                                               && d.id_pais==user_pais
                                                               select new PresupuestosViewModel
                                                               {
                                                                   id = d.id,
                                                                   codigo = d.codigo,
                                                                   nombre = d.nombre,
                                                                   anio = d.anio,
                                                                   cantidad_periodos = d.cantidad_periodos,
                                                                   id_estado_actual_presupuesto = d.id_estado_actual_presupuesto,
                                                                   id_pais = d.id_pais,
                                                                   id_moneda = d.id_moneda,
                                                                   id_usuario_creacion = d.id_usuario_creacion,
                                                                   id_tipo_presupuesto = d.id_tipo_presupuesto,
                                                                   fecha_creacion = d.fecha_creacion,
                                                                   comentarios = d.comentarios,
                                                                   nombre_moneda = d.monedas_presupuesto1.codigo_moneda,
                                                                   nombre_pais = d.pais.nombre,
                                                                   nombre_tipo_presupuesto = d.tipo_presupuestos.nombre,
                                                                   estado = 1,
                                                               }).ToList();
                            oR.result = 1;
                            oR.data = lst;
                        }
                      

                    }
                    else
                    {
                        List<PresupuestosViewModel> lst = (from d in db.presupuestos
                                                           where d.estado == 1 & d.id == ParametroConversion
                                                           select new PresupuestosViewModel
                                                           {
                                                               id = d.id,
                                                               codigo = d.codigo,
                                                               nombre = d.nombre,
                                                               anio = d.anio,
                                                               cantidad_periodos = d.cantidad_periodos,
                                                               id_estado_actual_presupuesto = d.id_estado_actual_presupuesto,
                                                               id_pais = d.id_pais,
                                                               id_moneda = d.id_moneda,
                                                               id_usuario_creacion = d.id_usuario_creacion,
                                                               id_tipo_presupuesto = d.id_tipo_presupuesto,
                                                               fecha_creacion = d.fecha_creacion,
                                                               comentarios = d.comentarios,
                                                               nombre_moneda = d.monedas_presupuesto1.codigo_moneda,
                                                               nombre_pais = d.pais.nombre,
                                                               nombre_tipo_presupuesto = d.tipo_presupuestos.nombre,
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
