using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParqueoEntidades
{
 
    // Entidad que representa un ticket de parqueo.

    public class Ticket
    {
        public int IdTicket { get; set; }
        public string NoTicket { get; set; }
        public string Placa { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime? FechaSalida { get; set; }
        public TimeSpan HoraEntrada { get; set; }
        public TimeSpan? HoraSalida { get; set; }
        public decimal? MontoCobrado { get; set; }
        public bool Pagado { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaMod { get; set; }
        public int IdTipoVehiculo { get; set; }
        public int IdTipoCobro { get; set; }
        public int? IdUsuario { get; set; }

        // Propiedades de navegación para mostrar en UI sin queries adicionales
        public string NombreTipoVehiculo { get; set; }
        public string NombreTipoCobro { get; set; }
        public decimal TarifaCobro { get; set; }
    }
}
