using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webapp.Models;

namespace webapp.Controllers
{
    public class lateralController : Controller
    {
        //Metodo para iniciar el dropdown con id y nombres
        [HttpPost]
        public JsonResult getNombresDropdown()
        {
            RespGeneric resp = new RespGeneric("KO");
            resp.msg = string.Empty;

            try
            {
                //Comprobacion del estado del boton+fichajes del mes en curso+totales de horas
               
                resp.d.Add("data", Data.lateral.getNombresDropdown());
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
