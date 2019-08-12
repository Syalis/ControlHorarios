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
        /// Método de llamada a la consulta y donde se validan y se parsean los datos
        /// </summary>
        /// <param name="item">párametro donde se recogen la id del usuario y los días de vacaciones</param>
        /// <param name="item">parámetro donde se recogen la fecha de incio y la fecha final</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getDiasVacaciones(Dictionary<string, object> item)
        {
            RespGeneric resp = new RespGeneric("KO");
            resp.msg = verificarDiasRestantesVacaciones(item);

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

        [HttpPost]
        public JsonResult createPeticionVacaciones(Dictionary<string, object> item)
        {
            RespGeneric resp = new RespGeneric("KO");
            resp.msg = validarFecha(item) + verificarFechas(item);

            if (string.IsNullOrEmpty(resp.msg))
            {
                try
                {
                    item["fecha_inicio_vacaciones"] = DateTime.Parse(item["fecha_inicio_vacaciones"].ToString());
                    item["fecha_final_vacaciones"] = DateTime.Parse(item["fecha_final_vacaciones"].ToString());
                    resp.d.Add("data", Data.Vacaciones.create(item));
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
        public string verificarDiasRestantesVacaciones(Dictionary<string, object> item)
        {
            RespGeneric resp = new RespGeneric();
            string msg = string.Empty;
            const int diasVacaciones = 30;

            try
            {
                if (DateTime.Parse(item["dias_restantes"].ToString()) > DateTime.Parse(diasVacaciones.ToString()))
                {
                    msg = "Los días de vacaciones no pueden sobrepasar los días de vaciones restantes";
                }
            }
            catch (Exception ex)
            {
                resp.msg = ex.Message;
            }

            return msg;
        }

    }
}