using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using ClosedXML.Excel;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]
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

        [HttpPost]
        public FileResult ExportarVenta(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> olista = new List<Reporte>();
            olista = new CN_Reporte().VerReporte(fechainicio, fechafin, idtransaccion);

            DataTable dt = new DataTable();
            dt.Locale = new System.Globalization.CultureInfo("es-CR");

            dt.Columns.Add("Fecha Venta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("IdTransaccion", typeof(string));

            foreach(Reporte rp in olista)
            {
                dt.Rows.Add(new object[]{
                    rp.FechaVenta,
                    rp.Cliente,
                    rp.Producto,
                    rp.Precio,
                    rp.Cantidad,
                    rp.Total,
                    rp.IdTransaccion
                });
            }

            dt.TableName = "Datos";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVenta" + DateTime.Now.ToString() + ".xlsx");
                };
            }
        }
    }
}