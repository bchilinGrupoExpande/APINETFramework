using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPresupuestos.Models
{
    public class ReporteEjecucionCuentaContableViewModel : SecurityViewModel
    {
        public int id_cuenta_contable { set; get; }
        public string codigo { set; get; }
        public string nombre { set; get; }
        public double Real { set; get; }
        public double Ppto { set; get; }
        public double Var { set; get; }
        public double Ejecucion { set; get; }
    }
}