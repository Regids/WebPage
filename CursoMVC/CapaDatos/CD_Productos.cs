using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Productos
    {
        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    try
                    {
                        string query = "SELECT p.IdProducto, p.Nombre, p.Descripcion, m.IdMarca, m.Descripcion[DesMarca], c.IdCategoria, c.Descripcion[DesCategoria]," +
                            "p.Precio, p.Stock, p.RutaImagen, p.NombreImagen, p.Activo FROM Producto P " +
                            "INNER JOIN Marca m on m.IdMarca = p.IdMarca " +
                            "INNER JOIN Categoria c on c.IdCategoria = p.IdCategoria";

                        SqlCommand cmd = new SqlCommand(query, oconexion)
                        {
                            CommandType = CommandType.Text
                        };

                        oconexion.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(
                                    new Producto()
                                    {
                                        IdProducto = Convert.ToInt32(dr["IdProducto"]),
                                        Nombre = dr["Nombre"].ToString(),
                                        Descripcion = dr["Descripcion"].ToString(),                                       
                                        oMarca = new Marca()
                                        {
                                            IdMarca = Convert.ToInt32(dr["IdMarca"]),
                                            Descripcion = dr["DesMarca"].ToString(),
                                        },
                                        Ocategoria = new Categoria()
                                        {
                                            IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                                            Descripcion = dr["DesCategoria"].ToString(),
                                        },
                                        Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-CR")),
                                        Stock = Convert.ToInt32(dr["Stock"]),
                                        RutaImagen = dr["RutaImagen"].ToString(),
                                        NombreImagen = dr["NombreImagen"].ToString(),
                                        Activo = Convert.ToBoolean(dr["Activo"])
                                    }
                               );                                
                            }
                        }
                        oconexion.Close();
                    }
                    catch (Exception ex)
                    {
                        oconexion.Close();
                        lista = new List<Producto>();
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<Producto>();
            }

            return lista;
        }

        public int Registrar(Producto obj, out string Mensaje)
        {
            int iDAutoGenerado = 0;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection SqlConnection = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_ProductRegister", SqlConnection);

                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", obj.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.Ocategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.AddWithValue("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("Mensaje", SqlDbType.VarChar).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlConnection.Open();
                    cmd.ExecuteNonQuery();

                    iDAutoGenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                    SqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                iDAutoGenerado = 0;
                Mensaje = ex.Message;
            }

            return iDAutoGenerado;
        }

        public bool Editar(Producto obj, out string Mensaje)
        {
            bool result = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection SqlConnection = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_ProductEdit", SqlConnection);

                    cmd.Parameters.AddWithValue("IdProducto", obj.IdProducto);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", obj.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.Ocategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.AddWithValue("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("Mensaje", SqlDbType.VarChar).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlConnection.Open();

                    cmd.ExecuteNonQuery();

                    result = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                    SqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                result = false;
                Mensaje = ex.Message;
            }

            return result;
        }

        public bool GuardarDatosImagen(Producto oProducto, out string Mensaje)
        {
            Mensaje = string.Empty;

            try
            {
                string query = "Update Producto SET RutaImagen = @rutaImagen, NombreImagen = @NombreImagen where IdProducto = @IdProducto";

                using (SqlConnection SqlConnection = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand(query, SqlConnection);

                    cmd.Parameters.AddWithValue("RutaImagen", oProducto.RutaImagen);
                    cmd.Parameters.AddWithValue("NombreImagen", oProducto.NombreImagen);
                    cmd.Parameters.AddWithValue("IdProducto", oProducto.IdProducto);
                    cmd.Parameters.AddWithValue("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("Mensaje", SqlDbType.VarChar).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.Text;

                    SqlConnection.Open();

                    

                    if (cmd.ExecuteNonQuery()> 0)
                    {
                        SqlConnection.Close();
                        return true;
                    }
                    else
                    {
                        SqlConnection.Close();
                        return false;
                    }                  
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                return false;
            }

        }

        public bool Eliminar(int idProducto, out string Mensaje)
        {
            bool result = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection SqlConnection = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_ProductDelete", SqlConnection);

                    cmd.Parameters.AddWithValue("IdProducto", idProducto);
                    cmd.Parameters.AddWithValue("Mensaje", SqlDbType.VarChar).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlConnection.Open();

                    cmd.ExecuteNonQuery();

                    result = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                    SqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                result = false;
                Mensaje = ex.Message;
            }

            return result;
        }
    }
}
