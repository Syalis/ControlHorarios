using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapp.Helpers;


namespace Webapp.Data
{
    public class Empleados
    {
        //metodo para comprobacion de que el usuario que se quiere insertar no repita email
        public static List<Dictionary<string, object>> getAll(bool controlSession = true)
        {
            return BD.getQueryResult($"select u.email from usuarios");
        }
        
        //insertar usuario (EMAIL,ACTIVO,NOMBRE 1 Y 2 APELLIDO , SI ESTA VALIDADO EL CORREO Y SALT   
        public static int InsertUser(Dictionary<string, object> data)
        {
            return BD.getInsertQueryResult("INSERT INTO usuarios (email, activo, nombre,primer_apellido,segundo_apellido,validado,salt) VALUES (?email,'0', ?nombre, ?primer_apellido,?segundo_apellido,'0','L_854WWWE*')", data);

        }
        public static Dictionary<string, object> getByField(string field, string value, bool controlSession = true)
        {
            return getAll(controlSession).Where(p => p[field].ToString() == value).FirstOrDefault();
        }


        public static int UpdateUser(Dictionary<string, object> data)
        {
            return BD.getNonQueryResult("UPDATE usuarios  SET password = ?ClaveHashed, telefono = ?telefono,validado = '1'  WHERE email = 'Prueba@1122.com' ", data);
        }

    }
}