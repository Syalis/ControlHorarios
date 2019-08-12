using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace webapp.Helpers
{
    public class Util
    {
        /// <summary>
        /// Devuelve la URL base de la web, aunque esté en un subdirectorio.
        /// </summary>
        /// <returns>Url Base de la Web</returns>
        static public string getBaseUrl()
        {
            string baseUrl = $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Authority}{HttpContext.Current.Request.ApplicationPath.TrimEnd('/')}";

            return baseUrl;
        }

        public static string getSessionVar(string variable)
        {
            string value = string.Empty;
            if (HttpContext.Current.Session != null && HttpContext.Current.Session.Count > 0 && HttpContext.Current.Session[variable] != null)
            {
                value = HttpContext.Current.Session[variable].ToString();
            }
            return value;
        }

        /// <summary>
        /// Metodo para encriptar 
        /// </summary>
        /// <param name="toEncode"></param>
        /// <returns></returns>
        public static string stringToBase64(string toEncode)
        {
            try
            {
                byte[] toEncodeAsBytes = System.Text.UTF8Encoding.UTF8.GetBytes(toEncode);
                return Convert.ToBase64String(toEncodeAsBytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Método para desencriptar
        /// </summary>
        /// <param name="encodedData"></param>
        /// <returns></returns>
        public static string stringFromBase64(string encodedData)
        {
            try
            {
                byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
                return System.Text.Encoding.UTF8.GetString(encodedDataAsBytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static bool notEmptyValue(Dictionary<string, object> data, string campo)
        {
            return data.ContainsKey(campo) && data[campo] != null && !string.IsNullOrEmpty(data[campo].ToString());
        }

        public static bool dataFormatOk(Dictionary<string, object> data, string campo, string type)
        {
            bool ok = false;
            try
            {
                switch (type)
                {
                    case "string":

                        ok = true;
                        break;

                    case "int":

                        int res;
                        ok = int.TryParse(data[campo].ToString(), out res);
                        break;

                    case "double":

                        double resd;
                        ok = double.TryParse(data[campo].ToString(), out resd);
                        break;

                    case "datetime":

                        DateTime resf;
                        ok = (data[campo].ToString().Split('/').Length == 3 || data[campo].ToString().Split('-').Length == 3) && DateTime.TryParse(data[campo].ToString(), out resf);
                        break;
                }
            }
            catch
            {
            }
            return ok;
        }

        // Devuelve true si el diccionario contiene el campo especificado y este es del tipo indicado
        public static bool hasValidValue(Dictionary<string, object> data, string campo, string type)
        {
            bool ok = notEmptyValue(data, campo);
            if (ok)
            {
                ok = dataFormatOk(data, campo, type);
            }
            return ok;
        }

        public static bool hasValidOrEmptyValue(Dictionary<string, object> data, string campo, string type)
        {
            bool ok = !notEmptyValue(data, campo);
            if (!ok)
            {
                ok = dataFormatOk(data, campo, type);
            }
            return ok;
        }

        /// <summary>
        /// Método para leer datos de un fichero excel, y convertirlo a un Datatable
        /// Nuget packages: ExcelDataReader, ExcelDataReader.Dataset
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="hasHeader"></param>
        /// <returns></returns>
        //public static DataTable getDatatableFromExcel(string filepath, bool hasHeader)
        //{
        //    DataSet resultDataset = null;
        //    ExcelDataSetConfiguration excelReaderConf = new ExcelDataSetConfiguration();

        //    excelReaderConf.ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration
        //    {
        //        UseHeaderRow = hasHeader
        //    };

        //    using (System.IO.FileStream stream = System.IO.File.Open(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
        //    {
        //        // Auto-detect format, supports:
        //        //  - Binary Excel files (2.0-2003 format; *.xls)
        //        //  - OpenXml Excel files (2007 format; *.xlsx)
        //        using (var reader = ExcelReaderFactory.CreateReader(stream))
        //        {
        //            resultDataset = reader.AsDataSet(excelReaderConf);
        //        }
        //    }
        //    if (resultDataset != null && resultDataset.Tables.Count > 0)
        //    {
        //        return resultDataset.Tables[0];
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// Método para eliminar ficheros de un directorio
        /// Nuget packages: ExcelDataReader, ExcelDataReader.Dataset
        /// </summary>
        /// <param name="folderpath"></param>
        /// <param name="olderThan"></param>
        /// <returns></returns>
        public static int removeFiles(string folderPath, DateTime olderThan)
        {
            int removed = 0;

            try
            {
                string[] files = System.IO.Directory.GetFiles(folderPath);

                foreach (string file in files)
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(file);
                    if (fi.LastWriteTime < olderThan)
                    {
                        fi.Delete();
                        removed++;
                    }
                }
            }
            catch
            {

            }

            return removed;
        }

        public static void deleteImage(string imagen = "")
        {
            try
            {
                var auxImgUrl = imagen.Replace("../", "");
                var urlImage = HostingEnvironment.MapPath($"~/{auxImgUrl}");
                var fo = new FileInfo(urlImage);
                if (fo.Exists)
                    fo.Delete();
            }
            catch (Exception)
            {
            }
        }

        public static string removeSpecialCharacters(string str)
        {
            return Regex.Replace(str, @"[^0-9A-Za-z]", "", RegexOptions.None);
        }

        public static bool isImeiValid(string imei)
        {
            return imei.All(char.IsNumber) && imei.Length >= 15;
        }

        public static bool doubleValid(string number)
        {
            double result;
            return double.TryParse(number, NumberStyles.Any, CultureInfo.CurrentCulture, out result);
        }

        public static bool isEmailValid(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool isStrongPassword(string password)
        {
            HashSet<char> specialCharacters = new HashSet<char>() { '`', '~', '!', '@', '$', '%', '^', '&', '-', '+', '*', '/', '_', '=', ',', ';', '.', '\'', ':', '|', '\\', '(', ')', '[', ']', '{', '}' };
            return (password.Any(char.IsLower) && password.Any(char.IsUpper) && password.Any(char.IsDigit) && password.Any(specialCharacters.Contains) && password.Length >= 8);
        }

        public static double stringToDouble(string number)
        {
            double result;
            bool ok = double.TryParse(number, NumberStyles.Any, CultureInfo.CurrentCulture, out result);
            return (ok ? result : 0);
        }

        public static double dictionaryFieldToDouble(Dictionary<string, object> dict, string field)
        {
            return (dict.ContainsKey(field) ? stringToDouble(dict[field].ToString()) : 0);
        }

        public static int lastDayMonth(int month, int year)
        {
            int day = 0;
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    day = 31;
                    break;
                case 2:
                    day = (year % 4 == 0 ? 29 : 28);
                    break;
                default:
                    day = 30;
                    break;
            }
            return day;
        }

        public static string serializeList(List<Dictionary<string, object>> list, string key, bool withQuotes = false)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in list)
            {
                sb.Append($"{(withQuotes ? "'" : string.Empty)}").Append(item[key]).Append($"{(withQuotes ? "'" : string.Empty)}").Append(",");
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        public static string dictionaryToString(Dictionary<string, object> dict)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, object> kv in dict)
            {
                sb.Append(kv.Key).Append(": ").Append(kv.Value).Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        public static string dictionaryListToString(List<Dictionary<string, object>> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                foreach (KeyValuePair<string, object> kv in item)
                {
                    sb.Append(kv.Key).Append(": ").Append(kv.Value).Append(Environment.NewLine);
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets the IP address for the current request, returns 0.0.0.0 if HttpsContext does not exist.
        /// </summary>
        /// <returns></returns>
        public static string getIpAddress()
        {
            var context = HttpContext.Current;
            var ipAddress = String.Empty;

            if (context != null)
            {
                ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (String.IsNullOrWhiteSpace(ipAddress))
                {
                    ipAddress = context.Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {
                    var ipAddresses = ipAddress.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (ipAddresses.Length > 0)
                    {
                        ipAddress = ipAddresses[0];
                    }
                }

                ipAddress = String.IsNullOrWhiteSpace(ipAddress) ? context.Request.UserHostName : ipAddress;
            }
            return String.IsNullOrWhiteSpace(ipAddress) ? "0.0.0.0" : ipAddress;
        }

        public static bool validateEmail(String email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private static string getLetraNIF(int id)
        {
            Dictionary<int, String> letras = new Dictionary<int, string>();
            letras.Add(0, "T");
            letras.Add(1, "R");
            letras.Add(2, "W");
            letras.Add(3, "A");
            letras.Add(4, "G");
            letras.Add(5, "M");
            letras.Add(6, "Y");
            letras.Add(7, "F");
            letras.Add(8, "P");
            letras.Add(9, "D");
            letras.Add(10, "X");
            letras.Add(11, "B");
            letras.Add(12, "N");
            letras.Add(13, "J");
            letras.Add(14, "Z");
            letras.Add(15, "S");
            letras.Add(16, "Q");
            letras.Add(17, "V");
            letras.Add(18, "H");
            letras.Add(19, "L");
            letras.Add(20, "C");
            letras.Add(21, "K");
            letras.Add(22, "E");
            return letras[id];
        }

        private static bool validateNIF(string data)
        {
            if (data == String.Empty)
                return false;
            try
            {
                String letra;
                letra = data.Substring(data.Length - 1, 1);
                data = data.Substring(0, data.Length - 1);
                int nifNum = int.Parse(data);
                int resto = nifNum % 23;
                string tmp = getLetraNIF(resto);
                if (tmp.ToLower() != letra.ToLower())
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public static bool validateNIFNIE(string data)
        {
            if (String.IsNullOrEmpty(data) || data.Length < 8)
                return false;

            var initialLetter = data.Substring(0, 1).ToUpper();
            if (Char.IsLetter(data, 0))
            {
                switch (initialLetter)
                {
                    case "X":
                        data = "0" + data.Substring(1, data.Length - 1);
                        return validateNIF(data);
                    case "Y":
                        data = "1" + data.Substring(1, data.Length - 1);
                        return validateNIF(data);
                    case "Z":
                        data = "2" + data.Substring(1, data.Length - 1);
                        return validateNIF(data);
                    default:
                        return false;
                }
            }
            else if (Char.IsLetter(data, data.Length - 1))
            {
                if (new Regex("[0-9]{8}[A-Za-z]").Match(data).Success || new Regex("[0-9]{7}[A-Za-z]").Match(data).Success)
                    return validateNIF(data);
            }
            return false;
        }

        public static bool validateCIF(string cif)
        {
            try
            {
                int pares = 0;
                int impares = 0;
                int suma;
                string ultima;
                int unumero;
                string[] uletra = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "0" };
                string[] fletra = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
                int[] fletra1 = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
                string xxx;

                /*
                * T      P      P      N  N  N  N  N  C
                Siendo:
                T: Letra de tipo de Organización, una de las siguientes: A,B,C,D,E,F,G,H,K,L,M,N,P,Q,S.
                P: Código provincial.
                N: Númeración secuenial dentro de la provincia.
                C: Dígito de control, un número ó letra: Aó1,Bó2,Có3,Dó4,Eó5,Fó6,Gó7,Hó8,Ió9,Jó0.
                *
                *
                A.    Sociedades anónimas.
                B.    Sociedades de responsabilidad limitada.
                C.    Sociedades colectivas.
                D.    Sociedades comanditarias.
                E.    Comunidades de bienes y herencias yacentes.
                F.    Sociedades cooperativas.
                G.    Asociaciones.
                H.    Comunidades de propietarios en régimen de propiedad horizontal.
                I.    Sociedades civiles, con o sin personalidad jurídica.
                J.    Corporaciones Locales.
                K.    Organismos públicos.
                L.    Congregaciones e instituciones religiosas.
                M.    Órganos de la Administración del Estado y de las Comunidades Autónomas.
                N.    Uniones Temporales de Empresas.
                O.    Otros tipos no definidos en el resto de claves.
                */
                cif = cif.ToUpper();

                ultima = cif.Substring(8, 1);

                int cont = 1;
                for (cont = 1; cont < 7; cont++)
                {
                    xxx = (2 * int.Parse(cif.Substring(cont++, 1))) + "0";
                    impares += int.Parse(xxx.ToString().Substring(0, 1)) + int.Parse(xxx.ToString().Substring(1, 1));
                    pares += int.Parse(cif.Substring(cont, 1));
                }

                xxx = (2 * int.Parse(cif.Substring(cont, 1))) + "0";
                impares += int.Parse(xxx.Substring(0, 1)) + int.Parse(xxx.Substring(1, 1));

                suma = pares + impares;
                unumero = int.Parse(suma.ToString().Substring(suma.ToString().Length - 1, 1));
                unumero = 10 - unumero;
                if (unumero == 10) unumero = 0;

                if ((ultima == unumero.ToString()) || (ultima == uletra[unumero - 1]))
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool validateIBAN(string iban)
        {
            if (string.IsNullOrEmpty(iban) || (!Regex.IsMatch(iban, "^[A-Z0-9]")))
            {
                return false;
            }

            iban = iban.Replace(" ", String.Empty);

            string iban2 = iban.Substring(4, iban.Length - 4) + iban.Substring(0, 4);

            const int asciiShift = 55;
            var sb = new StringBuilder();
            foreach (char c in iban2)
            {
                int x = Char.IsLetter(c) ? c - asciiShift : int.Parse(c.ToString());
                sb.Append(x);
            }

            string checkSumString = sb.ToString();
            int checksum = int.Parse(checkSumString.Substring(0, 1));
            for (var i = 1; i < checkSumString.Length; i++)
            {
                int v = int.Parse(checkSumString.Substring(i, 1));
                checksum *= 10;
                checksum += v;
                checksum %= 97;
            }
            return (checksum == 1);
        }
    }
}
