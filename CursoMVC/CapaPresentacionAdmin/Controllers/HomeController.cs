using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionAdmin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Usuarios()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarUsuario()
        {
            _ = new List<Usuario>();

            List<Usuario> oLista = new CN_Usuarios().Listar();

            return Json(new { 
                data =oLista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarUsuario(Usuario objUser)
        {
            object result;
            string mensaje;
            if (objUser.IdUsuario == 0)
            {
                result = new CN_Usuarios().Register(objUser, out mensaje);
            }
            else
            {
                result = new CN_Usuarios().Editar(objUser,out mensaje);
            }

            return Json(new 
            {
                result = result, mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            bool respuesta = new CN_Usuarios().Eliminar(id, out string mensaje);

            return Json(new 
            { 
               result = respuesta, mensaje = mensaje
            },JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult VistaDashboard()
        {
            DashBoard objeto = new CN_Reporte().VerDashboard();

            return Json(new
            {
                resultado = objeto,
            }, JsonRequestBehavior.AllowGet );
        }

        [HttpGet]
        public JsonResult ListaReporte(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> olista = new List<Reporte>();

            olista = new CN_Reporte().VerReporte(fechainicio, fechafin, idtransaccion);

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }
    }
}