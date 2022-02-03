using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPresupuestos.Models
{
    public class PresupuestosViewModel : SecurityViewModel
	{
		public int id { set; get; }
		public string codigo { set; get; }
		public string nombre { set; get; }
		public int anio { set; get; }
		public int cantidad_periodos { set; get; }
		public int id_estado_actual_presupuesto { set; get; }
		public int id_pais { set; get; }
		public int id_moneda { set; get; }
		public int id_usuario_creacion { set; get; }
		public int id_tipo_presupuesto { set; get; }
		public string fecha_creacion { set; get; }
		public string comentarios { set; get; }
		public int estado { set; get; }
		public string nombre_pais { set; get; }
		public string nombre_moneda { set; get; }
		public string nombre_tipo_presupuesto { set; get; }
	}
}