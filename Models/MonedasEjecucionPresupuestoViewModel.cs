using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPresupuestos.Models
{
    public class MonedasEjecucionPresupuestoViewModel:SecurityViewModel
    {
        public int id { set; get; }
        public int? Anio { set; get; }
        public int? Mes { set; get; }
        public string Pais { set; get; }
        public Decimal? TipoCambio { set; get; }
        public int id_moneda_presupuesto { set; get; }


    }
}