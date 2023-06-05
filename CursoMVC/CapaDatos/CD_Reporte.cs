using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Reporte
    {
        public DashBoard VerDashboard()
        {
            DashBoard objeto = new DashBoard();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("SP_FULLDATA", oconexion);
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                        };

                        oconexion.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                objeto = new DashBoard()
                                {
                                    TotalCliente = Convert.ToInt32(dr["TOTALCLIENTE"]),
                                    TotalVenta = Convert.ToInt32(dr["TOTALVENTA"]),
                                    TotalProducto = Convert.ToInt32(dr["TOTALPRODUCTO"])
                                };
                            }
                        }
                        oconexion.Close();
                    }
                    catch (Exception)
                    {
                        oconexion.Close();
                        objeto = new DashBoard();
                    }
                }
            }
            catch (Exception)
            {
                objeto = new DashBoard();
            }

            return objeto;
        }
        public List<Reporte> VerReporte(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> reporte = new List<Reporte>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_DetalleVentaCliente", oconexion);
                    cmd.Parameters.AddWithValue("FechaInicio", fechainicio);
                    cmd.Parameters.AddWithValue("FechaFin", fechafin);
                    cmd.Parameters.AddWithValue("idtransaccion", idtransaccion);

                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            reporte.Add(
                                new Reporte()
                                {
                                    FechaVenta = dr["FechaVenta"].ToString(),
                                    Cliente = dr["Cliente"].ToString(),
                                    Producto = dr["Producto"].ToString(),
                                    Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-CR")),
                                    Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                    Total = Convert.ToDecimal(dr["Total"]),
                                    IdTransaccion = dr["IdTransaccion"].ToString()

                                }
                         );
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return reporte;
        }
    }
}
