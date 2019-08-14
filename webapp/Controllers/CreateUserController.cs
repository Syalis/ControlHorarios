using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webapp.Helpers;

namespace webapp.Controllers
{
    public class CreateUserController : Controller
    {
        public JsonResult InsertUser(Dictionary<string, object> data)

        {

            try
            {
                //enviar email a la direccion email dentro del data
                Extensiones.sendEmail(to: (Convert.ToString(data["email"])), subject: "Invitacion", body: " <a href=http://localhost:51934/Home/FormularioRegistro > link para invitacion</a>", file: "");
            }
            catch (Exception e)
            {
                return null;
            }
            return Json(Webapp.Data.Empleados.InsertUser(data));

        }


        public JsonResult UpdateUser(Dictionary<string, object> data)

        {
            try
            {

                var passhashed = BD.HashPassword(pass: (Convert.ToString(data["pass"])), salt: "");
                data.Add("ClaveHashed", passhashed);

            }

            catch (Exception e)
            {
                return null;
            }
            return Json(Webapp.Data.Empleados.UpdateUser(data));

        }
    }

}

