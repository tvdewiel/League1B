using League.BL.Domein;
using League.BL.Exceptions;
using League.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.Managers
{
    public class TeamManager
    {
        private ITeamRepository repo;

        public TeamManager(ITeamRepository repo)
        {
            this.repo = repo;
        }
        public void RegistreerTeam(int stamnummer,string naam, string bijnaam)
        {
            try
            {
                Team t = new Team(stamnummer, naam);
                if (!string.IsNullOrWhiteSpace(bijnaam)) t.ZetBijnaam(bijnaam);
                if (!repo.BestaatTeam(t))
                {
                    repo.SchrijfTeamInDB(t);
                }
                else
                {
                    throw new TeamManagerException("RegistreerTeam - team bestaat al");
                }
            }
            catch (TeamManagerException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw new TeamManagerException("RegistreerTeam", ex);
            }
        }
    }
}
