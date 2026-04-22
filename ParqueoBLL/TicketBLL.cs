using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParqueoEntidades;
using ParqueoDAL;

namespace ParqueoBLL
{
    // lógica de negocio para el manejo de tickets de parqueo
    
    public class TicketBLL
    {
        private TicketDAL dal = new TicketDAL();

       
        // Genera un código único para el ticket
        // Formato: TKT-AAAAMMDD-XXXX
      
        public string GenerarCodigo()
        {
            string fecha = DateTime.Now.ToString("yyyyMMdd");
            string sufijo = Guid.NewGuid().ToString().Substring(0, 4).ToUpper();
            return $"TKT-{fecha}-{sufijo}";
        }

        // calcula el monto a cobrar según tiempo de estadia y tipo de cobro
        public decimal CalcularMonto(Ticket ticket)
        {
            DateTime entrada = ticket.FechaEntrada.Date + ticket.HoraEntrada;
            DateTime salida = DateTime.Now;

            TimeSpan duracion = salida - entrada;

            if (ticket.NombreTipoCobro.ToLower().Contains("hora"))
            {
                double horas = Math.Ceiling(duracion.TotalHours);
                return (decimal)horas * ticket.TarifaCobro;
            }
            else // por día
            {
                double dias = Math.Ceiling(duracion.TotalDays);
                return (decimal)dias * ticket.TarifaCobro;
            }
        }


        // Registra un nuevo ticket validando que los datos no estén vacoos

        public bool RegistrarTicket(Ticket ticket)
        {
            if (string.IsNullOrWhiteSpace(ticket.Placa))
                throw new Exception("La placa no puede estar vacía.");
            if (ticket.IdTipoVehiculo == 0)
                throw new Exception("Debe seleccionar un tipo de vehículo.");
            if (ticket.IdTipoCobro == 0)
                throw new Exception("Debe seleccionar un tipo de cobro.");

            ticket.NoTicket = GenerarCodigo();
            return dal.Insertar(ticket);
        }
    }
   }