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
    public class CD_Usuarios
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    try
                    {
                        string query = "SELECT IdUsuario, Nombres, Apellidos, Correo, Clave, Reestablecer, Activo FROM USUARIO";

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
                                    new Usuario()
                                    {
                                        IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                        Nombres = dr["Nombres"].ToString(),
                                        Apellidos = dr["Apellidos"].ToString(),
                                        Correo = dr["Correo"].ToString(),
                                        Clave = dr["Clave"].ToString(),
                                        Reestablecer = Convert.ToBoolean(dr["Reestablecer"]),
                                        Activo = Convert.ToBoolean(dr["Activo"])
                                    }
                               );
                            }
                        }
                        oconexion.Close();
                    }
                    catch(Exception ex)
                    {
                        oconexion.Close();
                        lista = new List<Usuario>();
                    }                    
                }
            }
            catch(Exception)
            {
                lista = new List<Usuario>();
            }

            return lista;
        }

        public int Registrar(Usuario obj, out string Mensaje)
        {
            int iDAutoGenerado = 0;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection SqlConnection = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_RegisterUser", SqlConnection);

                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);

                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlConnection.Open();
                    cmd.ExecuteNonQuery();

                    //iDAutoGenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    //Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
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

        public bool Editar(Usuario obj, out string Mensaje)
        {
            bool result = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection SqlConnection = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_EditUser", SqlConnection);

                    cmd.Parameters.AddWithValue("IdUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);

                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlConnection.Open();

                    cmd.ExecuteNonQuery();

                    result = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                    SqlConnection.Close();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                Mensaje = ex.Message;
            }

            return result;
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            bool result = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection connect = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("DELETE TOP(1) FROM Usuario WHERE IdUsuario = @id", connect);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    connect.Open();
                    result = cmd.ExecuteNonQuery() > 0 ? true : false;

                    connect.Close();
                }

            }catch(Exception ex)
            {
                result = false;
                Mensaje = ex.Message;
            }
            return result;
        }

        public bool CambiarClave(int idUsuario, string nuevaclave, out string Mensaje)
        {
            bool result = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection connect = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE USUARIO SET CLAVE = @nuevaclave, reestablecer = 0 WHERE IdUsuario = @id", connect);
                    cmd.Parameters.AddWithValue("@nuevaclave", nuevaclave);
                    cmd.Parameters.AddWithValue("@id", idUsuario);
                    cmd.CommandType = CommandType.Text;
                    connect.Open();
                    result = cmd.ExecuteNonQuery() > 0 ? true : false;

                    connect.Close();
                }

            }catch(Exception ex)
            {
                result = false;
                Mensaje = ex.Message;
            }
            return result;
        }

        public bool ReestablecerClave(int idUsuario, string clave, out string Mensaje)
        {
            bool result = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection connect = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE USUARIO SET CLAVE = @clave, reestablecer = 1 WHERE IdUsuario = @id", connect);
                    cmd.Parameters.AddWithValue("@nuevaclave", clave);
                    cmd.Parameters.AddWithValue("@id", idUsuario);
                    cmd.CommandType = CommandType.Text;
                    connect.Open();
                    result = cmd.ExecuteNonQuery() > 0 ? true : false;

                    connect.Close();
                }

            }catch(Exception ex)
            {
                result = false;
                Mensaje = ex.Message;
            }
            return result;
        }


    }
}
