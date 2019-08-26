using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webapp.Models;
using webapp.Helpers;

namespace webapp.Controllers
{
    //Controllador de los metodos para los fichajes de empleados
    public class FichajesController : Controller
    {
        //Metodo para iniciar la carga de fichajes hasta el momento del empleado en el mes actual por su id+comprobacion del boton de check
        //[HttpPost]
        //public JsonResult getInicioFichajes(int id)
        //{
        //    RespGeneric resp = new RespGeneric("KO");
        //    resp.msg = validarIntEmpleado(id);

        //    if (string.IsNullOrEmpty(resp.msg))
        //    {
        //        try
        //        {
        //            //Comprobacion del estado del boton+fichajes del mes en curso+totales de horas
        //            resp.d.Add("boton", Data.Fichajes.getEstadoBoton(id));
        //            resp.d.Add("fichajesTotales", Data.Fichajes.getTotalesFichajes(id));
        //            var fichajes = Data.Fichajes.getMesFichajes(id);
        //            foreach (var a in fichajes)
        //            {
        //               a["horas"]= a["horas"].ToString().Split(',').ToArray();
        //            }


        //            resp.d.Add("mesFichajes", fichajes);
        //            resp.cod = "OK";
        //            resp.msg = "Exito en la petición";
        //        }
        //        catch (Exception e)
        //        {
        //            resp.msg = e.Message;
        //        }
        //    }
        //    return Json(resp);


        //}
        //Metodo para validar el id del empleado
        public string validarIntEmpleado(int id)
        {
            string msg = string.Empty;
            if (id <= 0){
                msg = "Ha habido un error con los datos del empleado";
            }
            return msg;
        }

        //Metodo para hacer CheckIn

        [HttpPost]
        public JsonResult CheckIn(Dictionary<string, object> checkIn)
        {
            RespGeneric resp = new RespGeneric("KO");
            resp.msg = validarCheckIn(checkIn);
            if (string.IsNullOrEmpty(resp.msg))
            {
                try
                {
                    resp.d.Add("data", Data.Fichajes.CheckIn(checkIn));
                    resp.cod = "OK";
                    resp.msg = "Exito en la petición";
                }
                catch (Exception e)
                {
                    resp.msg = e.Message;
                }
            }
            return Json(resp);


        }


        //Metodo para hacer CheckOut
        [HttpPost]
        public JsonResult CheckOut(Dictionary<string, object> checkOut)
        {
            RespGeneric resp = new RespGeneric("KO");
            resp.msg = validarCheckOut(checkOut);
            if (string.IsNullOrEmpty(resp.msg))
            {
                try
                {
                    resp.d.Add("data", Data.Fichajes.CheckOut(checkOut));
                    resp.cod = "OK";
                    resp.msg = "Exito en la petición";
                }
                catch (Exception e)
                {
                    resp.msg = e.Message;
                }
            }
            return Json(resp);


        }


        //Metodo para validar el id_usuario en el checkin
        public string validarCheckIn(Dictionary<string,object> checkIn)
        {
            string msg = string.Empty;
            if(!Util.hasValidValue(checkIn, "id_usuario", "int"))
            {
                msg = "La id de usuario es necesaria";
            }
            return msg;
        }
        //Metodo para validar el id_usuario en el checkout
        public string validarCheckOut(Dictionary<string, object> checkOut)
        {
            string msg = string.Empty;
            if (!Util.hasValidValue(checkOut, "id_usuario", "int"))
            {
                msg = "La id de usuario es necesaria";
            }
            return msg;
        }
        //Metodo para obtener fichajes de meses distintos al actual
        public JsonResult mesResta(int id, int nMes)
        {
            RespGeneric resp = new RespGeneric("KO");
            resp.msg = validarIntEmpleado(id);

            if (string.IsNullOrEmpty(resp.msg))
            {
                try
                {
                    //Comprobacion del estado del boton+fichajes del mes en curso+totales de horas
                    resp.d.Add("boton", Data.Fichajes.getEstadoBoton(id));
                    resp.d.Add("fichajesTotalesResta", Data.Fichajes.getTotalesFichajesResta(id, nMes));
                    var fichajes = Data.Fichajes.mesResta(id, nMes);
                    foreach (var a in fichajes)
                    {
                        a["horas"] = a["horas"].ToString().Split(',').ToArray();
                    }

                    resp.d.Add("mesFichajesResta", fichajes);
                    resp.cod = "OK";
                    resp.msg = "Exito en la petición";
                }
                catch (Exception e)
                {
                    resp.msg = e.Message;
                }
            }
            return Json(resp);
        }
        




    }
}
