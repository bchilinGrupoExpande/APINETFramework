using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPresupuestos.Models
{
    public class UsuariosViewModel : SecurityViewModel
    {
        public int id { set; get; }
        public string correo_electronico { set; get; }
        public string contrasenia { set; get; }
        public string comentarios { set; get; }
        public int id_rol  { set; get; }
        public int id_pais  { set; get; }
        public int estado  { set; get; }
        public string nombre_pais { set; get; }
        public string nombre_rol { set; get; }
        public string nombre_completo { set; get; }
        public string nombre { set; get; }
        public string apellido { set; get; }
    }
}