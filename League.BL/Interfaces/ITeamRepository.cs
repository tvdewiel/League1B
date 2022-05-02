using League.BL.Domein;
using League.BL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.Interfaces
{
    public interface ITeamRepository
    {
        void SchrijfTeamInDB(Team t);
        bool BestaatTeam(int stamnummer);
        Team SelecteerTeam(int stamnummer);
        void UpdateTeam(Team team);
        IReadOnlyList<TeamInfo> SelecteerTeams();
    }
}
