using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webapp.Helpers;

namespace webapp.Data
{
    public class Vacaciones
    {
        public static List<Dictionary<string, object>> getDiasVacaciones(Dictionary<string, object> item)
        {
            return BD.getQueryResult($@"SELECT v.id_usuario, usuario, date_format( v.fecha_inicio_vacaciones, '%d-%m-%Y') as fecha_inicio, 

                      date_format( v.fecha_final_vacaciones, '%d-%m-%Y') as fecha_final, (30 - sum(v.dias_disfrutados_vacaciones)) as dias_restantes  from vacaciones v 

                      left join (select u.id , concat_ws(' ', u.nombre, u.primer_apellido, u.segundo_apellido) as usuario from usuarios u) u 

                      on u.id = v.id_usuario where  v.id_usuario = ?id_usuario  and v.fecha_inicio_vacaciones = ?fecha_inicio_vacaciones 

                      and v.fecha_final_vacaciones = ?fecha_final_vacaciones 

                      group by v.id_usuario and v.fecha_inicio_vacaciones and v.fecha_final_vacaciones, usuario",  

                      new Dictionary<string, object>() { { "fecha_inicio_vacaciones", item["fecha_inicio_vacaciones"] },

                          { "fecha_final_vacaciones", item["fecha_final_vacaciones"] }, { "id_usuario", item["id_usuario"] } });
        }

        public static List<Dictionary<string, object>> getVacacionesRestantes(Dictionary<string, object> item)
        {
            return BD.getQueryResult($@"SELECT v.id_usuario, usuario, date_format( v.fecha_inicio_vacaciones, '%d-%m-%Y') as fecha_inicio, 

                      date_format( v.fecha_final_vacaciones, '%d-%m-%Y') as fecha_final, (30 - sum(v.dias_disfrutados_vacaciones)) as dias_restantes  from vacaciones v 

                      left join (select u.id  from usuarios u) u 

                      on u.id = v.id_usuario where  v.id_usuario = ?id_usuario  and v.fecha_inicio_vacaciones = ?fecha_inicio_vacaciones 

                      and v.fecha_final_vacaciones = ?fecha_final_vacaciones 

                      group by v.id_usuario and v.fecha_inicio_vacaciones and v.fecha_final_vacaciones, usuario",  

                      new Dictionary<string, object>() { { "id_usuario", item["id_usuario"] } });
        }

    }

  

   
}