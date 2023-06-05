using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reestablecer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Usuario oUsuario = new Usuario();        

            oUsuario = new CN_Usuarios().usuarioActual(correo,clave);

            if (oUsuario == null)
            {
                ViewBag.Error = "Datos incorrectos";
                return View();
            }
            else
            {
                if (oUsuario.Reestablecer)
                {
                    TempData["IdUsuario"] = oUsuario.IdUsuario;
                    return RedirectToAction("CambiarClave", "Home");
                }

                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult CambiarClave(string idusuario, string claveactual, string nuevaclave, string confirmarclave)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.IdUsuario == int.Parse(idusuario)).FirstOrDefault();

            if (oUsuario.Clave != CN_Recursos.ConvertirSHA256(claveactual))
            {
                TempData["idUsuario"] = idusuario;
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();

            }
            else if (nuevaclave != confirmarclave)
            {
                TempData["idUsuario"] = idusuario;
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }

            return View();
        }
    }
}