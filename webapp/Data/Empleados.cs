using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webapp.Helpers;



namespace Webapp.Data
{
    public class Empleados
    {
        public static int InsertUser(Dictionary<string, object> data)
        {
            return BD.getInsertQueryResult("INSERT INTO usuarios (email, activo, nombre,primer_apellido,segundo_apellido,validado,salt) VALUES (?email,'0', ?nombre, ?primer_apellido,?segundo_apellido,'0','L_854WWWE*')", data);


        }





    }
}