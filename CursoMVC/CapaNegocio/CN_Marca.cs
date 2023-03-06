using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Marca
    {
        private CD_Marcas objCapaDatos = new CD_Marcas();

        public List<Marca> Listar()
        {
            return objCapaDatos.Listar();
        }

        public int Register(Marca obj, out string Mensaje)
        {

            string msj = string.IsNullOrEmpty(obj.Descripcion) ? msj = "La descripción no puede ser nula o vacía." : string.Empty;


            if (string.IsNullOrEmpty(msj))
            {
                return objCapaDatos.Registrar(obj, out Mensaje);
            }
            else
            {
                Mensaje = msj;
                return 0;
            }
        }

        public bool Editar(Marca obj, out string Mensaje)
        {
            bool result = false;

            Mensaje = string.Empty;
            try
            {

                string msj = string.IsNullOrEmpty(obj.Descripcion) ? msj = "La descripción no puede ser nula o vacía." : string.Empty;

                if (string.IsNullOrEmpty(msj))
                {
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
