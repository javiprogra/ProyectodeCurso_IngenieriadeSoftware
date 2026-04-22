using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

   
namespace ParqueoDAL
{
        
        // Clase encargada de gestionar la conexión a la base de datos
       
        public class Conexion
        {
            private static string cadena =
                "Server=localhost\\SQLJAVIDB2026;Database=ParqueoDB;Integrated Security=True;";

        
            // Retorna una nueva instancia de SqlConnection abierta
          
            public static SqlConnection ObtenerConexion()
            {
                return new SqlConnection(cadena);
            }
        }
}


