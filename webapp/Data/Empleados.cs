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
        public static List<Dictionary<string, object>> getPass()
        {
            return BD.getQueryResult($"select u.password from usuarios u ");
        }
        //comprobar email
        public static Dictionary<string, object> getByEmail(string email)
        {
            return getAll().Where (p => Convert.ToString(p["email"].ToString()) == email ).FirstOrDefault();
        }

       //insertar usuario (EMAIL,ACTIVO,NOMBRE 1 Y 2 APELLIDO , SI ESTA VALIDADO EL CORREO Y SALT   
        public static int InsertUser(Dictionary<string, object> data)
        {
            return BD.getInsertQueryResult("INSERT INTO usuarios (email, activo,salt,invitacion) VALUES (?email,'0','',?invitacion)", data);

        }
        //registro de usuario
        public static int UpdateUser(Dictionary<string, object> data)
        {
            return BD.getNonQueryResult("UPDATE usuarios  SET password = ?ClaveHashed, telefono = ?telefono, validado = '1' ,nombre = ?nombre , primer_apellido= ?primer_apellido , segundo_apellido = ?segundo_apellido, salt =  ?salt, id_perfil = '1'  WHERE invitacion = ?invitacion " , data);
        }
        //comprobar invitacion de registro
        public static Dictionary<string, object> getByInvitacion(string invitacion)
        {
            return getAll().Where(p => Convert.ToString(p["invitacion"].ToString()) == invitacion).FirstOrDefault();
        }
        //introducir codigo para restablecer contraseña

        public static int InsertCodigoPass(Dictionary<string, object> data)
        {
            return BD.getNonQueryResult("UPDATE usuarios set codigo_forgot_pass = ?codigo WHERE email = ?email", data);

        }

        //actualizar la contraseña
        public static int resetPass(Dictionary<string, object> data)
        {
            return BD.getNonQueryResult("UPDATE usuarios  SET password = ?ClaveHashed, salt = ?salt  WHERE codigo_forgot_pass = ?codigo ", data);
        }
        //comprobar que el codigo es valido
        public static Dictionary<string, object> getByCodigo(string codigo)
        {
            return getAll().Where(p => Convert.ToString(p["codigo_forgot_pass"].ToString()) == codigo).FirstOrDefault();
        }
        //eliminar codigo de restablecer contraseña
        public static int DeleteCodigo(Dictionary<string, object> data)
        {
            return BD.getNonQueryResult("UPDATE usuarios  SET codigo_forgot_pass = '0'  WHERE codigo_forgot_pass = ?codigo ", data);
        }
        public static List<Dictionary<string, object>> getSalt()
        {
            return BD.getQueryResult($"select u.* from usuarios u ");
        }
        //comprobar que la conraseña no es la misma para restablecerla
        public static Dictionary<string, object> getByPass(string pass1)
        {
            return getAll().Where(p => Convert.ToString(p["password"].ToString()) == pass1).FirstOrDefault();
        }

    }
}