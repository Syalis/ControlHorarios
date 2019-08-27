using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webapp.Helpers;
using webapp.Models;

namespace webapp.Controllers
{
    public class PerfilController : Controller
    {
        //Metodo para actualizar el nombre de usuario y apellidos del usuario
       [HttpPost]
       public JsonResult setNombre(Dictionary<string, object> nombreUsuario, int id)
       {
            RespGeneric resp = new RespGeneric("KO");
            resp.msg = validarUsuario(nombreUsuario);
            if (string.IsNullOrEmpty(resp.msg))
            {
                try
                {
                    resp.d.Add("setNombre", Data.Perfil.setNombre(nombreUsuario, id));
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

        private string validarUsuario(Dictionary<string, object> nombreUsuario)
        {
            string msg = string.Empty;
            if (!Util.hasValidOrEmptyValue(nombreUsuario, "nombre", "string"))
            {
                msg = "Debe indicar el Nombre";
            }
            else if (!Util.hasValidOrEmptyValue(nombreUsuario, "primer_apellido", "string"))
            {
                msg = "Debe indicar el Primer Apellido";
            }
            else if (!Util.hasValidOrEmptyValue(nombreUsuario, "segundo_apellido", "string"))
            {
                msg = "Debe indicar el Segundo Apellido";
            
            
            }
            return msg;
        }
        //Metodo para actualizar email
        [HttpPost]
        public JsonResult setEmail(Dictionary<string, object> emailUsuario, int id)
        {
            RespGeneric resp = new RespGeneric("KO");
            resp.msg = validarEmail(emailUsuario);
            if (string.IsNullOrEmpty(resp.msg))
            {
                try
                {
                    resp.d.Add("setEmail", Data.Perfil.setEmail(emailUsuario, id));
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



        //Metodo para actualizar departamento
        [HttpPost]
        public JsonResult setDepartamento(Dictionary<string, object> departamentoUsuario, int id1)
        {
            RespGeneric resp = new RespGeneric("KO");
            resp.msg = validarDepartamento(departamentoUsuario);
            if (string.IsNullOrEmpty(resp.msg))
            {
                try
                {
                    resp.d.Add("setDepartamento", Data.Perfil.setDepartamento(departamentoUsuario, id1));
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
        //Metodo para validar email
        private static string validarEmail(Dictionary<string, object> emailUsuario)
        {
            string msg = string.Empty;
            if (!Util.hasValidOrEmptyValue(emailUsuario, "email", "string"))
            {
                msg = "Debe indicar el Email";
            }
            return msg;
        }
        //Metodo para validar departamento
        private static string validarDepartamento(Dictionary<string, object> departamentoUsuario)
        {
            string msg = string.Empty;
            if (!Util.hasValidOrEmptyValue(departamentoUsuario, "id", "int"))
            {
                msg = "Debe indicar el Departamento";
            }
            return msg;
        }
    }
}
