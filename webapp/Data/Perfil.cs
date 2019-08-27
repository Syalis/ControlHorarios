using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webapp.Helpers;

namespace webapp.Data
{
    public class Perfil
    {
        //Metodo para actualizar nombre y apellidos del usuario en la base de datos
        public static int setNombre(Dictionary<string, object> nombreUsuario, int id)
        {
            return BD.getInsertQueryResult($@"update usuarios set nombre= CONCAT(UPPER(LEFT(?nombre, 1)), LOWER(SUBSTRING(?nombre, 2))), 
                                            primer_apellido=CONCAT(UPPER(LEFT(?primer_apellido, 1)), LOWER(SUBSTRING(?primer_apellido, 2))), 
                                            segundo_apellido= CONCAT(UPPER(LEFT(?segundo_apellido, 1)), LOWER(SUBSTRING(?segundo_apellido, 2)))
                                            where id = ?id", new Dictionary<string, object>() { {"id", id },{"nombre", nombreUsuario["nombre"] },{"primer_apellido", nombreUsuario["primer_apellido"] },{"segundo_apellido", nombreUsuario["segundo_apellido"] } });
        }
        
        //Metodo para actualizar el email
        public static int setEmail(Dictionary<string, object> emailUsuario, int id)
        {
            return BD.getInsertQueryResult($@"update usuarios set email =?email where id = ?id", 
                new Dictionary<string, object>() { { "id", id }, { "email", emailUsuario["email"] } });
        }
        //Metodo para actualizar el departamento
        public static int setDepartamento(Dictionary<string, object> departamentoUsuario, int id1)
        {
            return BD.getInsertQueryResult($@"update usuario set departamento = ?id where id=?id1 ", new Dictionary<string, object>() { { "id1", id1 }, { "id", departamentoUsuario["id"] } });
        }
    }
}