using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPresupuestos.Models
{
    public class LineasViewModel : SecurityViewModel
    {
        public int id
        {
            set; get;
        }
        public string codigo
        {
            set; get;
        }
        public string descripcion
        {
            set; get;
        }
        public int id_pais
        {
            set; get;
        }

        public string nombre_pais
        {
            set; get;
        }

        public string nombre_unidad_negocio
        {
            set; get;
        }

        public int id_unidad_negocio
        {
            set; get;
        }
        public int estado
        {
            set; get;
        }
    }
}