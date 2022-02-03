using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPresupuestos.Models
{
    public class ConceptosContablesCorporativosViewModel : SecurityViewModel
    {
        public int id { set; get; }
        public string codigo { set; get; }
        public string descripcion { set; get; }
        public int estado { set; get; } 
    }
}