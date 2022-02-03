using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPresupuestos.Models
{
    public class AreasViewModel : SecurityViewModel
    {
        public int id { set; get; }
        public string codigo { set; get; }
        public string descripcion { set; get; }
        public int id_pais  { set; get; }
        public int id_area_equivalente  { set; get; }
        public int estado { set; get; }
        public string nombre_pais { set; get; }
        public string nombre_area_equivalente { set; get; }
    }
}