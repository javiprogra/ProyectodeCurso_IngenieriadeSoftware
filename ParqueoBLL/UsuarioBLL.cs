using ParqueoDAL;
using ParqueoEntidades;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace ParqueoBLL
{
   
    // lgica de negocio para autenticación y manejo de usuarios
   
    public class UsuarioBLL
    {
        private UsuarioDAL dal = new UsuarioDAL();

        
        // Encripta un texto plano usando SHA256
    
        public string Encriptar(string texto)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(texto));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

       
        // Autentica al usuario. Retorna el objeto Usuario si las credenciales son validas
       
        public Usuario Login(string login, string passwordPlano)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(passwordPlano))
                throw new Exception("El usuario y la contraseña son obligatorios.");

            return dal.ObtenerPorCredenciales(login, Encriptar(passwordPlano));
        }

        
        // Crea un nuevo usuario validando unicidad del login y encriptando la contraseña
       
        public bool CrearUsuario(string nombre, string login, string passwordPlano)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(login))
                throw new Exception("El login es obligatorio.");
            if (string.IsNullOrWhiteSpace(passwordPlano) || passwordPlano.Length < 6)
                throw new Exception("La contraseña debe tener al menos 6 caracteres.");
            if (dal.LoginExiste(login))
                throw new Exception("Ese login ya está en uso.");

            Usuario usuario = new Usuario
            {
                NombreUsuario = nombre,
                Login = login,
                Password = Encriptar(passwordPlano)
            };

            return dal.Insertar(usuario);
        }

       
        // Retorna todos los usuarios para el mantenimiento
    
        public List<Usuario> ObtenerTodos() => dal.ObtenerTodos();

    
        // Actualiza los datos de un usuario. Si se envía contraseña nueva la encripta,
        // si se deja vacía conserva la actual.
    
        public bool Actualizar(Usuario usuario, string passwordNuevo)
        {
            if (string.IsNullOrWhiteSpace(usuario.NombreUsuario))
                throw new Exception("El nombre es obligatorio.");

            if (!string.IsNullOrWhiteSpace(passwordNuevo))
            {
                if (passwordNuevo.Length < 6)
                    throw new Exception("La contraseña debe tener al menos 6 caracteres.");
                usuario.Password = Encriptar(passwordNuevo);
            }

            return dal.Actualizar(usuario);
        }

   
        /// Desactiva un usuario (baja lógica, no elimina el registro)
   
        public bool Desactivar(int idUsuario)
        {
            if (idUsuario == Sesion.UsuarioActivo.IdUsuario)
                throw new Exception("No puedes desactivar tu propio usuario.");

            return dal.CambiarEstado(idUsuario, false);
        }
    }
}