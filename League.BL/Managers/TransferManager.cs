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
    public class TransferManager
    {
        private ITransferRepository transferRepo;
        private ISpelerRepository spelerRepo;
        private ITeamRepository teamRepo;

        public TransferManager(ITransferRepository transferRepo, ISpelerRepository spelerRepo, ITeamRepository teamRepo)
        {
            this.transferRepo = transferRepo;
            this.spelerRepo = spelerRepo;
            this.teamRepo = teamRepo;
        }

        public Transfer RegistreerTransfer(SpelerInfo spelerInfo,TeamInfo nieuwTeamInfo,int prijs)
        {
            if (spelerInfo == null) throw new TransferManagerException("RegistreerTransfer - speler is null");
            Transfer transfer = null;
            try
            {
                Speler speler = spelerRepo.SelecteerSpeler(spelerInfo.id);
                //speler stopt
                if (nieuwTeamInfo == null)
                {
                    if (spelerInfo.teamNaam == null) throw new TransferManagerException("Registreertransfer - team is null");            
                    transfer = new Transfer(speler, speler.Team); //speler + oud team
                    speler.VerwijderTeam();
                }
                //nieuwe speler
                else if (spelerInfo.teamNaam==null)
                {
                    Team nieuwTeam=teamRepo.SelecteerTeam(nieuwTeamInfo.stamnummer);
                    speler.ZetTeam(nieuwTeam);
                    transfer=new Transfer(speler,nieuwTeam,prijs);
                }
                //klassieke transfer
                else
                {
                    Team nieuwTeam = teamRepo.SelecteerTeam(nieuwTeamInfo.stamnummer);
                    transfer =new Transfer(speler, nieuwTeam,speler.Team,prijs);
                    speler.ZetTeam(nieuwTeam);
                }
                return transferRepo.SchrijfTransferInDB(transfer);
            }
            catch(Exception ex)
            {
                throw new TransferManagerException("RegistreerTransfer", ex);
            }
        }
    }
}
