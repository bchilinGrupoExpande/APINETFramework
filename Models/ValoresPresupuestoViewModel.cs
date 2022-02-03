using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPresupuestos.Models
{
    public class ValoresPresupuestoViewModel: SecurityViewModel
	{
		public int id { set; get; }
		public int id_presupuesto { set; get; }
		public int id_periodo { set; get; }
		public int id_estructura_contable { set; get; }
		public int id_moneda { set; get; }
		public double valor_presupuestado { set; get; }
		public string fecha_creacion_valor_presupuesto { set; get; }
		public string comentarios { set; get; }
		public int estado { set; get; }
        public double? periodo_1 { set; get; }
        public double? periodo_2 { set; get; }
        public double? periodo_3 { set; get; }
        public double? periodo_4 { set; get; }
        public double? periodo_5 { set; get; }
        public double? periodo_6 { set; get; }
        public double? periodo_7 { set; get; }
        public double? periodo_8 { set; get; }
        public double? periodo_9 { set; get; }
        public double? periodo_10 { set; get; }
        public double? periodo_11 { set; get; }
        public double? periodo_12 { set; get; }

        public string nombre_cuenta_contable { set; get; }
        public string nombre_centro_costo { set; get; }
        public string nombre_linea { set; get; }
        public string nombre_area { set; get; }
        public string nombre_rubro_corporativo { set; get; }
        public string nombre_pais { set; get; }

    }
}