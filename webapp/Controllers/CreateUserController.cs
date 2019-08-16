﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using webapp.Helpers;
using webapp.Models;

namespace webapp.Controllers
{
    public class CreateUserController : Controller
    {
        [HttpPost]
        public JsonResult InsertUser(Dictionary<string, object> data)

        {
            RespGeneric resp = new RespGeneric("KO");

            try
            {
                //comprobar formato correcto de email
                String expresion;
                expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
                if (Regex.IsMatch(Convert.ToString(data["email"]), expresion))
                {
                    if (Regex.Replace(Convert.ToString(data["email"]), expresion, String.Empty).Length == 0)
                    {
                        //comprobar si usuario que se va a crear esta en la base de datos
                        if (Webapp.Data.Empleados.getByEmail(data["email"].ToString()) == null)
                        {
                            int longitud = 7;
                            Guid miGuid = Guid.NewGuid();
                            string token = Convert.ToBase64String(miGuid.ToByteArray());
                            token = token.Replace("=", "").Replace("+", "");
                            Console.WriteLine(token.Substring(0, longitud));
                            data.Add("invitacion",token);


                            Webapp.Data.Empleados.InsertUser(data);
                            resp.cod = "OK";
                            //enviar email a la direccion email dentro del data
                            Extensiones.sendEmail(to: (Convert.ToString(data["email"])), subject: (Convert.ToString(data["invitacion"])), body: "Introduce el codigo que te hemos enviado junto con tus datos para finalizar el registro  <a href=http://localhost:51934/Home/FormularioRegistro> link de registro </a> ", file: "");
                        }

                        else
                        {
                            resp.msg = "Ya un usuario con ese Email";
                        }
                    }
                    else
                    {
                        resp.msg = "El Email no es Valido";
                    }
                    }
                else
                    {
                    resp.msg = "El Email no es Valido";
                }

            }

            catch (Exception e)
            {
                if (e.Message != "")
                    resp.msg = e.Message;
            }
            return Json(resp);
        }
        


        public JsonResult UpdateUser(Dictionary<string, object> data)

        {
            RespGeneric resp = new RespGeneric("KO");
            try
            {
                if (Webapp.Data.Empleados.getByInvitacion(data["invitacion"].ToString()) != null)
                {

                    var passhashed = BD.HashPassword(pass: (Convert.ToString(data["pass"])), salt: "");
                    data.Add("ClaveHashed", passhashed);
                    Webapp.Data.Empleados.UpdateUser(data);
                }
                else
                {
                    resp.msg = "El codigo de invitacion no es valido";
                }
            }

            catch (Exception e)
            {
                return null;
            }
            return Json(resp);

        }
    }

}

