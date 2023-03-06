using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
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
        public ActionResult Productos()
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
    }
}