using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPresupuestos.Models
{
    public class MonedasPresupuestoViewModel : SecurityViewModel
    {
        public int id { set; get; }
        public string codigo_moneda { set; get; }
        public string comentarios { set; get; }
        public string nombre_pais { set; get; }
        public double tasa_cambio_presupuesto { set; get; }
        public int estado  { set; get; }
        public int? id_presupuesto_asignado { set; get; }
        public int? anio_moneda { set; get; }
        public string estado_moneda { set; get; }
        public string presupuesto_asignado_moneda { set; get; }
    }
}