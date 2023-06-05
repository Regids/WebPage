using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.Web.Security;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CambiarClave()
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
                    return RedirectToAction("CambiarClave", "Acceso");
                }

                FormsAuthentication.SetAuthCookie(oUsuario.Correo, false);


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
                ViewData["vclave"] = "";
                ViewBag.Error = "LLa contraseña actual no es correcta.";
                return View();

            }
            else if (nuevaclave != confirmarclave)
            {
                TempData["idUsuario"] = idusuario;
                ViewData["vclave"] = claveactual;
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }

            ViewData["vclave"] = "";
            nuevaclave = CN_Recursos.ConvertirSHA256(claveactual);
            string mensaje = string.Empty;

            bool respuesta = new CN_Usuarios().CambiarClave(int.Parse(idusuario), nuevaclave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["idUsuario"] = idusuario;
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }
        }

        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Usuario oUsuario = new Usuario();
            string mensaje = string.Empty;

            oUsuario = new CN_Usuarios().Listar().Where(x => x.Correo == correo).FirstOrDefault();

            if(oUsuario == null)
            {
                ViewBag.Error = "No se encontro un usuario para este correo";
                return View();
            }

            bool respuesta = new CN_Usuarios().RestablecerClave(oUsuario.IdUsuario, correo, out mensaje);

            if (respuesta)
            {
                ViewBag.Error = "";
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }
    }
}