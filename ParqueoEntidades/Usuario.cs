using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParqueoEntidades
{

    // Entidad que representa un usuario del sistema.
    
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool Estado { get; set; }
    }
}