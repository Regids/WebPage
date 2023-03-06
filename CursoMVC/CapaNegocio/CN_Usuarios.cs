using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuarios
    {
        private CD_Usuarios objCapaDatos = new CD_Usuarios();

        public List<Usuario> Listar()
        {
            return objCapaDatos.Listar();
        }

        public int Register(Usuario obj, out string Mensaje)
        {

           string msj = string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Nombres) ||
           string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos) || 
           string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo) ? msj = "Exists null data" : string.Empty;


            if(string.IsNullOrEmpty(msj))
            {
                string clave = CN_Recursos.GenerarClave();
                string asunto = "Creacion de cuenta";
                //string mensajecorreo = "<h3> Su cuenta fue creada exitosamente</h3></br><p>Su clave para acceder es: " + clave + "</p>";

                string mensajecorreo = @"
                        <!DOCTYPE html>
                        <html>
                          <head>
                            <title>Nueva contraseña creada</title>
                            <style>
                              body {
                                font-family: Arial, sans-serif;
                                background-color: #f5f5f5;
                                color: #333;
                              }
                              h1 {
                                font-size: 2em;
                                color: #333;
                              }
                              p {
                                font-size: 1.2em;
                                line-height: 1.5em;
                              }
                              .container {
                                max-width: 600px;
                                margin: 0 auto;
                                padding: 20px;
                                background-color: #fff;
                                box-shadow: 0px 2px 10px rgba(0,0,0,0.1);
                                border-radius: 5px;
                              }
                            </style>
                          </head>
                          <body>
                            <div class='container'>
                              <h1>Contraseña creada exitosamente</h1>
                              <p>Su nueva contraseña ha sido creada correctamente.</p>
                              <ul>                          
                                <li><strong>Fecha y hora:</strong> datetime</li>
                                <li><strong>Contraseña:</strong> pass</li>
                              </ul>
                              <p>Asegúrese de guardar su nueva contraseña en un lugar seguro y no compartirla con nadie.</p>
                              <p>Si tiene alguna pregunta o necesita ayuda, por favor contáctenos a través de nuestro sitio web.</p>
                            </div>
                          </body>
                        </html>
                        ";
                mensajecorreo = mensajecorreo.Replace("datetime", DateTime.Now.ToString());
                mensajecorreo = mensajecorreo.Replace("pass", clave);

                bool respuesta = CN_Recursos.EnviarCorreo(obj.Correo, asunto, mensajecorreo);

                if (respuesta)
                {
                    obj.Clave = CN_Recursos.ConvertirSHA256(clave); 
                    Mensaje = string.Empty;
                    return msj != string.Empty ? 0 : objCapaDatos.Registrar(obj, out Mensaje);
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo";
                    return 0;
                }           
            }
            else
            {
                Mensaje = msj;
                return 0;
            }          
        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
            bool result = false;

            Mensaje = string.Empty;
            try
            {

                string msj = string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Nombres) ||
                string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos) ||
                string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo) ? msj = "Exists null data" : string.Empty;


                if (!(msj != string.Empty))
                {
                    obj.Clave = "123"; //arreglar esto

                    return objCapaDatos.Editar(obj, out Mensaje);
                }
                else
                {
                    return false;
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
            return objCapaDatos.Eliminar(id, out Mensaje);
        }

    }
}
