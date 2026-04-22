using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ParqueoEntidades;

namespace ParqueoDAL
{
   
    // Acceso a datos para la tabla TipoCobro
    
    public class TipoCobroDAL
    {
       
        // Retorna todos los tipos de cobro activos
 
        public List<TipoCobro> ObtenerTodos()
        {
            List<TipoCobro> lista = new List<TipoCobro>();
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                string query = "SELECT IdTipoCobro, NombreTipo, CantidadCobro FROM TipoCobro WHERE Estado = 1";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new TipoCobro
                    {
                        IdTipoCobro = (int)dr["IdTipoCobro"],
                        NombreTipo = dr["NombreTipo"].ToString(),
                        CantidadCobro = (decimal)dr["CantidadCobro"]
                    });
                }
            }
            return lista;
        }
    }
}