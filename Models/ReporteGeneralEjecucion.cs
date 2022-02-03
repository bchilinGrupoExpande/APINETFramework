using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPresupuestos.Models
{
    public class ReporteGeneralEjecucion : SecurityViewModel
    {
        public int id_estructura { set; get; }
        public string cuenta_contable { set; get; }
        public string area { set; get; }
        public string centro_costo { set; get; }
        public string linea { set; get; }
        public string rubro { set; get; }
        public EjecucionPeriodoViewModel[] ejecucion_presupuestaria { set; get; }
    }
}