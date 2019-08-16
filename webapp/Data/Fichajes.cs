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
        //Metodo para obtener los fichajes del mes en curso por pares de entrada y salida
        public static List<Dictionary<string, object>> getMesFichajes(int id)
        {
            return BD.getQueryResult($@"select t.fecha, ifnull(a.id, ?id) as id, ifnull(a.empleado,0) as empleado, ifnull(a.email, 0) as email, 

                                    ifnull(a.mes, concat_ws(' ',
                                    monthname(str_to_date(date_format(CURDATE(), '%m/%Y'), '%m')),
                                     date_format(CURDATE(), '%Y'))) as mes, 

                                    group_concat(concat_ws(',',(ifnull(a.hora_entrada, 0)), ifnull(a.hora_salida, 0))) as horas from

                                    (select 
                                                    DATE_FORMAT(m1, '%d/%m/%Y') as fecha
                                    from
                                    (
                                                    select 
                                                                   (CURDATE() - INTERVAL day(CURDATE())-1 DAY) + INTERVAL m day as m1
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
                                                                                            m1<=last_day(CURDATE())
                                                                            order by 
                                                                                            m1) t left join


                                        (SELECT u.id as id, u.email as email, concat_ws(' ',u.nombre, u.primer_apellido,u.segundo_apellido) as empleado, 

                                        time(ch.hora_entrada) as hora_entrada, 
                                        time(ch.hora_salida) as hora_salida, date_format(ch.fecha, '%d/%m/%Y') as fecha, concat_ws(' ',
                                        monthname(str_to_date(date_format(CURDATE(), '%m/%Y'), '%m')),
                                         date_format(CURDATE(), '%Y')) as mes

                                        FROM usuarios u right join control_horas ch on u.id = id_usuario

                                            where u.id = ?id
                                            ) a on a.fecha = t.fecha  group by t.fecha order by (t.fecha)asc", new Dictionary<string, object>() { { "id", id } });
        }
        //Metodo para comprobar total de horas y porcentajes totales de jornada laboral
        public static List<Dictionary<string, object>> getTotalesFichajes(int id)
        {
            return BD.getQueryResult($@"select t.fecha, ifnull(h.porcentaje, '0%') as porcentaje, ifnull(h.id_usuario, ?id)as id, 

                                        time_format(ifnull(h.computado, 0), '%H:%i:%s') as computado from

                                        (select
                                                        DATE_FORMAT(m1, '%d/%m/%Y') as fecha
                                        from
                                        (
                                                        select
                                                                       (curdate() - INTERVAL day(curdate()) - 1 DAY) + INTERVAL m day as m1
                                                        from
                                                                       (
                                                                                       select @rownum:= @rownum + 1 as m from
                                                                                       (select 1 union select 2 union select 3 union select 4) t1,
                                                                                       (select 1 union select 2 union select 3 union select 4) t2,
                                                                                       (select 1 union select 2 union select 3 union select 4) t3,
                                                                                       (select 1 union select 2 union select 3 union select 4) t4,
                                                                                       (select @rownum:= -1) t0
                                                                       ) d1
                                        ) d2
                                        where
                                                        m1 <= last_day(curdate())
                                          order by
                                                          m1) t left join

                                          (select id_usuario, concat(round(sum((timediff(time(hora_salida), time(hora_entrada))) * 100 / time_to_sec('08:00:00')), 2), '%') as porcentaje,



                                          time_format(timediff(ifnull(time(hora_salida), '08:00:00'), time(hora_entrada)), '%H:%i:%s') as computado,

                                          date_format(fecha, '%d/%m/%Y') as fecha
  


                                          from control_horas group by date_format(fecha, '%d/%m/%Y')) h on t.fecha = h.fecha  and h.id_usuario = ?id order by(t.fecha)asc", new Dictionary<string, object>() { { "id", id } });
        }
        //Metodo de comprobacion de boton de fichajes
        public static List<Dictionary<string, object>> getEstadoBoton(int id)
        {
            return BD.getQueryResult($@"SELECT date_format(now(), '%d/%m/%Y') as hoy, time_format(hora_entrada, '%H:%i:%s')as hora_entrada, 

                                            time_format(hora_salida, '%H:%i:%s') as hora_salida 


                                            FROM control_horas where id_usuario = ?id and date_format(fecha, '%d/%m/%Y') = date_format(now(), '%d/%m/%Y') order by (fecha)asc", new Dictionary<string, object>() { { "id", id } });
        }
        //Metodo para crear un checkIn en base de datos
        public static int CheckIn(Dictionary<string, object> checkIn)
        {
            return BD.getInsertQueryResult($"INSERT into control_horas(id_usuario, hora_entrada, fecha) values(?id_usuario, now(), now())", checkIn);
        }

        //Metodo para hacer checkOut en base de datos en una row ya creada
        public static int CheckOut(Dictionary<string, object> checkOut)
        {
            return BD.getNonQueryResult($"update control_horas set hora_salida= now() where id_usuario= ?id_usuario and hora_salida is null", checkOut);
        }
        //Metodo para obtener fichajes de los meses inferiores
        //public static List<Dictionary<string, object>> mesResta(int id, int nMes)
        //{
        //    return BD.getQueryResult($@"select t.fecha, ifnull(a.id, ?id) as id, ifnull(a.empleado,0) as empleado, ifnull(a.email, 0) as email, 

        //                            ifnull(a.mes, concat_ws(' ',
        //                            monthname(str_to_date(date_format(date_sub(curdate(), interval ?nMes month), '%m/%Y'), '%m')),
        //                             date_format(date_sub(curdate(), interval ?nMes month), '%Y'))) as mes, 

        //                            group_concat(concat_ws(',',(ifnull(a.hora_entrada, 0)), ifnull(a.hora_salida, 0))) as horas from

        //                            (select 
        //                                            DATE_FORMAT(m1, '%d/%m/%Y') as fecha
        //                            from
        //                            (
        //                                            select 
        //                                                           (date_sub(curdate(), interval ?nMes month) - INTERVAL day(date_sub(curdate(), interval ?nMes month))-1 DAY) + INTERVAL m day as m1
        //                                            from
        //                                                           (
        //                                                                           select @rownum:=@rownum+1 as m from
        //                                                                           (select 1 union select 2 union select 3 union select 4) t1,
        //                                                                           (select 1 union select 2 union select 3 union select 4) t2,
        //                                                                           (select 1 union select 2 union select 3 union select 4) t3,
        //                                                                           (select 1 union select 2 union select 3 union select 4) t4,
        //                                                                           (select @rownum:=-1) t0
        //                                                                                                   ) d1
        //                                                                    ) d2 
        //                                                                    where 
        //                                                                                    m1<=last_day(date_sub(curdate(), interval ?nMes month))
        //                                                                    order by 
        //                                                                                    m1) t left join


        //                                (SELECT u.id as id, u.email as email, concat_ws(' ',u.nombre, u.primer_apellido,u.segundo_apellido) as empleado, 

        //                                time(ch.hora_entrada) as hora_entrada, 
        //                                time(ch.hora_salida) as hora_salida, date_format(ch.fecha, '%d/%m/%Y') as fecha, concat_ws(' ',
        //                                monthname(str_to_date(date_format(date_sub(curdate(), interval ?nMes month), '%m/%Y'), '%m')),
        //                                 date_format(date_sub(curdate(), interval ?nMes month), '%Y')) as mes

        //                                FROM usuarios u right join control_horas ch on u.id = id_usuario

        //                                    where u.id = ?id
        //                                    ) a on a.fecha = t.fecha  group by t.fecha order by (t.fecha)asc", new Dictionary<string, object>() { { "id", id }, { "nMes", nMes } });
        //}
        ////Metodo para obtener porcentaje y totales de mes en resta
        //public static List<Dictionary<string, object>> getTotalesFichajesResta(int id, int nMes)
        //{
        //    return BD.getQueryResult($@"select t.fecha, ifnull(h.porcentaje, '0%') as porcentaje, ifnull(h.id_usuario, ?id)as id, 

        //                                time_format(ifnull(h.computado, 0), '%H:%i:%s') as computado from

        //                                (select
        //                                                DATE_FORMAT(m1, '%d/%m/%Y') as fecha
        //                                from
        //                                (
        //                                                select
        //                                                               (date_sub(curdate(), interval ?nMes month) - INTERVAL day(date_sub(curdate(), interval ?nMes month)) - 1 DAY) + INTERVAL m day as m1
        //                                                from
        //                                                               (
        //                                                                               select @rownum:= @rownum + 1 as m from
        //                                                                               (select 1 union select 2 union select 3 union select 4) t1,
        //                                                                               (select 1 union select 2 union select 3 union select 4) t2,
        //                                                                               (select 1 union select 2 union select 3 union select 4) t3,
        //                                                                               (select 1 union select 2 union select 3 union select 4) t4,
        //                                                                               (select @rownum:= -1) t0
        //                                                               ) d1
        //                                ) d2
        //                                where
        //                                                m1 <= last_day(date_sub(curdate(), interval ?nMes month))
        //                                  order by
        //                                                  m1) t left join

        //                                  (select id_usuario, concat(round(sum((timediff(time(hora_salida), time(hora_entrada))) * 100 / time_to_sec('08:00:00')), 2), '%') as porcentaje,



        //                                  time_format(timediff(ifnull(time(hora_salida), '08:00:00'), time(hora_entrada)), '%H:%i:%s') as computado,

        //                                  date_format(fecha, '%d/%m/%Y') as fecha



        //                                  from control_horas group by date_format(fecha, '%d/%m/%Y')) h on t.fecha = h.fecha  and h.id_usuario = ?id order by(t.fecha)asc", new Dictionary<string, object>() { { "id", id }, { "nMes", nMes } });
        //}
        //Metodo para obtener fichajes de los meses superiores
        //public static List<Dictionary<string, object>> mesSuma(int id, int nMes)
        //{
        //    return BD.getQueryResult($@"select t.fecha, ifnull(a.id, ?id) as id, ifnull(a.empleado,0) as empleado, ifnull(a.email, 0) as email, 

        //                            ifnull(a.mes, concat_ws(' ',
        //                            monthname(str_to_date(date_format(date_add(curdate(), interval ?nMes month), '%m/%Y'), '%m')),
        //                             date_format(DATE_ADD(curdate(), interval ?nMes month), '%Y'))) as mes, 

        //                            group_concat(concat_ws(',',(ifnull(a.hora_entrada, 0)), ifnull(a.hora_salida, 0))) as horas from

        //                            (select 
        //                                            DATE_FORMAT(m1, '%d/%m/%Y') as fecha
        //                            from
        //                            (
        //                                            select 
        //                                                           (DATE_ADD(curdate(), interval ?nMes month) - INTERVAL day(DATE_ADD(curdate(), interval ?nMes month))-1 DAY) + INTERVAL m day as m1
        //                                            from
        //                                                           (
        //                                                                           select @rownum:=@rownum+1 as m from
        //                                                                           (select 1 union select 2 union select 3 union select 4) t1,
        //                                                                           (select 1 union select 2 union select 3 union select 4) t2,
        //                                                                           (select 1 union select 2 union select 3 union select 4) t3,
        //                                                                           (select 1 union select 2 union select 3 union select 4) t4,
        //                                                                           (select @rownum:=-1) t0
        //                                                                                                   ) d1
        //                                                                    ) d2 
        //                                                                    where 
        //                                                                                    m1<=last_day(DATE_ADD(curdate(), interval ?nMes month))
        //                                                                    order by 
        //                                                                                    m1) t left join


        //                                (SELECT u.id as id, u.email as email, concat_ws(' ',u.nombre, u.primer_apellido,u.segundo_apellido) as empleado, 

        //                                time(ch.hora_entrada) as hora_entrada, 
        //                                time(ch.hora_salida) as hora_salida, date_format(ch.fecha, '%d/%m/%Y') as fecha, concat_ws(' ',
        //                                monthname(str_to_date(date_format(DATE_ADD(curdate(), interval ?nMes month), '%m/%Y'), '%m')),
        //                                 date_format(DATE_ADD(curdate(), interval ?nMes month), '%Y')) as mes

        //                                FROM usuarios u right join control_horas ch on u.id = id_usuario

        //                                    where u.id = ?id
        //                                    ) a on a.fecha = t.fecha  group by t.fecha order by (t.fecha)asc", new Dictionary<string, object>() { { "id", id }, { "nMes", nMes } });
        //}
        ////Metodo para obtener porcentaje y totales de mes en suma
        //public static List<Dictionary<string, object>> getTotalesFichajesSuma(int id, int nMes)
        //{
        //    return BD.getQueryResult($@"select t.fecha, ifnull(h.porcentaje, '0%') as porcentaje, ifnull(h.id_usuario, ?id)as id, 

        //                                time_format(ifnull(h.computado, 0), '%H:%i:%s') as computado from

        //                                (select
        //                                                DATE_FORMAT(m1, '%d/%m/%Y') as fecha
        //                                from
        //                                (
        //                                                select
        //                                                               (DATE_ADD(curdate(), interval ?nMes month) - INTERVAL day(DATE_ADD(curdate(), interval ?nMes month)) - 1 DAY) + INTERVAL m day as m1
        //                                                from
        //                                                               (
        //                                                                               select @rownum:= @rownum + 1 as m from
        //                                                                               (select 1 union select 2 union select 3 union select 4) t1,
        //                                                                               (select 1 union select 2 union select 3 union select 4) t2,
        //                                                                               (select 1 union select 2 union select 3 union select 4) t3,
        //                                                                               (select 1 union select 2 union select 3 union select 4) t4,
        //                                                                               (select @rownum:= -1) t0
        //                                                               ) d1
        //                                ) d2
        //                                where
        //                                                m1 <= last_day(DATE_ADD(curdate(), interval ?nMes month))
        //                                  order by
        //                                                  m1) t left join

        //                                  (select id_usuario, concat(round(sum((timediff(time(hora_salida), time(hora_entrada))) * 100 / time_to_sec('08:00:00')), 2), '%') as porcentaje,



        //                                  time_format(timediff(ifnull(time(hora_salida), '08:00:00'), time(hora_entrada)), '%H:%i:%s') as computado,

        //                                  date_format(fecha, '%d/%m/%Y') as fecha
  


        //                                  from control_horas group by date_format(fecha, '%d/%m/%Y')) h on t.fecha = h.fecha  and h.id_usuario = ?id order by(t.fecha)asc", new Dictionary<string, object>() { { "id", id }, { "nMes", nMes } });
        //}

        public static List<Dictionary<string, object>> mesResta(int id, int nMes)
        {
            return BD.getQueryResult($@"select t.fecha, ifnull(a.id, ?id) as id, ifnull(a.empleado,0) as empleado, ifnull(a.email, 0) as email, 

                                    ifnull(a.mes, concat_ws(' ',
                                    monthname(str_to_date(date_format(date_sub(curdate(), interval ?nMes month), '%m/%Y'), '%m')),
                                     date_format(date_sub(curdate(), interval ?nMes month), '%Y'))) as mes, 

                                    group_concat(concat_ws(',',(ifnull(a.hora_entrada, 0)), ifnull(a.hora_salida, 0))) as horas from

                                    (select 
                                                    DATE_FORMAT(m1, '%d/%m/%Y') as fecha
                                    from
                                    (
                                                    select 
                                                                   (date_sub(curdate(), interval ?nMes month) - INTERVAL day(date_sub(curdate(), interval ?nMes month))-1 DAY) + INTERVAL m day as m1
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
                                                                                            m1<=last_day(date_sub(curdate(), interval ?nMes month))
                                                                            order by 
                                                                                            m1) t left join


                                        (SELECT u.id as id, u.email as email, concat_ws(' ',u.nombre, u.primer_apellido,u.segundo_apellido) as empleado, 

                                        time(ch.hora_entrada) as hora_entrada, 
                                        time(ch.hora_salida) as hora_salida, date_format(ch.fecha, '%d/%m/%Y') as fecha, concat_ws(' ',
                                        monthname(str_to_date(date_format(date_sub(curdate(), interval ?nMes month), '%m/%Y'), '%m')),
                                         date_format(date_sub(curdate(), interval ?nMes month), '%Y')) as mes

                                        FROM usuarios u right join control_horas ch on u.id = id_usuario

                                            where u.id = ?id
                                            ) a on a.fecha = t.fecha  group by t.fecha order by (t.fecha)asc", new Dictionary<string, object>() { { "id", id }, { "nMes", nMes } });
        }
        //Metodo para obtener porcentaje y totales de mes en resta
        public static List<Dictionary<string, object>> getTotalesFichajesResta(int id, int nMes)
        {
            return BD.getQueryResult($@"select t.fecha, ifnull(h.porcentaje, '0%') as porcentaje, ifnull(h.id_usuario, ?id)as id, 

                                        time_format(ifnull(h.computado, 0), '%H:%i:%s') as computado from

                                        (select
                                                        DATE_FORMAT(m1, '%d/%m/%Y') as fecha
                                        from
                                        (
                                                        select
                                                                       (date_sub(curdate(), interval ?nMes month) - INTERVAL day(date_sub(curdate(), interval ?nMes month)) - 1 DAY) + INTERVAL m day as m1
                                                        from
                                                                       (
                                                                                       select @rownum:= @rownum + 1 as m from
                                                                                       (select 1 union select 2 union select 3 union select 4) t1,
                                                                                       (select 1 union select 2 union select 3 union select 4) t2,
                                                                                       (select 1 union select 2 union select 3 union select 4) t3,
                                                                                       (select 1 union select 2 union select 3 union select 4) t4,
                                                                                       (select @rownum:= -1) t0
                                                                       ) d1
                                        ) d2
                                        where
                                                        m1 <= last_day(date_sub(curdate(), interval ?nMes month))
                                          order by
                                                          m1) t left join

                                          (select id_usuario, concat(round(sum((timediff(time(hora_salida), time(hora_entrada))) * 100 / time_to_sec('08:00:00')), 2), '%') as porcentaje,



                                          time_format(timediff(ifnull(time(hora_salida), '08:00:00'), time(hora_entrada)), '%H:%i:%s') as computado,

                                          date_format(fecha, '%d/%m/%Y') as fecha
  


                                          from control_horas group by date_format(fecha, '%d/%m/%Y')) h on t.fecha = h.fecha  and h.id_usuario = ?id order by(t.fecha)asc", new Dictionary<string, object>() { { "id", id }, { "nMes", nMes } });
        }
    }
}