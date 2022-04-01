using League.BL.Domein;
using League.BL.Interfaces;
using League.DL.Exceptions;
using System;
using System.Data;
using System.Data.SqlClient;

namespace League.DL
{
    public class SpelerRepoADO : ISpelerRepository
    {
        private string connectieString;

        public SpelerRepoADO(string connectieString)
        {
            this.connectieString = connectieString;
        }
        private SqlConnection getConnection()
        {
            return new SqlConnection(connectieString);
        }

        public bool BestaatSpeler(Speler s)
        {
            SqlConnection connection = getConnection();
            string query = "SELECT count(*) FROM dbo.Speler WHERE naam=@naam";

            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    command.CommandText = query;
                    command.Parameters["@naam"].Value = s.Naam;

                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true;
                    else return false;
                }
                catch (Exception ex)
                {
                    throw new TeamRepoADOException("BestaatSpeler", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public Speler SchrijfSpelerInDB(Speler s)
        {
            SqlConnection conn = getConnection();
            string query = "INSERT INTO dbo.Speler(naam,lengte,gewicht) "
                +"output INSERTED.ID VALUES(@naam,@lengte,@gewicht)";
            try
            {
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.Parameters.Add(new SqlParameter("@naam", System.Data.SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@lengte", System.Data.SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@gewicht", System.Data.SqlDbType.Int));
                    cmd.Parameters["@naam"].Value = s.Naam;
                    if (s.Lengte==null) cmd.Parameters["@lengte"].Value =DBNull.Value;
                    else cmd.Parameters["@lengte"].Value = s.Lengte;
                    if (s.Gewicht == null) cmd.Parameters["@gewicht"].Value = DBNull.Value;
                    else cmd.Parameters["@gewicht"].Value = s.Gewicht;
                    cmd.CommandText = query;
                    int newID=(int)cmd.ExecuteScalar();
                    s.ZetId(newID);
                    return s;
                }
            }
            catch (Exception ex)
            {
                throw new SpelerRepoADOException("SchrijfSpelerInDB", ex);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
