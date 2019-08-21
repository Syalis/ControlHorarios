using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webapp.Models;

namespace webapp.Controllers
{
    public class FichajesEmpleadosController : Controller
    {
        //Metodo para iniciar la carga de fichajes hasta el momento del empleado en el mes actual por su id+comprobacion del boton de check
        [HttpPost]
        public JsonResult getInicioFichajes(int id)
        {
            RespGeneric resp = new RespGeneric("KO");
            resp.msg = validarIntEmpleado(id);

            if (string.IsNullOrEmpty(resp.msg))
            {
                try
                {
                    
                    resp.d.Add("fichajesTotales", Data.FichajesEmpleados.getTotalesFichajes(id));
                    var fichajes = Data.FichajesEmpleados.getMesFichajes(id);
                    foreach (var a in fichajes)
                    {
                        a["horas"] = a["horas"].ToString().Split(',').ToArray();
                    }


                    resp.d.Add("mesFichajes", fichajes);
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
        //Metodo para validar el id del empleado
        public string validarIntEmpleado(int id)
        {
            string msg = string.Empty;
            if (id <= 0)
            {
                msg = "Ha habido un error con los datos del empleado";
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
                   
                    resp.d.Add("fichajesTotalesResta", Data.FichajesEmpleados.getTotalesFichajesResta(id, nMes));
                    var fichajes = Data.FichajesEmpleados.mesResta(id, nMes);
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
