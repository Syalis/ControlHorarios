using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webapp.Helpers;

namespace webapp.Data
{
    public class Vacaciones
    {
        /// <summary>
        /// Método para pintar los días de vacaciones que nos queda en el año presente
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> getDiasTotalVacaciones(Dictionary<string, object> item)
        {
            return BD.getQueryResult($@"select u.id, t.id_usuario, usuario, ifnull( t.total_vacaciones, 30) as total_vacaciones

                from usuarios u left join

                (SELECT v.id_usuario,  usuario,

                ifnull((30 -(sum(v.dias_disfrutados_vacaciones))), 30) as total_vacaciones  from vacaciones v 

                left join (select u.id , concat_ws(' ', u.nombre, u.primer_apellido, u.segundo_apellido) as usuario from usuarios u) u 

                on u.id = v.id_usuario

                group by v.id_usuario , usuario) t on u.id = t.id_usuario where u.id= ?id_usuario and year(curdate())" ,  

                      new Dictionary<string, object>() { { "id_usuario", item["id_usuario"] } });
        }

        /// <summary>
        /// Método para insertar un nuevo registro de vacaciones
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int create(Dictionary<string, object> data) {

            return BD.getInsertQueryResult("insert into vacaciones (id_usuario, fecha_inicio_vacaciones, fecha_final_vacaciones, dias_disfrutados_vacaciones) values (?id_usuario, ?fecha_inicio_vacaciones, ?fecha_final_vacaciones, (datediff(?fecha_final_vacaciones, ?fecha_inicio_vacaciones) + 1))", data);
        }

        /// <summary>
        /// Método para recoger los días de vacaciones para pintarlo en el calendario. 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> getDiasCalendario(Dictionary<string, object> item)
        {
            return BD.getQueryResult($@"SELECT v.id, v.id_usuario, day(v.fecha_inicio_vacaciones) as dia_inicio, 

                month(v.fecha_inicio_vacaciones) as mes_inicio, year (v.fecha_inicio_vacaciones) as anio_inicio, 

                day(v.fecha_final_vacaciones) as dia_final, 

                month(v.fecha_final_vacaciones) as mes_final, year (v.fecha_final_vacaciones) as anio_final

                from vacaciones v

                where v.id_usuario = ?id_usuario and year (curdate())",

                      new Dictionary<string, object>() { { "id_usuario", item["id_usuario"] } });
        }

    }

  

   
}