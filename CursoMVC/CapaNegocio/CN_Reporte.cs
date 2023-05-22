using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Reporte
    {
        private CD_Reporte objCapaDatos = new CD_Reporte();

        public DashBoard VerDashboard()
        {
            return objCapaDatos.VerDashboard();
        }

        public List<Reporte> VerReporte(string fechainicio, string fechafin, string idtransaccion)
        {
            return objCapaDatos.VerReporte(fechainicio, fechafin, idtransaccion);
        }
    }
}
