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
    }
}
