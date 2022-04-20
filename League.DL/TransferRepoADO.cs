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
    public class TransferRepoADO : ITransferRepository
    {
        private string connectieString;

        public TransferRepoADO(string connectieString)
        {
            this.connectieString = connectieString;
        }
        private SqlConnection getConnection()
        {
            return new SqlConnection(connectieString);
        }
        public Transfer SchrijfTransferInDB(Transfer transfer)
        {
            SqlConnection conn = getConnection();
            string querySpeler = "UPDATE speler SET teamid=@teamid WHERE id=@id";
            string queryTransfer = "INSERT INTO transfer(spelerid,prijs,oudteamid,nieuwteamid) "
                +" output INSERTED.ID VALUES(@spelerid,@prijs,@oudteamid,@nieuwteamid) ";

            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            using (SqlCommand cmdSpeler = conn.CreateCommand())
            using (SqlCommand cmdTransfer = conn.CreateCommand())
            {
                cmdSpeler.Transaction = tran;
                cmdTransfer.Transaction = tran;
                try
                {
                    //transfer
                    cmdTransfer.Parameters.Add(new SqlParameter("@spelerid", SqlDbType.Int));
                    cmdTransfer.Parameters.Add(new SqlParameter("@prijs", SqlDbType.Int));
                    cmdTransfer.Parameters.Add(new SqlParameter("@oudteamid", SqlDbType.Int));
                    cmdTransfer.Parameters.Add(new SqlParameter("@nieuwteamid", SqlDbType.Int));
                    cmdTransfer.CommandText = queryTransfer;
                    cmdTransfer.Parameters["@spelerid"].Value = transfer.Speler.Id;
                    cmdTransfer.Parameters["@prijs"].Value = transfer.Prijs;
                    if (transfer.OudTeam != null)
                        cmdTransfer.Parameters["@oudteamid"].Value = transfer.OudTeam.Stamnummer;
                    else
                        cmdTransfer.Parameters["@oudteamid"].Value = DBNull.Value;
                    if (transfer.NieuwTeam != null)
                        cmdTransfer.Parameters["@nieuwteamid"].Value = transfer.NieuwTeam.Stamnummer;
                    else
                        cmdTransfer.Parameters["@nieuwteamid"].Value = DBNull.Value;
                    int transferid = (int)cmdTransfer.ExecuteScalar();
                    transfer.ZetId(transferid);
                    //speler
                    cmdSpeler.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    cmdSpeler.Parameters.Add(new SqlParameter("@teamid", SqlDbType.Int));
                    cmdSpeler.CommandText = querySpeler;
                    cmdSpeler.Parameters["@id"].Value = transfer.Speler.Id;
                    if (transfer.NieuwTeam == null)
                        cmdSpeler.Parameters["@teamid"].Value = DBNull.Value;
                    else
                        cmdSpeler.Parameters["@teamid"].Value = transfer.NieuwTeam.Stamnummer;
                    cmdSpeler.ExecuteNonQuery();
                    tran.Commit();
                    return transfer;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new TransferRepoADOException("SchrijfTransferInDB", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
