using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParqueoEntidades
{
  
    // Mantiene los datos del usuario autenticado durante toda la sesión.

    public static class Sesion
    {
        public static Usuario UsuarioActivo { get; set; }
    }
}
