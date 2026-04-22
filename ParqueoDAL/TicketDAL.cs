using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ParqueoEntidades;

namespace ParqueoDAL
{

    // Acceso a datos para la tabla Ticket
    public class TicketDAL
    {
       
        // Inserta un nuevo ticket en la base de datos
        public bool Insertar(Ticket ticket)
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                string query = @"INSERT INTO Ticket 
                                 (NoTicket, Placa, FechaEntrada, HoraEntrada,
                                  IdTipoVehiculo, IdTipoCobro, Pagado, Estado)
                                 VALUES
                                 (@NoTicket, @Placa, @FechaEntrada, @HoraEntrada,
                                  @IdTipoVehiculo, @IdTipoCobro, 0, 1)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@NoTicket", ticket.NoTicket);
                cmd.Parameters.AddWithValue("@Placa", ticket.Placa.ToUpper());
                cmd.Parameters.AddWithValue("@FechaEntrada", ticket.FechaEntrada);
                cmd.Parameters.AddWithValue("@HoraEntrada", ticket.HoraEntrada);
                cmd.Parameters.AddWithValue("@IdTipoVehiculo", ticket.IdTipoVehiculo);
                cmd.Parameters.AddWithValue("@IdTipoCobro", ticket.IdTipoCobro);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

    
        // Busca un ticket por su código único, trayendo datos de las tablas relacionadas
   
        public Ticket ObtenerPorCodigo(string noTicket)
        {
            Ticket ticket = null;
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                string query = @"SELECT t.*, 
                                        tv.Nombre        AS NombreTipoVehiculo,
                                        tc.NombreTipo    AS NombreTipoCobro,
                                        tc.CantidadCobro AS TarifaCobro
                                 FROM Ticket t
                                 INNER JOIN TipoVehiculo tv ON t.IdTipoVehiculo = tv.IdTipoVehiculo
                                 INNER JOIN TipoCobro    tc ON t.IdTipoCobro    = tc.IdTipoCobro
                                 WHERE t.NoTicket = @NoTicket AND t.Estado = 1";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@NoTicket", noTicket);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                    ticket = MapearTicket(dr);
            }
            return ticket;
        }

        // Retorna los tickets más recientes sin pagar, para la pantalla principal
 
        public List<Ticket> ObtenerRecientes(int cantidad = 20)
        {
            List<Ticket> lista = new List<Ticket>();
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                string query = @"SELECT TOP (@Cantidad) t.*,
                                        tv.Nombre        AS NombreTipoVehiculo,
                                        tc.NombreTipo    AS NombreTipoCobro,
                                        tc.CantidadCobro AS TarifaCobro
                                 FROM Ticket t
                                 INNER JOIN TipoVehiculo tv ON t.IdTipoVehiculo = tv.IdTipoVehiculo
                                 INNER JOIN TipoCobro    tc ON t.IdTipoCobro    = tc.IdTipoCobro
                                 WHERE t.Estado = 1
                                 ORDER BY t.FechaEntrada DESC, t.HoraEntrada DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                    lista.Add(MapearTicket(dr));
            }
            return lista;
        }

     
        // Retorna tickets cobrados dentro de un rango de fechas para el reporte
        
        public List<Ticket> ObtenerPorRangoFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            List<Ticket> lista = new List<Ticket>();
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                string query = @"SELECT t.*,
                                        tv.Nombre        AS NombreTipoVehiculo,
                                        tc.NombreTipo    AS NombreTipoCobro,
                                        tc.CantidadCobro AS TarifaCobro
                                 FROM Ticket t
                                 INNER JOIN TipoVehiculo tv ON t.IdTipoVehiculo = tv.IdTipoVehiculo
                                 INNER JOIN TipoCobro    tc ON t.IdTipoCobro    = tc.IdTipoCobro
                                 WHERE t.Pagado = 1
                                   AND t.FechaSalida BETWEEN @FechaInicio AND @FechaFin
                                 ORDER BY t.FechaSalida DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio.Date);
                cmd.Parameters.AddWithValue("@FechaFin", fechaFin.Date);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                    lista.Add(MapearTicket(dr));
            }
            return lista;
        }

      
        /// rgistra el cobro del ticket: fecha/hora salida, monto, usuario que cobro
       
        public bool RegistrarCobro(Ticket ticket)
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                string query = @"UPDATE Ticket SET
                                    FechaSalida  = @FechaSalida,
                                    HoraSalida   = @HoraSalida,
                                    MontoCobrado = @MontoCobrado,
                                    Pagado       = 1,
                                    FechaMod     = @FechaMod,
                                    IdUsuario    = @IdUsuario
                                 WHERE IdTicket = @IdTicket";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@FechaSalida", ticket.FechaSalida);
                cmd.Parameters.AddWithValue("@HoraSalida", ticket.HoraSalida);
                cmd.Parameters.AddWithValue("@MontoCobrado", ticket.MontoCobrado);
                cmd.Parameters.AddWithValue("@FechaMod", DateTime.Now);
                cmd.Parameters.AddWithValue("@IdUsuario", ticket.IdUsuario);
                cmd.Parameters.AddWithValue("@IdTicket", ticket.IdTicket);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

       
        // Mapea una fila del SqlDataReader a un objeto Ticket.
        // Método privado reutilizado por todos los SELECT.
       
        private Ticket MapearTicket(SqlDataReader dr)
        {
            return new Ticket
            {
                IdTicket = (int)dr["IdTicket"],
                NoTicket = dr["NoTicket"].ToString(),
                Placa = dr["Placa"].ToString(),
                FechaEntrada = (DateTime)dr["FechaEntrada"],
                HoraEntrada = (TimeSpan)dr["HoraEntrada"],
                FechaSalida = dr["FechaSalida"] == DBNull.Value ? (DateTime?)null : (DateTime)dr["FechaSalida"],
                HoraSalida = dr["HoraSalida"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan)dr["HoraSalida"],
                MontoCobrado = dr["MontoCobrado"] == DBNull.Value ? (decimal?)null : (decimal)dr["MontoCobrado"],
                Pagado = (bool)dr["Pagado"],
                Estado = (bool)dr["Estado"],
                IdTipoVehiculo = (int)dr["IdTipoVehiculo"],
                IdTipoCobro = (int)dr["IdTipoCobro"],
                IdUsuario = dr["IdUsuario"] == DBNull.Value ? (int?)null : (int)dr["IdUsuario"],
                NombreTipoVehiculo = dr["NombreTipoVehiculo"].ToString(),
                NombreTipoCobro = dr["NombreTipoCobro"].ToString(),
                TarifaCobro = (decimal)dr["TarifaCobro"]
            };
        }
    }
}
