using System;
using System.Data;
using webapp.Helpers;
using MySql.Data.MySqlClient;

namespace webapp.Helpers
{
    public static class GenericMethods
    {

        public static bool hasAccess(string idUsuario, int left, int right)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL()))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT id FROM usuarios WHERE lft>=?lft AND rgt<=?rgt AND id=?idUsuario", con))
                    {
                        cmd.Parameters.AddWithValue("?idUsuario", idUsuario);
                        cmd.Parameters.AddWithValue("?lft", left);
                        cmd.Parameters.AddWithValue("?rgt", right);
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }

                        if (dt.Rows.Count > 0)
                        {
                            return true;
                        }

                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}