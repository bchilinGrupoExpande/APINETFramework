using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPresupuestos.Models
{
    public class LoginViewModel
    {
        public string UsuarioCorreo { get; set; }
        public string contrasenia { get; set; }
        public string DireccionFisica { get; set; }
    }
}