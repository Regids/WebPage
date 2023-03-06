using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Marcas
    {
        public List<Marca> Listar()
        {
            List<Marca> lista = new List<Marca>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    try
                    {
                        string query = "SELECT IdMarca, Descripcion, Activo From Marca";

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
                                    new Marca()
                                    {
                                        IdMarca = Convert.ToInt32(dr["IdMarca"]),
                                        Descripcion = dr["Descripcion"].ToString(),
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
                        lista = new List<Marca>();
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<Marca>();
            }

            return lista;
        }

        public int Registrar(Marca obj, out string Mensaje)
        {
            int iDAutoGenerado = 0;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection SqlConnection = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_RegisterBrands", SqlConnection);

                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
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

        public bool Editar(Marca obj, out string Mensaje)
        {
            bool result = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection SqlConnection = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_EditBrand", SqlConnection);

                    cmd.Parameters.AddWithValue("IdMarca", obj.IdMarca);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
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

        public bool Eliminar(int idMarca, out string Mensaje)
        {
            bool result = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection SqlConnection = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_DeleteBrand", SqlConnection);

                    cmd.Parameters.AddWithValue("IdMarca", idMarca);
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
