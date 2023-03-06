using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CapaDatos
{
    public class CD_Categoria
    {
        public List<Categoria> Listar()
        {
            List<Categoria> lista = new List<Categoria>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    try
                    {
                        string query = "SELECT IdCategoria, Descripcion, Activo FROM Categoria";

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
                                    new Categoria()
                                    {
                                        IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
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
                        lista = new List<Categoria>();
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<Categoria>();
            }

            return lista;
        }

        public int Registrar(Categoria obj, out string Mensaje)
        {
            int iDAutoGenerado = 0;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection SqlConnection = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_RegisterCategory", SqlConnection);

                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.AddWithValue("Resultado",SqlDbType.Int).Direction = ParameterDirection.Output;
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

        public bool Editar(Categoria obj, out string Mensaje)
        {
            bool result = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection SqlConnection = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_EditCategory", SqlConnection);

                    cmd.Parameters.AddWithValue("IdCategoria", obj.IdCategoria);
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

        public bool Eliminar(int idCategoria, out string Mensaje)
        {
            bool result = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection SqlConnection = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_DeleteCategory", SqlConnection);

                    cmd.Parameters.AddWithValue("IdCategoria", idCategoria);
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
