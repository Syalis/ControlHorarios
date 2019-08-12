using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webapp.Helpers;

namespace webapp.Data
{
    //Clase que contiene consultas para el registro de fichajes
    public class Fichajes
    {
        //Metodo para obtener los fichajes del mes en curso
        public static List<Dictionary<string, object>> getInicioFichajes(int id)
        {
            return BD.getQueryResult($@"select t.fecha, ifnull(a.id, 0) as id, ifnull(a.empleado,0) as empleado, ifnull(a.email, 0) as email, ifnull(a.computado, 0) as computado, ifnull(a.porcentaje, '0%') as porcentaje, 

                                            ifnull(a.mes, concat_ws(' ',
                                            monthname(str_to_date(date_format(timestamp(now()), '%m/%Y'), '%m')),
                                             date_format(timestamp(now()), '%Y'))) as mes, 

                                            ifnull(a.hora_entrada, 0) as hora_entrada, ifnull(a.hora_salida, 0) as hora_salida from

                                            (select 
                                                            DATE_FORMAT(m1, '%d/%m/%Y') as fecha
                                            from
                                            (
                                                            select 
                                                                           (now() - INTERVAL day((now()))-1 DAY) + INTERVAL m day as m1
                                                            from
                                                                           (
                                                                                           select @rownum:=@rownum+1 as m from
                                                                                           (select 1 union select 2 union select 3 union select 4) t1,
                                                                                           (select 1 union select 2 union select 3 union select 4) t2,
                                                                                           (select 1 union select 2 union select 3 union select 4) t3,
                                                                                           (select 1 union select 2 union select 3 union select 4) t4,
                                                                                           (select @rownum:=-1) t0
                                                                           ) d1
                                            ) d2 
                                            where 
                                                            m1<=last_day(now()) + Interval 1 day
                                            order by 
                                                            m1) t left join


                                            (SELECT u.id as id, u.email as email, concat_ws(' ',u.nombre, u.primer_apellido,u.segundo_apellido) as empleado, 

                                            time_format(timediff(ifnull(time(ch.hora_salida), '08:00:00'), time(ch.hora_entrada)), '%H:%i:%s') as computado, 

                                            concat(round((time_to_sec(time_format(timediff(ifnull(time(ch.hora_salida), '08:00:00'), time(ch.hora_entrada)), '%H:%i:%s'))*100/time_to_sec('08:00:00')),2),'%') as porcentaje


                                            ,time(ch.hora_entrada) as hora_entrada, 
                                            time(ch.hora_salida) as hora_salida, date_format(ch.fecha, '%d/%m/%Y') as fecha, concat_ws(' ',
                                            monthname(str_to_date(date_format(timestamp(now()), '%m/%Y'), '%m')),
                                             date_format(timestamp(now()), '%Y')) as mes

                                            FROM usuarios u right join control_horas ch on u.id = id_usuario

                                            where u.id = ?id
                                            ) a on a.fecha = t.fecha order by (t.fecha)asc", new Dictionary<string, object>() { {"id", id } });
        }
    }
}