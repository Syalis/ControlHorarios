using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webapp.Models;

namespace webapp.Controllers
{
    public class EmpleadosTablaController : Controller
    {
        //Metodo para obtener todos los empleados para la tabla
        [HttpPost]
        public JsonResult getEmpleadosTabla()
        {
            RespGeneric resp = new RespGeneric("KO");
            resp.msg = string.Empty;
                       
            try
            {
                    resp.d.Add("tablaEmpleado", Data.EmpleadosTabla.getEmpleadosTabla());
                    resp.cod = "OK";
                    resp.msg = "Exito en la petición";
            }
            catch (Exception e)
            {
                    resp.msg = e.Message;
            }
            
            return Json(resp);
        }
        //Metodo para fltrar todos los empleados para la tabla por id
        [HttpPost]
        public JsonResult getEmpleadosFiltrados(int id)
        {
            RespGeneric resp = new RespGeneric("KO");
            resp.msg = string.Empty;

            try
            {
                resp.d.Add("getEmpleadosFiltrados", Data.EmpleadosTabla.getEmpleadosFiltrados(id));
                resp.cod = "OK";
                resp.msg = "Exito en la petición";
            }
            catch (Exception e)
            {
                resp.msg = e.Message;
            }

            return Json(resp);
        }
    }
}
