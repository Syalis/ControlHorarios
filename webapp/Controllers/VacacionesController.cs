﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webapp.Helpers;
using webapp.Models;

namespace webapp.Controllers
{
    public class VacacionesController : Controller
    {


        /// <summary>
        /// Método de llamada a la consulta para que nos devuelva cuantos días de vacaciones nos queda
        /// </summary>
        /// <param name="item">párametro donde se recogen la id del usuario y los días de vacaciones</param>
        /// <param name="item">parámetro donde se recogen la fecha de incio y la fecha final</param>
        /// <returns></returns>
        //[HttpPost]
        //public JsonResult getDiasTotalVacaciones(Dictionary<string, object> item)
        //{
        //    RespGeneric resp = new RespGeneric("KO");
           

        //    if (string.IsNullOrEmpty(resp.msg))
        //    {
        //        try
        //        {
        //            resp.d.Add("data", Data.Vacaciones.getDiasTotalVacaciones(item));
        //            resp.cod = "OK";
                
        //        }
        //        catch (Exception ex)
        //        {
        //            resp.msg = ex.Message;
        //        }
        //    }

        //    return Json(resp);
        //}

        /// <summary>
        /// Método de llamada a la consulta para que nos devuelva cuantos días de vacaciones nos queda al pasar del año actual. 
        /// </summary>
        /// <param name="item">párametro donde se recogen la id del usuario y los días de vacaciones</param>
        /// <param name="item">parámetro donde se recogen la fecha de incio y la fecha final</param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult getDiasTotalVacacionesAnio(Dictionary<string, object> item)
        {
            RespGeneric resp = new RespGeneric("KO");
           
            if (string.IsNullOrEmpty(resp.msg))
            {
                try
                {
                    resp.d.Add("data", Data.Vacaciones.getDiasTotalVacacionesAnio(item));
                    resp.cod = "OK";
                
                }
                catch (Exception ex)
                {
                    resp.msg = ex.Message;
                }
            }

            return Json(resp);
        }

        /// <summary>
        /// Método de llamada a la consulta para que nos devuelva tos los registro de vacaciones
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getDiasVacacionesCalendario(Dictionary<string, object> item)
        {
            RespGeneric resp = new RespGeneric("KO");
   
            if (string.IsNullOrEmpty(resp.msg))
            {
                try
                {
                    resp.d.Add("data", Data.Vacaciones.getDiasCalendario(item));
                    resp.cod = "OK";
                }
                catch (Exception ex)
                {
                    resp.msg = ex.Message;
                }
            }

            return Json(resp);
        }

        /// <summary>
        /// Método para crear un evento de vacaciones
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult createPeticionVacaciones(Dictionary<string, object> item)
        {
            RespGeneric resp = new RespGeneric("KO");
            resp.msg = validarFecha(item) + verificarFechas(item); ;

            
            if (string.IsNullOrEmpty(resp.msg))
            {
                // Comprobación de los días a través de las fechas introducidas para que no sea mayor al total de vacaciones actual.
                var fecha1 = DateTime.Parse(item["fecha_inicio_vacaciones"].ToString());
                var fecha2 = DateTime.Parse(item["fecha_final_vacaciones"].ToString());

                TimeSpan dif = fecha2 - fecha1;

                int dias = dif.Days; // Diferencia de dias entre la fecha final y la fecha de inicio.
                var d = dias + 1; 
                var a = Data.Vacaciones.getDiasTotalVacaciones(item);
               //Recorremos el el campo "total_vacaciones" del diccionario. 
                foreach (var x in a)
                {
                    x["total_vacaciones"].ToString();
                }
                //Convertimos ese resultado en un número entero. 
                var b = Convert.ToInt32(a[0]["total_vacaciones"].ToString());

                try
                {

                    if (b >= d)
                    {
                        item["fecha_inicio_vacaciones"] = DateTime.Parse(item["fecha_inicio_vacaciones"].ToString());
                        item["fecha_final_vacaciones"] = DateTime.Parse(item["fecha_final_vacaciones"].ToString());
                        resp.d.Add("data", Data.Vacaciones.create(item));
                        resp.cod = "OK";
                        resp.msg = "El intervalo de fechas introducidas no supera a tus días de vacaciones";
                    }
                    else {
                        resp.msg = "Error";
                    }

                }
                catch (Exception ex)
                {
                    resp.msg = ex.Message;
                }
            }
            else {

                resp.msg = "Petición incorrecta";

            }


            return Json(resp);
        }

