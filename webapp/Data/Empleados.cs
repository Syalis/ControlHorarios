using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapp.Helpers;
using System.Web;


namespace Webapp.Data
{
    public class Empleados
    {
        //obtener todos los datos de usuario
        public static List<Dictionary<string, object>> getAll()
        {
            return BD.getQueryResult($"select u.* from usuarios u ");
        }

        public static Dictionary<string, object> getByEmail(string email)
        {
            return getAll().Where(p => Convert.ToString(p["email"].ToString()) == email).FirstOrDefault();
        }

       //insertar usuario (EMAIL,ACTIVO,NOMBRE 1 Y 2 APELLIDO , SI ESTA VALIDADO EL CORREO Y SALT   
        public static int InsertUser(Dictionary<string, object> data)
        {
            return BD.getInsertQueryResult("INSERT INTO usuarios (email, activo,salt,invitacion) VALUES (?email,'0','',?invitacion)", data);

        }
        public static int UpdateUser(Dictionary<string, object> data)
        {
            return BD.getNonQueryResult("UPDATE usuarios  SET password = ?ClaveHashed, telefono = ?telefono, validado = '1' ,nombre = ?nombre , primer_apellido= ?primer_apellido , segundo_apellido = ?segundo_apellido  WHERE invitacion = ?invitacion " , data);
        }
        public static Dictionary<string, object> getByInvitacion(string invitacion)
        {
            return getAll().Where(p => Convert.ToString(p["invitacion"].ToString()) == invitacion).FirstOrDefault();
        }
        public static int InsertCodigoPass(Dictionary<string, object> data)
        {
            return BD.getNonQueryResult("UPDATE usuarios set codigo_forgot_pass = ?codigo WHERE email = email", data);

        }

    }
}