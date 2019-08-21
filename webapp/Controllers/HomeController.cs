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
        public ActionResult FormularioRegistro(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        public ActionResult FichajesEmpleados()
        {
            return View();
        }


    }
 }
 