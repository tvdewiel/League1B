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
        public bool BestaatTeam(int stamnummer)
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
                    command.Parameters["@stamnummer"].Value = stamnummer;

                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true;
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
        public Team SelecteerTeam(int stamnummer)
        {
            SqlConnection connection = getConnection();
            string query = "SELECT t1.stamnummer,t1.naam as ploegnaam,t1.bijnaam,t2.* "
                + "FROM [dbo].[Team] t1 left join [dbo].[speler] t2 on t1.Stamnummer = t2.teamid "
                + "WHERE stamnummer=@stamnummer";
            using (SqlCommand command = connection.CreateCommand())
            {
                command.Parameters.Add(new SqlParameter("@stamnummer", SqlDbType.Int));
                command.CommandText = query;
                command.Parameters["@stamnummer"].Value = stamnummer;
                connection.Open();
                try
                {
                    Team team = null;
                    IDataReader reader = command.ExecuteReader(); //of SqlDataReader
                    while (reader.Read())
                    {
                        if (team == null)
                        {
                            string naam = (string)reader["ploegnaam"];
                            string bijnaam = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("bijnaam"))) bijnaam = (string)reader["bijnaam"];
                            team = new Team(stamnummer, naam);
                            if (bijnaam != null) team.ZetBijnaam(bijnaam);
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("id")))
                        {
                            int? lengte = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("lengte"))) lengte = (int?)reader["lengte"];
                            int? gewicht = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("gewicht"))) gewicht = (int?)reader["gewicht"];
                            Speler speler = new Speler((int)reader["id"], (string)reader["naam"], lengte, gewicht);
                            speler.ZetTeam(team);
                            if (!reader.IsDBNull(reader.GetOrdinal("rugnummer")))
                                speler.ZetRugnummer((int)reader["rugnummer"]);
                        }
                    }
                    reader.Close();
                    return team;
                }
                catch (Exception ex)
                {
                    throw new TeamRepoADOException("selecteerTeam", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void UpdateTeam(Team team)
        {
            SqlConnection connection = getConnection();
            string query = "UPDATE team SET naam=@naam, bijnaam=@bijnaam WHERE stamnummer=@stamnummer";
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@stamnummer", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@bijnaam", SqlDbType.NVarChar));
                    command.CommandText = query;
                    command.Parameters["@stamnummer"].Value = team.Stamnummer;
                    command.Parameters["@naam"].Value = team.Naam;
                    if (team.Bijnaam == null)
                        command.Parameters["@bijnaam"].Value = DBNull.Value;
                    else
                        command.Parameters["@bijnaam"].Value = team.Bijnaam;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new TeamRepoADOException("UpdateTeam", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
