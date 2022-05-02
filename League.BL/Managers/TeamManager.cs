using League.BL.Domein;
using League.BL.DTO;
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
                if (!repo.BestaatTeam(stamnummer))
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
        public Team SelecteerTeam(int stamnummer)
        {
            try
            {
                if (repo.BestaatTeam(stamnummer))
                {
                    return repo.SelecteerTeam(stamnummer);
                }
                else
                {
                    throw new TeamManagerException("SelecteerTeam");
                }
            }
            catch(Exception ex)
            {
                throw new TeamManagerException("SelecteerTeam", ex);
            }
        }
        public void UpdateTeam(Team team)
        {
            if (team == null) throw new TeamManagerException("updateteam - team is null");
            try
            {
                if (repo.BestaatTeam(team.Stamnummer))
                {
                    repo.UpdateTeam(team);
                }
                else
                {
                    throw new TeamManagerException("updateteam - team niet gevonden");
                }
            }
            catch (TeamManagerException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new TeamManagerException("updateteam", ex);
            }
        }
        public IReadOnlyList<TeamInfo> SelecteerTeams()
        {
            try
            {
                return repo.SelecteerTeams();
            }
            catch (Exception ex)
            {
                throw new TeamManagerException("SelecteerTeams");
            }
        }
    }
}
