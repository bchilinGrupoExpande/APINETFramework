using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPresupuestos.Models
{
    public class IndicadorCostoOperativoViewModel : SecurityViewModel
    {
        public int id_presupuesto { set; get; }
        public string pais { set; get; }
        public int anio { set; get; }
        public double valor { set; get; }
        public double creciminento { set; get; }
    }
}