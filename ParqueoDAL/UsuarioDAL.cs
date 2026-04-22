using ParqueoEntidades;
using ParqueoEntidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ParqueoDAL
{
   
    //Clase de acceso a datos para la tabla Usuario
    public class UsuarioDAL
    {

        //Busca un usuario activo por login y contraseña encriptada
        public Usuario ObtenerPorCredenciales(string login, string passwordEncriptado)
        {
            Usuario usuario = null;
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                string query = @"SELECT IdUsuario, NombreUsuario, Login, Estado
                                 FROM Usuario
                                 WHERE Login    = @Login
                                   AND Password = @Password
                                   AND Estado   = 1";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Login", login);
                cmd.Parameters.AddWithValue("@Password", passwordEncriptado);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                    usuario = MapearUsuario(dr);
            }
            return usuario;
        }

        //Encriptacion de contraseña usando SHA256
        public string HashSHA256(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));
                StringBuilder builder = new StringBuilder();

                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }


        //Retorna todos los usuarios registrados, activos e inactivos
        public List<Usuario> ObtenerTodos()
        {
            List<Usuario> lista = new List<Usuario>();
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                string query = @"SELECT IdUsuario, NombreUsuario, Login, Estado
                                 FROM Usuario
                                 ORDER BY NombreUsuario ASC";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                    lista.Add(MapearUsuario(dr));
            }
            return lista;
        }

   
        //Inserta un nuevo usuario en la base de datos
      
        public bool Insertar(Usuario usuario)
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                string query = @"INSERT INTO Usuario (NombreUsuario, Login, Password, Estado)
                                 VALUES (@NombreUsuario, @Login, @Password, 1)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                cmd.Parameters.AddWithValue("@Login", usuario.Login);
                cmd.Parameters.AddWithValue("@Password", usuario.Password);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

       
        //Actualiza nombre y opcionalmente la contraseña de un usuario.
        //Si Password viene vacío o null, no se modifica.
       
        public bool Actualizar(Usuario usuario)
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                //Si viene contraseña nueva se actualiza, si no se deja la actual
                string query = string.IsNullOrWhiteSpace(usuario.Password)
                    ? @"UPDATE Usuario SET
                            NombreUsuario = @NombreUsuario
                        WHERE IdUsuario = @IdUsuario"
                    : @"UPDATE Usuario SET
                            NombreUsuario = @NombreUsuario,
                            Password      = @Password
                        WHERE IdUsuario = @IdUsuario";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);

                if (!string.IsNullOrWhiteSpace(usuario.Password))
                    cmd.Parameters.AddWithValue("@Password", usuario.Password);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        
        //Cambia el estado activo/inactivo de un usuario (baja lógica)
       
        public bool CambiarEstado(int idUsuario, bool estado)
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                string query = @"UPDATE Usuario SET Estado = @Estado
                                 WHERE IdUsuario = @IdUsuario";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Estado", estado);
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }


        //Verifica si un login ya existe en la base de datos
     
        public bool LoginExiste(string login)
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                string query = "SELECT COUNT(*) FROM Usuario WHERE Login = @Login";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Login", login);
                con.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        
        //mapea una fila del SqlDataReader a un objeto Usuario
      
        private Usuario MapearUsuario(SqlDataReader dr)
        {
            return new Usuario
            {
                IdUsuario = (int)dr["IdUsuario"],
                NombreUsuario = dr["NombreUsuario"].ToString(),
                Login = dr["Login"].ToString(),
                Estado = (bool)dr["Estado"]
            };
        }
    }
}
