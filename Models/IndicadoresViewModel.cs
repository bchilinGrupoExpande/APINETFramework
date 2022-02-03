using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPresupuestos.Models
{
    public class IndicadoresViewModel:SecurityViewModel
    {
        public int? id_pais { set; get; }
        public int? id_tipo_datos_1 { set; get; }
    }
}