using League.BL.Domein;
using League.BL.Interfaces;
using League.DL.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.DL
{
    public class TeamRepoADO : ITeamRepository
    {
        private string connectieString;

        public TeamRepoADO(string connectieString)
        {
            this.connectieString = connectieString;
        }
        private SqlConnection getConnection()
        {
            return new SqlConnection(connectieString);
        }
        public bool BestaatTeam(Team team)
        {
            SqlConnection connection = getConnection();
            string query = "SELECT count(*) FROM dbo.Team WHERE stamnummer=@stamnummer";

            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@stamnummer", SqlDbType.Int));                 
                    command.CommandText = query;
                    command.Parameters["@stamnummer"].Value = team.Stamnummer;
                   
                    int n=(int)command.ExecuteScalar();
                    if (n>0) return true;
                    else return false;
                }
                catch (Exception ex)
                {
                    throw new TeamRepoADOException("BestaatTeam", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void SchrijfTeamInDB(Team team)
        {
            SqlConnection connection = getConnection();
            string query = "INSERT INTO dbo.Team(stamnummer,naam,bijnaam) VALUES(@stamnummer,@naam,@bijnaam)";

            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@stamnummer", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@bijnaam", SqlDbType.NVarChar));

                    command.CommandText = query;
                    command.Parameters["@naam"].Value = team.Naam;
                    command.Parameters["@stamnummer"].Value = team.Stamnummer;
                    if (team.Bijnaam == null)
                        command.Parameters["@bijnaam"].Value = DBNull.Value;
                    else
                        command.Parameters["@bijnaam"].Value = team.Bijnaam;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new TeamRepoADOException("VoegTeamToe", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
