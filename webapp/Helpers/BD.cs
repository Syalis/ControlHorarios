using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using Medo.Security.Cryptography;
using System.Data;
using System.Text;
using System.Diagnostics;

namespace webapp.Helpers
{
    public class BD
    {
        private static readonly string server = "192.168.0.107";
        private static readonly string schema = "control_horarios";

        public const string BDuser = "horarios";
        public const string BDpass = "fu3i4r378r12g";

        private static readonly int passIterations = 12000;

        public const int PERFIL_ADMIN = 1;
        public const int PERFIL_ESTRUCTURAL = 2;

        public static String CadConMySQL()
        {
            MySqlConnectionStringBuilder cadCon = new MySqlConnectionStringBuilder();

            cadCon.Server = server;

            cadCon.UserID = BDuser;
            cadCon.Password = BDpass;

            cadCon.Database = schema;
            cadCon.PersistSecurityInfo = true;
            cadCon.AllowZeroDateTime = false;
            cadCon.ConnectionTimeout = 200;
            cadCon.Pooling = true;
            cadCon.MaximumPoolSize = 200;
            cadCon.AllowUserVariables = true;

            return cadCon.ConnectionString;
        }

        public static DataTable getQueryResultDt(string query, Dictionary<string, object> parameters = null)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL()))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> par in parameters)
                        {
                            cmd.Parameters.AddWithValue($"?{par.Key}", par.Value);
                        }
                    }

                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        public static int getNonQueryResult(string query, Dictionary<string, object> parameters = null, bool log = true)
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            int nrows = 0;

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL()))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> par in parameters)
                        {
                            cmd.Parameters.AddWithValue($"?{par.Key}", par.Value);
                            sb.Append($"{par.Key}={par.Value}; ");
                        }
                    }
                    nrows = cmd.ExecuteNonQuery();
                }
                
            }
            return nrows;
        }

        public static int getInsertQueryResult(string query, Dictionary<string, object> parameters = null, bool log = true)
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            int newId = 0;

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL()))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> par in parameters)
                        {
                            cmd.Parameters.AddWithValue($"?{par.Key}", par.Value);
                            sb.Append($"{par.Key}={par.Value}; ");
                        }
                    }

                    cmd.ExecuteNonQuery();
                    newId = Convert.ToInt32(cmd.LastInsertedId);


                }
                con.Close();
            }
            return newId;
        }

        public static List<Dictionary<string, object>> getQueryResult(string query, Dictionary<string, object> parameters = null)
        {
            return getQueryResultDt(query, parameters).ToList();
        }

        #region SECURITY
      
        public static bool CheckPassword(string passwordHashed, string pass)
        {
            if (!string.IsNullOrEmpty(passwordHashed))
            {
                String salt = passwordHashed.Substring(20, 12);
                String hashed = HashPassword(pass, salt);

                if (hashed.Equals(passwordHashed))
                {
                    return true;
                }
            }
            return false;
        }

        public static string HashPassword(string pass, string salt)

        {
            int passIterations = 12000;

            var hashed = String.Empty;

            using (var hmac = new HMACSHA256())
            {
                var df = new Pbkdf2(hmac, pass, salt, passIterations);

                String passEncrypted = Convert.ToBase64String(df.GetBytes(32));

                hashed = String.Format("pbkdf2_sha256${0}${1}${2}", passIterations, salt, passEncrypted);

            }

            return hashed;
        }

        private static string CalculateSha1(string text, System.Text.Encoding enc)
        {
            byte[] buffer = enc.GetBytes(text);
            SHA1CryptoServiceProvider cryptoTransformSha1 = new SHA1CryptoServiceProvider();
            string hash = BitConverter.ToString(cryptoTransformSha1.ComputeHash(buffer)).Replace("-", "");
            
            return hash;
        }

        public static String CreateSalt(Int32 num)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            Byte[] buff = new Byte[num];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public static string getDecodeParam(string param)
        {
            byte[] encryptedstring;
            param = HttpUtility.UrlDecode(param.ToString());
            encryptedstring = Convert.FromBase64String(param.Replace('-', '+').Replace('_', '/'));
            param = System.Text.Encoding.UTF8.GetString(encryptedstring.ToArray());
            return param;
        }

        #endregion


        public static void addLogErrores(string metodo, string message, string traceback)
        {
            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL()))
            {
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO log_errores(metodo, message, traceback,fecha,idusuario) " +
                                  "VALUES (?metodo, ?message,?traceback,?fecha,?idusuario)", con))
                {

                    cmd.Parameters.AddWithValue("?metodo", metodo);
                    cmd.Parameters.AddWithValue("?message", message);
                    cmd.Parameters.AddWithValue("?traceback", traceback);
                    cmd.Parameters.AddWithValue("?idusuario", HttpContext.Current.Session["id"]);
                    cmd.Parameters.AddWithValue("?fecha", DateTime.Now);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public static DataTable getUsuario(string usuario)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL()))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        cmd.CommandText = "SELECT * FROM usuarios WHERE usuario=?usuario;";
                        cmd.Parameters.AddWithValue("?usuario", usuario);
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }
    }
}