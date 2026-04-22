using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParqueoEntidades
{

    // Entidad que representa una tarifa de cobro configurable en base de datos.
   
    public class TipoCobro
    {
        public int IdTipoCobro { get; set; }
        public string NombreTipo { get; set; }
        public decimal CantidadCobro { get; set; }
        public bool Estado { get; set; }
    }
}
