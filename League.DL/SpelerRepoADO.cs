using League.BL.Domein;
using League.BL.DTO;
using League.BL.Interfaces;
using League.DL.Exceptions;
using System;
using System.Collections.Generic;
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
                    throw new SpelerRepoADOException("BestaatSpeler", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public bool BestaatSpeler(int spelerId)
        {
            SqlConnection connection = getConnection();
            string query = "SELECT count(*) FROM dbo.Speler WHERE id=@id";

            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    command.CommandText = query;
                    command.Parameters["@id"].Value = spelerId;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true;
                    else return false;
                }
                catch (Exception ex)
                {
                    throw new SpelerRepoADOException("BestaatSpeler", ex);
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
        public void UpdateSpeler(Speler speler)
        {
            SqlConnection connection = getConnection();
            string query = "UPDATE speler SET naam=@naam, rugnummer=@rugnummer,lengte=@lengte,"
                + "gewicht=@gewicht WHERE id=@id";

            connection.Open();
            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@lengte", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@gewicht", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@rugnummer", SqlDbType.Int));
                    command.CommandText = query;
                    command.Parameters["@id"].Value = speler.Id;
                    command.Parameters["@naam"].Value = speler.Naam;
                    if (speler.Lengte == null)
                        command.Parameters["@lengte"].Value = DBNull.Value;
                    else
                        command.Parameters["@lengte"].Value = speler.Lengte;
                    if (speler.Gewicht == null)
                        command.Parameters["@gewicht"].Value = DBNull.Value;
                    else
                        command.Parameters["@gewicht"].Value = speler.Gewicht;
                    if (speler.Rugnummer == null)
                        command.Parameters["@rugnummer"].Value = DBNull.Value;
                    else
                        command.Parameters["@rugnummer"].Value = speler.Rugnummer;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new SpelerRepoADOException("UpdateSpeler", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public IReadOnlyList<SpelerInfo> SelecteerSpelers(int? id, string naam)
        {
            if ((!id.HasValue) && (string.IsNullOrEmpty(naam) == true))
                throw new SpelerRepoADOException("SelecteerSpelers - no valid input");
            string query = "SELECT Id,t1.Naam,Rugnummer,Lengte,Gewicht, "
                            + " case when t2.Stamnummer is null then null "
                            + "      else concat(t2.Naam, ' (', t2.Bijnaam, ') - ', t2.Stamnummer) "
                            + " end teamnaam "
                            + " FROM Speler t1 "
                            + " left join team t2 on t1.teamid = t2.Stamnummer";
            if (id.HasValue) query += " WHERE t1.id=@id";
            else query += " WHERE t1.naam=@naam";
            List<SpelerInfo> spelers = new List<SpelerInfo>();
            SqlConnection connection = getConnection();
            using(SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                connection.Open();
                try
                {
                    if (id.HasValue)
                    {
                        command.Parameters.AddWithValue("@id", id);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@naam",naam);
                    }
                    IDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int idspeler = (int) reader["id"];
                        string naamspeler = (string)reader["naam"];
                        int? lengte=null;
                        if (!reader.IsDBNull(reader.GetOrdinal("lengte"))) lengte = (int?)reader["lengte"];
                        int? gewicht = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("gewicht"))) gewicht = (int?)reader["gewicht"];
                        int? rugnummer = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("rugnummer"))) rugnummer = (int?)reader["rugnummer"];
                        string teamnaam= null;
                        if (!reader.IsDBNull(reader.GetOrdinal("teamnaam"))) teamnaam = (string)reader["teamnaam"];
                        SpelerInfo spelerInfo = new SpelerInfo(idspeler, naamspeler, lengte, gewicht, rugnummer, teamnaam);
                        spelers.Add(spelerInfo);
                    }
                    return spelers.AsReadOnly();
                }
                catch(Exception ex)
                {
                    throw new SpelerRepoADOException("SelecteerSpelers",ex);
                }
                finally { connection.Close(); }
            }
        }
        public Speler SelecteerSpeler(int id)
        {
            string query = "select t1.id spelerid,t1.naam spelernaam,t1.Rugnummer spelerrugnummer, "
                +"t1.Lengte spelerlengte, t1.Gewicht spelergewicht, stamnummer, "
                +"t2.naam ploegnaam, bijnaam, t3.* "
                +"from speler t1 "
                +"left join team t2 on t1.teamid = t2.Stamnummer "
                +"left join speler t3 on t2.Stamnummer = t3.TeamId "
                +"where t1.id = @id";
            SqlConnection connection = getConnection();
            Speler speler = null;
            Team team = null; 
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                try
                {
                    IDataReader reader = command.ExecuteReader();
                    bool heeftTeam = false;
                    while (reader.Read())
                    {
                        if (speler == null)
                        {
                            int? lengte = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("spelerlengte"))) lengte = (int?)reader["spelerlengte"];
                            int? gewicht = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("spelergewicht"))) gewicht = (int?)reader["spelergewicht"];
                            int? rugnummer = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("spelerrugnummer"))) rugnummer = (int?)reader["spelerrugnummer"];
                            speler = new Speler(id, (string)reader["spelernaam"], lengte, gewicht);
                            if (rugnummer.HasValue) speler.ZetRugnummer((int)rugnummer);
                            heeftTeam = !reader.IsDBNull(reader.GetOrdinal("stamnummer"));
                        }
                        if (heeftTeam) {
                            if (team == null)
                            {
                                int stamnummer = (int)reader["stamnummer"];
                                string teamnaam = (string)reader["ploegnaam"];
                                string bijnaam = null;
                                if (!reader.IsDBNull(reader.GetOrdinal("bijnaam"))) bijnaam = (string)reader["bijnaam"];
                                team = new Team(stamnummer, teamnaam);
                                if (bijnaam!=null) team.ZetBijnaam(bijnaam);
                                speler.ZetTeam(team);
                            }
                            int? lengte = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("lengte"))) lengte = (int?)reader["lengte"];
                            int? gewicht = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("gewicht"))) gewicht = (int?)reader["gewicht"];
                            int? rugnummer = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("rugnummer"))) rugnummer = (int?)reader["rugnummer"];
                            Speler teamSpeler = new Speler((int)reader["id"], (string)reader["naam"], lengte, gewicht);
                            if (rugnummer.HasValue) teamSpeler.ZetRugnummer((int)rugnummer);
                            teamSpeler.ZetTeam(team);
                        }
                    }
                    reader.Close();
                    
                    return speler;
                }
                catch (Exception ex)
                {
                    throw new SpelerRepoADOException("SelecteerSpelers", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
