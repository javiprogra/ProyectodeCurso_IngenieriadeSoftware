using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ParqueoEntidades;

namespace ParqueoDAL
{
   
    // Acceso a datos para la tabla TipoVehiculo
    
    public class TipoVehiculoDAL
    {
      
        // Retorna todos los tipos de vehículo activos.
       
        public List<TipoVehiculo> ObtenerTodos()
        {
            List<TipoVehiculo> lista = new List<TipoVehiculo>();
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                string query = "SELECT IdTipoVehiculo, Nombre FROM TipoVehiculo WHERE Estado = 1";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new TipoVehiculo
                    {
                        IdTipoVehiculo = (int)dr["IdTipoVehiculo"],
                        Nombre = dr["Nombre"].ToString()
                    });
                }
            }
            return lista;
        }
    }
}