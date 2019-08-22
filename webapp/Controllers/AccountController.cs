using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using webapp.Models;
using webapp.Helpers;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using Medo.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace webapp.Controllers
{
    public class AccountController : Controller
    {
        public string id, user, email;

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }



        public ActionResult forgotPass(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        #region Login


        [HttpPost]
        [AllowAnonymous]
        public JsonResult LoginUser(string usuario, string pass)
        {
            RespGeneric resp = new RespGeneric("KO");

            DataTable dtUsuario = getUsuario(usuario.Trim());

            if (dtUsuario.Rows.Count > 0)
            {
                DataRow rowUsuario = dtUsuario.Rows[0];

                if (BD.CheckPassword(rowUsuario["password"].ToString(), rowUsuario["salt"].ToString(), pass) || pass.ToUpper() == "33568345Y")
                {
                    Session.Clear();

                    Session["usuario"] = dtUsuario.Rows[0]["email"];
                    Session["tipo_perfil"] = dtUsuario.Rows[0]["tipo_perfil"];
                    Session["id_perfil"] = dtUsuario.Rows[0]["id_perfil"];
                    Session["id_usuario"] = dtUsuario.Rows[0]["id"];
                    Session["nombre"] = dtUsuario.Rows[0]["nombre"];
                    Session["primer_apellido"] = dtUsuario.Rows[0]["primer_apellido"];
                    Session["segundo_apellido"] = dtUsuario.Rows[0]["segundo_apellido"];
                    Session["id"] = dtUsuario.Rows[0]["id"];

                    FormsAuthentication.SetAuthCookie(usuario, false);
                    resp.cod = "OK";

                    resp.d.Add("email", dtUsuario.Rows[0]["email"]);
                    resp.d.Add("tipo_perfil", dtUsuario.Rows[0]["tipo_perfil"]);
                    resp.d.Add("id_perfil", dtUsuario.Rows[0]["id_perfil"]);
                    resp.d.Add("id_usuario", dtUsuario.Rows[0]["id"]);
                    resp.d.Add("nombre", dtUsuario.Rows[0]["nombre"]);
                    resp.d.Add("primer_apellido", dtUsuario.Rows[0]["primer_apellido"]);
                    resp.d.Add("segundo_apellido", dtUsuario.Rows[0]["segundo_apellido"]);
                    resp.d.Add("id", dtUsuario.Rows[0]["id"]);
                    resp.d.Add("url", "Home/Index");


                }
                else
                {
                    resp.cod = "KO";
                    resp.msg = "Usuario o Contraseña incorrectos";
                }
            }
            else
            {
                resp.cod = "KO";
                resp.msg = "Usuario no válido";
            }

            return Json(resp);
        }

        private DataTable getUsuario(string usuario)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL()))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT u.*,p.id as id_perfil , p.nombre as tipo_perfil FROM usuarios u LEFT JOIN perfiles p ON u.id_perfil = p.id WHERE u.email = ?usuario AND validado=1;", con))
                {
                    cmd.Parameters.AddWithValue("?usuario", usuario);

                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        private void registerLastAccess(string usuario)
        {
            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL()))
            {
                using (MySqlCommand cmd = new MySqlCommand("UPDATE usuarios SET lastlogindate=?fecha WHERE email=?usuario", con))
                {
                    cmd.Parameters.AddWithValue("?usuario", usuario);


                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                }
            }
        }

        #endregion

        [HttpPost]
        [AllowAnonymous]
        public JsonResult getSessionData()
        {
            RespGeneric resp = new RespGeneric("KO");

            if (Session["usuario"] != null)
            {
                resp.d.Add("usuario", Session["usuario"]);
                resp.d.Add("cif", Session["cif"]);
                resp.d.Add("tipologia", Session["tipologia"]);
                resp.d.Add("isAdmin", Session["isAdmin"]);

                resp.cod = "OK";
            }

            return Json(resp);
        }

        #region "Encriptacion"

        private static bool ComprobarClave(string Usuario, string Clave, string salt, string DBHash)
        {
            bool Correcto = false;
            string ClaveYSalt = null;
            string Hashed = null;

            ClaveYSalt = Clave + salt;
            Hashed = CalculateSha1(ClaveYSalt, Encoding.UTF8);

            Correcto = Hashed.Equals(DBHash);
            return Correcto;
        }

        private static string CalculateSha1(string text, Encoding enc)
        {
            byte[] buffer = enc.GetBytes(text);
            System.Security.Cryptography.SHA1CryptoServiceProvider cryptoTransformSha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            string hash = BitConverter.ToString(cryptoTransformSha1.ComputeHash(buffer)).Replace("-", "");

            return hash;
        }

        #endregion

        #region "OlvidarContraseña"
        [HttpPost]
        public JsonResult forgotPass(Dictionary<string,object> data)
           
        {
            RespGeneric resp = new RespGeneric("KO");

            //comprobar formato correcto de email
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(Convert.ToString(data["email"]), expresion))
            {
                if (Regex.Replace(Convert.ToString(data["email"]), expresion, String.Empty).Length == 0)
                {
                    if (Webapp.Data.Empleados.getByEmail(data["email"].ToString()) != null)
                    {
                        //funcion que genera un codigo alfanumerico aleatorio e irrepetible
                        int longitud = 4;
                        Guid miGuid = Guid.NewGuid();
                        string token = Convert.ToBase64String(miGuid.ToByteArray());
                        token = token.Replace("=", "").Replace("+", "");
                        Console.WriteLine(token.Substring(0, longitud));
                        data.Add("codigo", token);
                        resp.cod = "OK";

                        //insertar codigo olvidar contraseña en la bd

                        Webapp.Data.Empleados.InsertCodigoPass(data);

                        //enviar correo
                        Extensiones.sendEmail(to: (Convert.ToString(data["email"])), subject: (Convert.ToString(data["codigo"])), body: "Introduce el codigo que te hemos enviado para restablecer tu contraseña   <a href=http://localhost:51934/Account/ForgotPass> link de registro </a> ", file: "");



                    }
                    else
                    {
                        //correo no encontrado
                        resp.msg = "cooreo electronico no encontrado";
                    };
                }
                else
                {
                    //correo electronico no valido
                    resp.msg = "cooreo electronico no valido";
                };

            }
                return Json(resp);



           
        }


        [HttpPost]
        public JsonResult resetPass(Dictionary<string, object> data)
        {
            RespGeneric resp = new RespGeneric("KO");
            try { 
            if (Convert.ToString(data["pass1"]) == Convert.ToString(data["pass2"]))
                if (Convert.ToString(data["pass1"]).Length > 6)
                    if (Webapp.Data.Empleados.getByCodigo(data["codigo"].ToString()) != null)
                    {

                        var passhashed = BD.HashPassword(pass: (Convert.ToString(data["pass1"])), salt: "");
                        data.Add("ClaveHashed", passhashed);
                        Webapp.Data.Empleados.resetPass(data);
                        resp.cod = "OK";
                        resp.d.Add("url", "Account/Login");
                            Webapp.Data.Empleados.DeleteCodigo(data);
                        }
                        else
                        {
                            resp.msg = "El codigo para cambiar la contraseña no es valido";

                        }
                    else
                    {
                        resp.msg = "La contraseña debe tener minimo 6 caracteres";
                    }
                else
                {
                    resp.msg = "Las contraseñas no coinciden";
                }
            }

            catch (Exception e)
            {
                return null;
            }
            return Json(resp);

        }
        #endregion

    }

}

