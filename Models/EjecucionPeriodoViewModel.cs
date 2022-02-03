using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPresupuestos.Models
{
    public class EjecucionPeriodoViewModel
    {
        public int id_periodo { set; get; }
        public double Real { set; get; }
        public double Ppto { set; get; }
        public double Var { set; get; }
        public double Ejecucion { set; get; }
    }
}