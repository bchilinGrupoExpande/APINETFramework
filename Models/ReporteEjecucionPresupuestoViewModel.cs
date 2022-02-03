using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPresupuestos.Models
{
    public class ReporteEjecucionPresupuestoViewModel : SecurityViewModel
    {
        public string ListadoPresupuestos { set; get; }
        public string ListadoAreas { set; get; }
        public string ListadoPeriodos { set; get; }
        public string ListadoCuentasContables { set; get; }
        public string ListadosCentrosDeCosto { set; get; }
        public string ListadoLineas { set; get; }
        public int id_moneda { set; get; }
    }
}