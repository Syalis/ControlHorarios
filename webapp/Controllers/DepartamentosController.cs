using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webapp.Models;

namespace webapp.Controllers
{
    public class DepartamentosController : Controller
    {
        //Metodo para obtener todos los usuarios con su departamento para representar en tabla
        [HttpPost]
        public JsonResult getDepartamentosTabla()
        {
            RespGeneric resp = new RespGeneric("KO");
            resp.msg = string.Empty;

            try
            {
                resp.d.Add("departamentosTabla", Data.Departamentos.getDepartamentosTabla());
                resp.cod = "OK";
                resp.msg = "Exito en la petición";
            }
            catch (Exception e)
            {
                resp.msg = e.Message;
            }

            return Json(resp);

        }
        //Metodo para obtener todos los departamentos para el dropdown
        [HttpPost]
        public JsonResult getDepartamentosTotal()
        {
            RespGeneric resp = new RespGeneric("KO");
            resp.msg = string.Empty;

            try
            {
                resp.d.Add("data", Data.Departamentos.getDepartamentosTotal());
                resp.cod = "OK";
                resp.msg = "Exito en la petición";
            }
            catch (Exception e)
            {
                resp.msg = e.Message;
            }

            return Json(resp);

        }
        //Metodo para filtrar y buscar por id de departamento y mostrar en tabla
        [HttpPost]
        public JsonResult getDepartamentoEmpleados(int id)
        {
            RespGeneric resp = new RespGeneric("KO");
            resp.msg = string.Empty;

            try
            {
                resp.d.Add("getDepartamentoEmpleados", Data.Departamentos.getDepartamentoEmpleados(id));
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
