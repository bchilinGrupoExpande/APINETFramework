using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPresupuestos.Models
{
    public class PeriodosPresupuestoViewModel : SecurityViewModel
    {
        public int id { set; get; }
        public int id_presupuesto { set; get; }
        public int numero_periodo { set; get; }
        public string fecha_inicio  { set; get; }
        public string fecha_final { set; get; }
        public string comentarios { set; get; }
        public int estado  { set; get; }
    }
}