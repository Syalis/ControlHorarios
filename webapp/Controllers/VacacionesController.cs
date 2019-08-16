using System;
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
        [HttpPost]
        public JsonResult getDiasVacaciones(Dictionary<string, object> item)
        {
            RespGeneric resp = new RespGeneric("KO");
           

            if (string.IsNullOrEmpty(resp.msg))
            {
                try
                {
                    //item["fecha_inicio_vacaciones"] = DateTime.Parse(item["fecha_inicio_vacaciones"].ToString());
                    //item["fecha_final_vacaciones"] = DateTime.Parse(item["fecha_final_vacaciones"].ToString());
                    resp.d.Add("data", Data.Vacaciones.getDiasTotalVacaciones(item));
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
                    //item["fecha_inicio_vacaciones"] = DateTime.Parse(item["fecha_inicio_vacaciones"].ToString());
                    //item["fecha_final_vacaciones"] = DateTime.Parse(item["fecha_final_vacaciones"].ToString());
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

        [HttpPost]
        public JsonResult createPeticionVacaciones(Dictionary<string, object> item)
        {
            RespGeneric resp = new RespGeneric("KO");
            resp.msg = validarFecha(item) + verificarFechas(item); ;

            
            if (string.IsNullOrEmpty(resp.msg))
            {
                var fecha1 = DateTime.Parse(item["fecha_inicio_vacaciones"].ToString());
                var fecha2 = DateTime.Parse(item["fecha_final_vacaciones"].ToString());

                TimeSpan dif = fecha2 - fecha1;

                int dias = dif.Days;
                var d = dias + 1;
                var a = Data.Vacaciones.getDiasTotalVacaciones(item);

                foreach (var x in a)
                {
                    x["total_vacaciones"].ToString();
                }

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
        /// Validación de los campos fechas
        /// </summary>
        /// <param name="item">parámetro donde se recogen la fecha de incio y la fecha final</param>
        /// <returns>Mensaje de error si no introduce una fecha</returns>
        private string validarFecha(Dictionary<string, object> item)
        {
            RespGeneric resp = new RespGeneric();
            string msg = string.Empty;

            try {

                if (!Util.hasValidValue(item, "fecha_inicio_vacaciones", "datetime") && !string.IsNullOrEmpty(item["fecha_inicio_vacaciones"].ToString()))
                {
                    msg = "Introduce una fecha inicio";
                }
                else if (!Util.hasValidValue(item, "fecha_final_vacaciones", "datetime") && !string.IsNullOrEmpty(item["fecha_final_vacaciones"].ToString()))
                {
                    msg = "Introduce una fecha final";
                }
            }
            catch (Exception ex)
            {
                resp.msg = ex.Message;
            }

            return msg;

        }

        /// <summary>
        /// Método para comprobar las fecha de inicio de vacaciones no sea mayor que la fecha final de vacaciones
        /// </summary>
        /// <param name="item">parámetro donde se recogen la fecha de incio y la fecha final</param>
        /// <returns>Mensaje de error si la fecha de inicio es superior a la fecha final</returns>
        public string verificarFechas(Dictionary<string, object> item)
        {
            RespGeneric resp = new RespGeneric();
            string msg = string.Empty;


            try
            {
                if (DateTime.Parse(item["fecha_inicio_vacaciones"].ToString()) > DateTime.Parse(item["fecha_final_vacaciones"].ToString()))
                {
                    msg = "Fecha de inicio de vacaciones no puede ser posterior a la fecha final de vaciones";
                }
            }
            catch (Exception ex)
            {
                resp.msg = ex.Message;
            }

            return msg;
        }

        /// <summary>
        /// Método para comprobar que los dias solicitados no excenden de los días total de vacaciones 
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Mensaje de error si los dias de vaciones excede de los dias de vaciones restantes</returns>
    //    public string verificarDiasRestantesVacaciones(Dictionary<string, object> item)
    //    {
    //        RespGeneric resp = new RespGeneric();
    //        string msg = string.Empty;
     
             

    //        try
    //        {
    //            var fecha1 = DateTime.Parse(item["fecha_inicio_vacaciones"].ToString());
    //            var fecha2 = DateTime.Parse(item["fecha_final_vacaciones"].ToString());

    //            TimeSpan dif = fecha2 - fecha1;

    //            int dias = dif.Days;
    //            var a = Data.Vacaciones.getDiasTotalVacaciones(item);

    //            foreach (var x in a)
    //            {
    //                x["total_vacaciones"].ToString();
    //            }
    //            var b = Convert.ToInt32(a[0]["total_vacaciones"].ToString());

    //            if (b == 0)
    //            {
    //                b = 30;
    //            }
                

    //            if ((dias +1) <= b) {

    //                msg = "El intervalo de fechas introducidas no supera a tus días de vacaciones";

    //            }
    //            else {
    //                msg = "El intervalo de fechas introducidas supera a tus días de vacaciones";
    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            resp.msg = ex.Message;
    //        }

    //        return msg;
    //    }

    }
}