using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPresupuestos.Models
{
    public class ReportesPresupuestoViewModel: SecurityViewModel
    {
        public int id_presupuesto_1 { set; get; }
        public int id_presupuesto_2 { set; get; }
        public string listado_periodos_1 { set; get; }
        public string listado_periodos_2 { set; get; }
        public int id_tipo_datos_1 { set; get; }
        public int id_tipo_datos_2 { set; get; }
        public int id_tipo_reporte { set; get; }
        public int anio_1 { set; get; }
        public int anio_2 { set; get; }
        public int id_tipo_presupuesto_1 { set; get; }
        public int id_tipo_presupuesto_2 { set; get; }

    }
}