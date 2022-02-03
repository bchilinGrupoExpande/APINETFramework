using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPresupuestos.Models
{
    public class Reply
    {
        public int result { get; set; }
        public object data { get; set; }
        public string message { get; set; }

        public string NombreUsuario { get; set; }
        public string RolUsuario { get; set; }
        public string CorreoElectronico { get; set; }

    }
}