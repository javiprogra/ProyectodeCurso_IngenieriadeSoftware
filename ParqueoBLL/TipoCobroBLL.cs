using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParqueoDAL;
using ParqueoEntidades;

namespace ParqueoBLL
{

    // Logica de negocio para tipos de cobro
    public class TipoCobroBLL
    {
        private TipoCobroDAL dal = new TipoCobroDAL();

        public List<TipoCobro> ObtenerTodos() => dal.ObtenerTodos();
    }
}
