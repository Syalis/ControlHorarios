using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webapp.Helpers;

namespace webapp.Data
{
    public class Departamentos
    {
        //Metodo para obtener todos los empleados con su departamento para representar en tabla
        public static List<Dictionary<string, object>> getDepartamentosTabla()
        {
            return BD.getQueryResult($@"select  d.id as id, d.nombre as Departamento, u.nombre as Nombre, concat_ws(' ', u.primer_apellido, u.segundo_apellido) as Apellidos, 
                u.email as Correo from usuarios u left join departamentos d on d.id = u.departamento");
        }
        //Metodo para obtener todos los departamentos para el dropdown
        public static List<Dictionary<string, object>> getDepartamentosTotal()
        {
            return BD.getQueryResult($@"select * from departamentos");
        }
        //Metodo para buscar empleados por departamento seleccionando del dropdown
        public static List<Dictionary<string, object>> getDepartamentoEmpleados(int id)
        {
            return BD.getQueryResult($@"select u.nombre as Nombre,  concat_ws(' ', u.primer_apellido, u.segundo_apellido) as Apellidos, 
                    u.email as Correo, ifnull(sum(dias_disfrutados_vacaciones), 0) as Vacaciones, upper(d.nombre) as Departamento
                    from usuarios u
                    left
                    join departamentos d
                    on u.departamento = d.id
                    left
                    join vacaciones v on u.id = v.id_usuario
                    where d.id = ?id
                    group by u.id", new Dictionary<string, object>() { { "id", id } });
        }
        //Metodo para filtrar empleados por departamento seleccionando del dropdown
        public static List<Dictionary<string, object>> getDepartamentoEmpleadosFiltro(int id)
        {
            return BD.getQueryResult($@"select upper(u.nombre) as Nombre,  upper(concat_ws(' ', u.primer_apellido, u.segundo_apellido)) as Apellidos, 
                    u.email as Correo, ifnull(sum(dias_disfrutados_vacaciones), 0) as Vacaciones, upper(d.nombre) as Departamento
                    from usuarios u
                    left
                    join departamentos d
                    on u.departamento = d.id
                    left
                    join vacaciones v on u.id = v.id_usuario
                    where u.id = ?id
                    group by u.id", new Dictionary<string, object>() { { "id", id } });
        }
    }
}