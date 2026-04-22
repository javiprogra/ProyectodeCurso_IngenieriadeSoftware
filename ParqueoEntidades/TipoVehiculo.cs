using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParqueoEntidades
{
    
    //Entidad que representa un tipo de vehículo del catálogo.
  
    public class TipoVehiculo
    {
        public int IdTipoVehiculo { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
}
