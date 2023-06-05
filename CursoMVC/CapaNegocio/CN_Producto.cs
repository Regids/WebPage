using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Producto
    {
        private CD_Productos objCapaDatos = new CD_Productos();

        public List<Producto> Listar()
        {
            return objCapaDatos.Listar();
        }

        public int Register(Producto obj, out string Mensaje)
        {

            string msj = string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrEmpty(obj.Descripcion) ? "La descripción no puede ser nula o vacía." : string.Empty;

            if (obj.oMarca.IdMarca == 0)
                msj = "Seleccione una marca";
            
            if(obj.Ocategoria.IdCategoria == 0)
                msj = "Seleccione una categoria";

            if (obj.Precio == 0)
                msj = "Indique el precio del producto";

            if (obj.Stock == 0)
                msj = "Ingrese el stock del producto";

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
        public bool Editar(Producto obj, out string Mensaje)
        {
            bool result = false;

            Mensaje = string.Empty;
            try
            {

                string msj = string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrEmpty(obj.Descripcion) ? msj = "La descripción no puede ser nula o vacía." : string.Empty;

                if (obj.oMarca.IdMarca == 0)
                    msj = "Seleccione una marca";

                if (obj.Ocategoria.IdCategoria == 0)
                    msj = "Seleccione una categoria";

                if (obj.Precio == 0)
                    msj = "Indique el precio del producto";

                if (obj.Stock == 0)
                    msj = "Ingrese el stock del producto";

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

        public bool GuardarDatosImagen(Producto oProducto, out string Mensaje)
        {
            return objCapaDatos.GuardarDatosImagen(oProducto, out Mensaje);
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDatos.Eliminar(id, out Mensaje);
        }
    }


}
