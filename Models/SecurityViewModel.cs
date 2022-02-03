using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPresupuestos.Models
{
    public class SecurityViewModel
    {
        public string token { set; get; }
        public int? IDUsuario { set; get; }
        public string Parametro { set; get; }
        public int id_pais { set; get; }
        public int? id_tipo_cambio { set; get; }
    }
}