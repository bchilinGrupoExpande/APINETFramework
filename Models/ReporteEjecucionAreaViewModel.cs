using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPresupuestos.Models
{
    public class ReporteEjecucionAreaViewModel : SecurityViewModel
    {
        public int id_area { set; get; }
        public string area { set; get; }
        public double Real { set; get; }
        public double Ppto { set; get; }
        public double Var { set; get; }
        public double Ejecucion { set; get; }
    }
}