        /// <summary>
        /// Método para pasar de año
        /// </summary>
        /// <param name="item">Paárametro al quue se le pasa el año</param>
        /// <returns></returns>
        public JsonResult getYear(Dictionary<string, object> item)
        {
            RespGeneric resp = new RespGeneric("KO");
            resp.msg = string.Empty;

            if (string.IsNullOrEmpty(resp.msg))
            {
                try
                {
                    resp.d.Add("year", Data.Vacaciones.getYear(item));
                    resp.cod = "OK";
                }
                catch (Exception e)
                {
                    resp.msg = e.Message;
                }
            }
            return Json(resp);
        }

        public JsonResult envioEmail(Dictionary<string , object> email)
        {
            RespGeneric resp = new RespGeneric("KO");
            resp.msg = string.Empty;
            if (string.IsNullOrEmpty(resp.msg))
            {
                try
                {
                    //enviar email a la direccion email dentro del data
                    Extensiones.envioEmail(to: (Convert.ToString(email["email"])), subject: (Convert.ToString(email["invitacion"])), body: "Introduce el codigo que te hemos enviado junto con tus datos para finalizar el registro  <a href=http://localhost:51934/Account/FormularioRegistro> link de registro </a> ", file: "");
                    resp.d.Add("email", Data.Vacaciones.envioEmail(email));

                    resp.cod = "OK";
                }
                catch (Exception e)
                {
                    resp.msg = e.Message;
                }
            }
            return Json(resp);
        }


        /// <summary>
        /// Validación de los campos fechas para que no vengan vacios
        /// </summary>
        /// <param name="item">parámetro donde se recogen la fecha de incio y la fecha final</param>
        /// <returns>Mensaje de error si no introduce una fecha</returns>
        private string validarFecha(Dictionary<string, object> item)
        {
            string msg = string.Empty;

            if (!Util.hasValidValue(item, "fecha_inicio_vacaciones", "datetime") && !string.IsNullOrEmpty(item["fecha_inicio_vacaciones"].ToString()))
            {
                msg = "Introduce una fecha inicio";
            }
            else if (!Util.hasValidValue(item, "fecha_final_vacaciones", "datetime") && !string.IsNullOrEmpty(item["fecha_final_vacaciones"].ToString()))
            {
                msg = "Introduce una fecha final";
            }

            return msg;

        }

        /// <summary>
        /// Método para comprobar las fecha de inicio de vacaciones no sea mayor que la fecha final de vacaciones y que la fecha de inicio no sea
        /// inferior a la fecha actual
        /// </summary>
        /// <param name="item">parámetro donde se recogen la fecha de incio y la fecha final</param>
        /// <returns>Mensaje de error si la fecha de inicio es superior a la fecha final</returns>
        public string verificarFechas(Dictionary<string, object> item)
        {

            string msg = string.Empty;
            var fechaActual = DateTime.Today;

            if (DateTime.Parse(item["fecha_inicio_vacaciones"].ToString()) > DateTime.Parse(item["fecha_final_vacaciones"].ToString()))
            {
                msg = "Fecha de inicio de vacaciones no puede ser posterior a la fecha final de vaciones";

            }
            else if (DateTime.Parse(item["fecha_inicio_vacaciones"].ToString()) < fechaActual)
            {
                msg = "Fecha de inicio de vacaciones no puede ser posterior a la fecha actual";
            }

            return msg;
        }

    }
}