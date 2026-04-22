using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParqueoDAL;
using ParqueoEntidades;

namespace ParqueoBLL
{
   
    // Lógica de negocio para tipos de vehiculo
   
    public class TipoVehiculoBLL
    {
        private TipoVehiculoDAL dal = new TipoVehiculoDAL();

        public List<TipoVehiculo> ObtenerTodos() => dal.ObtenerTodos();
    }


}