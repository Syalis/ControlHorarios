using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webapp.Helpers;

namespace webapp.Data
{
    public class Vacaciones
    {

        /// Consulta para pintar los días de vacaciones que nos queda en el año presente
        public static List<Dictionary<string, object>> getDiasTotalVacaciones(Dictionary<string, object> item)
        {
            return BD.getQueryResult($@"select u.id, t.id_usuario, usuario, ifnull( t.total_vacaciones, 30) as total_vacaciones

                from usuarios u left join

                (SELECT v.id_usuario,  usuario,

                ifnull((30 -(sum(v.dias_disfrutados_vacaciones))), 30) as total_vacaciones  from vacaciones v 

                left join (select u.id , concat_ws(' ', u.nombre, u.primer_apellido, u.segundo_apellido) as usuario from usuarios u) u 

                on u.id = v.id_usuario

                group by v.id_usuario , usuario) t on u.id = t.id_usuario where u.id= ?id_usuario",

                      new Dictionary<string, object>() { { "id_usuario", item["id_usuario"] } });
        }

        /// Consulta para pintar los días de vacaciones en el año que estemos
        public static List<Dictionary<string, object>> getDiasTotalVacacionesAnio(Dictionary<string, object> item)
        {
            return BD.getQueryResult($@"select u.id, t.id_usuario, usuario, ifnull( t.total_vacaciones, 30) as total_vacaciones, anio_inicio

                from usuarios u left join

                (SELECT v.id_usuario,  usuario,  year(v.fecha_final_vacaciones) as anio_inicio,

                ifnull((30 -(sum(v.dias_disfrutados_vacaciones))), 30) as total_vacaciones  from vacaciones v 

                left join (select u.id , concat_ws(' ', u.nombre, u.primer_apellido, u.segundo_apellido) as usuario from usuarios u) u 

                on u.id = v.id_usuario

                group by v.id_usuario , usuario) t on u.id = t.id_usuario where u.id= ?id_usuario and anio_inicio = ?nYear",

                      new Dictionary<string, object>() { { "id_usuario", item["id_usuario"] }, { "nYear", item["nYear"] } });
        }

        /// Consulta para insertar un nuevo registro de vacaciones
        public static int create(Dictionary<string, object> data)
        {

            return BD.getInsertQueryResult("insert into vacaciones (id_usuario, fecha_inicio_vacaciones, fecha_final_vacaciones, dias_disfrutados_vacaciones) values (?id_usuario, ?fecha_inicio_vacaciones, ?fecha_final_vacaciones, (datediff(?fecha_final_vacaciones, ?fecha_inicio_vacaciones) + 1))", data);
        }

        /// Consulta para recoger los días de vacaciones para pintarlo en el calendario. 
        public static List<Dictionary<string, object>> getDiasCalendario(Dictionary<string, object> item)
        {
            return BD.getQueryResult($@"select a.anio, b.id, b.id_usuario, dia_inicio, mes_inicio, anio_inicio, dia_final, mes_final, 
                
                anio_final from (select DATE_FORMAT(now(), '%Y') as anio) a

                left join

                (SELECT v.id, v.id_usuario, day(v.fecha_inicio_vacaciones) as dia_inicio, 
                
                month(v.fecha_inicio_vacaciones) as mes_inicio, year (v.fecha_inicio_vacaciones) as anio_inicio, 

                day(v.fecha_final_vacaciones) as dia_final, 

                month(v.fecha_final_vacaciones) as mes_final, year (v.fecha_final_vacaciones) as anio_final

                from vacaciones v

                where v.id_usuario = ?id_usuario) b on a.anio = year(now())",

                      new Dictionary<string, object>() { { "id_usuario", item["id_usuario"] } });
        }

        /// Consulta para pasar sumar o restar al año actual. 
        public static List<Dictionary<string, object>> getYear(Dictionary<string, object> item)
        {
            return BD.getQueryResult($@"

                    select a.anio, b.id, b.id_usuario, dia_inicio, mes_inicio, anio_inicio, dia_final, mes_final, 
                
                    anio_final

                    from (select year(date_sub(now(), interval ?nYear year)) as anio) a

                    left join 

                    (SELECT v.id, ifnull(v.id_usuario, ?id_usuario) as id_usuario, 

                    year(date_sub(now(), interval ?nYear year)) as anio,

                    day(v.fecha_inicio_vacaciones) as dia_inicio, 

                    month(v.fecha_inicio_vacaciones) as mes_inicio, 
                
                    year(v.fecha_inicio_vacaciones) as anio_inicio, 

                    day(v.fecha_final_vacaciones) as dia_final, 

                    month(v.fecha_final_vacaciones) as mes_final, 

                    year (v.fecha_final_vacaciones) as anio_final

                    from vacaciones v 

                    where v.id_usuario = ?id_usuario ) b  on a.anio = year(date_sub(now(), interval ?nYear year))",

                    new Dictionary<string, object>() { { "id_usuario", item["id_usuario"] }, { "nYear", item["nYear"] } });
        }
    

        public static List<Dictionary<string, object>> envioEmail(Dictionary<string, object> email) {
            return BD.getQueryResult($@"

                    SELECT u.email, u. nombre as nombre_administrador, d.nombre as nombre_departamento, d.id as id_departamento from usuarios u 

                    left join (select d.id , d.nombre from departamentos d) d on 

                    d.id = u.departamento

                    where id_perfil = '2' and departamento  = ?departamento_nombre)",

                    new Dictionary<string, object>() { { "departamento_nombre", email["departamento_nombre"] } });
        }
    }
}
    
    

