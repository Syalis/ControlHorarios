using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webapp.Helpers;

namespace webapp.Data
{
    public class lateral
    {
        //Metodo para obtener todos los usuarios y su id
        public static List<Dictionary<string, object>> getNombresDropdown()
        {
            return BD.getQueryResult($"SELECT id, upper(concat_ws(' ', nombre, primer_apellido, segundo_apellido)) as nombre FROM usuarios");
        }
    }
}