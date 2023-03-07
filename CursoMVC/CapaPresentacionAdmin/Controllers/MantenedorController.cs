using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Categoria()
        {
            return View();
        }
        public ActionResult Marca()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }

        #region Categoria
        [HttpGet]
        public JsonResult ListarCategorias()
        {
            _ = new List<Categoria>();

            List<Categoria> oLista = new CN_Categoria().Listar();

            return Json(new
            {
                data = oLista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            object result;
            string mensaje = string.Empty;

            if (objeto.IdCategoria == 0)
            {
                result = new CN_Categoria().Register(objeto, out mensaje);
            }
            else
            {
                result = new CN_Categoria().Editar(objeto, out mensaje);
            }

            return Json(new
            {
                result = result,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = new CN_Categoria().Eliminar(id, out string mensaje);

            return Json(new
            {
                result = respuesta,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region MARCA

        [HttpGet]
        public JsonResult ListarMarcas()
        {
            _ = new List<Marca>();

            List<Marca> oLista = new CN_Marca().Listar();

            return Json(new
            {
                data = oLista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarMarca(Marca objeto)
        {
            object result;
            string mensaje = string.Empty;

            if (objeto.IdMarca == 0)
            {
                result = new CN_Marca().Register(objeto, out mensaje);
            }
            else
            {
                result = new CN_Marca().Editar(objeto, out mensaje);
            }

            return Json(new
            {
                result = result,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            bool respuesta = new CN_Marca().Eliminar(id, out string mensaje);

            return Json(new
            {
                result = respuesta,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PRODUCTO

        [HttpGet]
        public JsonResult ListarProductos()
        {
            _ = new List<Producto>();

            List<Producto> oLista = new CN_Producto().Listar();

            return Json(new
            {
                data = oLista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {
            string mensaje = string.Empty;
            bool operacionExitosa = true;
            bool GuardarImagenExitosa = true;
            decimal precio;

            Producto oProducto = new Producto();
            oProducto = JsonConvert.DeserializeObject<Producto>(objeto);

            if(decimal.TryParse(oProducto.PrecioTexto,NumberStyles.AllowDecimalPoint, new CultureInfo("es-CR"), out precio))
            {
                oProducto.Precio = precio;
            }
            else
            {
                return Json(new
                {
                    operacionExitosa = false,
                    mensaje = "Formato de precio incorrecto"
                }, JsonRequestBehavior.AllowGet);
            }

            if (oProducto.IdProducto == 0)
            {
                int idProductoGenerado = new CN_Producto().Register(oProducto, out mensaje);

                if (idProductoGenerado != 0)
                {
                    oProducto.IdProducto = idProductoGenerado;
                }
                else
                {
                    operacionExitosa = false;
                }
            }
            else
            {
                operacionExitosa = new CN_Producto().Editar(oProducto, out mensaje);
            }

            if (operacionExitosa)
            {
                if(archivoImagen!= null)
                {
                    string ruta = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string nombre_imagen = string.Concat(oProducto.IdProducto.ToString(), extension);

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(ruta,nombre_imagen));
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        GuardarImagenExitosa = false;
                    }

                    if (GuardarImagenExitosa)
                    {
                        oProducto.RutaImagen = ruta;
                        oProducto.NombreImagen = nombre_imagen;
                        bool respuesta = new CN_Producto().GuardarDatosImagen(oProducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Producto guardado, problemas con el guardado de la imagen";
                    }
                }
            }

            return Json(new
            {
                operacionExitosa = operacionExitosa,
                idGenerado = oProducto.IdProducto,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool conversion;
            Producto oproducto = new CN_Producto().Listar().Where(p => p.IdProducto == id).FirstOrDefault();

            string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(oproducto.RutaImagen,oproducto.NombreImagen), out conversion);

            return Json(new
            {
                conversion = conversion,
                textoBase64 = textoBase64,
                extension = Path.GetExtension(oproducto.NombreImagen)
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = new CN_Producto().Eliminar(id, out string mensaje);

            return Json(new
            {
                result = respuesta,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}