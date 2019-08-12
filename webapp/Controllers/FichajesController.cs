using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webapp.Models;

namespace webapp.Controllers
{
    //Controllador de los metodos para los fichajes de empleados
    public class FichajesController : Controller
    {
        //Metodo para iniciar la carga de fichajes hasta el momento del empleado en el mes actual por su id
        [HttpPost]
        public JsonResult getInicioFichajes(int id)
        {
            RespGeneric resp = new RespGeneric("KO");
            resp.msg = validarIntEmpleado(id);

            if (string.IsNullOrEmpty(resp.msg))
            {
                try
                {
                    resp.d.Add("data", Data.Fichajes.getInicioFichajes(id));
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
            if (id <= 0){
                msg = "Ha habido un error con los datos del empleado";
            }
            return msg;
        }
















        //// GET: Fichajes
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: Fichajes/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Fichajes/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Fichajes/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Fichajes/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Fichajes/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Fichajes/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Fichajes/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
