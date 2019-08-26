using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webapp.Helpers;

namespace webapp.Data
{
    public class EmpleadosTabla
    {
        //Metodo para cargar empleados en la tabla
        public static List<Dictionary<string, object>> getEmpleadosTabla()
        {
            return BD.getQueryResult($@"select u.nombre as Nombre, concat_ws(' ', u.primer_apellido, u.segundo_apellido) as Apellidos, 

                                        u.email as Correo, d.nombre as Departamento,


                                        ifnull(sum(v.dias_disfrutados_vacaciones), 0) as Vacaciones from usuarios u left join vacaciones v 

                                        on u.id = v.id_usuario 
                                        left join departamentos d on d.id = u.departamento

                                        group by u.id");
        }
        //Metodo para filtrar empleados en la tabla
        public static List<Dictionary<string, object>> getEmpleadosFiltrados(int id)
        {
            return BD.getQueryResult($@"select u.nombre as Nombre, concat_ws(' ', u.primer_apellido, u.segundo_apellido) as Apellidos, u.email as Correo, u.departamento as departamento,
                ifnull(sum(v.dias_disfrutados_vacaciones), 0) as Vacaciones from usuarios u left join vacaciones v on u.id = v.id_usuario where u.id = ?id group by u.id", new Dictionary<string, object>() { { "id", id } });
        }
    }
}