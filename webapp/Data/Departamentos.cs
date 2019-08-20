using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webapp.Helpers;

namespace webapp.Data
{
    public class Departamentos
    {
        public static List<Dictionary<string, object>> getDepartamentosTabla()
        {
            return BD.getQueryResult($@"select  d.id as id, d.nombre as Departamento, upper(u.nombre) as Nombre, upper(concat_ws(' ', u.primer_apellido, u.segundo_apellido)) as Apellidos, 
                u.email as Correo from usuarios u left join departamentos d on d.id = u.departamento");
        }
    }
}