using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.IO;


namespace CapaNegocio
{
    public class CN_Recursos
    {

        public static string GenerarClave()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 6);
        }

        public static string ConvertirSHA256(string texto)
        {
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool resultado;

            try
            {
                MailMessage mail= new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("regids2012@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smpt = new SmtpClient() //servidor cliente para el envio de correos.
                {
                    Credentials = new NetworkCredential("regids2012@gmail.com", "gadsyhemsiwpwseg"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };

                smpt.Send(mail);
                resultado = true;

            }
            catch (Exception)
            {
                resultado = false;
            }

            return resultado;
        }

        public static string ConvertirBase64(string ruta, out bool conversion)
        {
            string textoBase64 = string.Empty;
            conversion = true;

            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                textoBase64 = Convert.ToBase64String(bytes);
            }
            catch (Exception)
            {
                conversion = false;
            }

            return textoBase64;
        }

    }
}
