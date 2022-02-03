using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPresupuestos.Models
{
    public class EstructurasContablesViewModel : SecurityViewModel
	{
		public int	id { set; get; }
		public string nombre_cuenta_contable { set; get; }
		public string nombre_area { set; get; }
		public string nombre_centro_costo { set; get; }
		public string nombre_linea { set; get; }
		public string nombre_rubro_corporativo { set; get; }
		public string nombre_pais { set; get; }
		public int? id_cuenta_contable { set; get; }
		public int? id_area { set; get; }
		public int? id_centro_costos { set; get; }
		public int? id_linea { set; get; }
		public int? id_rubro_corporativo { set; get; } 
		public string comentarios { set; get; }
		public int id_pais { set; get; }
		public int estado { set; get; }
		public int? id_tipo_estructura_contable { set; get; }
		public string nombre_tipo_estructura_contable { set; get; }
	}
}