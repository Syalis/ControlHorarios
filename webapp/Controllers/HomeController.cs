using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using webapp.Helpers;
using MySql.Data.MySqlClient;
using webapp.Models;

namespace webapp.Controllers
{
    public class HomeController : Controller
    {
        //Vistas de la app
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FormularioAdmin()
        {
            return View();
        }
        public ActionResult Empleados()
        {
            return View();
        }
        public ActionResult Vacaciones()
        {
            return View();
        }
        public ActionResult Perfil()
        {
            return View();
        }
        public ActionResult Departamentos()
        {
            return View();
        }
        //Fin de las vistas
        [HttpPost]
         [AllowAnonymous]
        public JsonResult InsertUser(Dictionary<string, object> data)
        {

            try
            {


            }
            catch (Exception e)
            {
                return null;
            }
            return Json(Webapp.Data.Empleados.InsertUser(data));
        }
    }
}