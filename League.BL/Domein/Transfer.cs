using League.BL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.Domein
{
    public class Transfer
    {
        public Transfer(int id, Speler speler, Team nieuwTeam, Team oudTeam, int prijs)
        {
            ZetId(id);
            ZetSpeler(speler);
            ZetNieuwTeam(nieuwTeam);
            ZetOudTeam(oudTeam);
            ZetPrijs(prijs);
        }
        // speler stopt
        public Transfer(int id, Speler speler, Team oudTeam)
        {
            ZetId(id);
            ZetSpeler(speler);
            ZetOudTeam(oudTeam);
            ZetPrijs(0);
        }
        //speler is nieuw
        public Transfer(int id, Speler speler, Team nieuwTeam, int prijs)
        {
            ZetId(id);
            ZetSpeler(speler);
            ZetNieuwTeam(nieuwTeam);
            ZetPrijs(prijs);
        }
        public int Id { get; private set; }
        public Speler Speler { get; private set; }
        public Team NieuwTeam { get; private set; }
        public Team OudTeam { get; private set; }
        public int Prijs { get; private set; }

        public void ZetId(int id)
        {
            if (id <= 0) throw new TransferException("ZetId");
            Id = id;
        }
        public void ZetPrijs(int prijs)
        {
            if (prijs < 0) throw new TransferException("ZetPrijs");
            Prijs = prijs;
        }
        public void VerwijderOudTeam()
        {
            if (NieuwTeam is null) throw new TransferException("VerwijderOudTeam"); //minstens 1 team
            OudTeam = null;
        }
        public void ZetOudTeam(Team team)
        {
            if (team == null) throw new TransferException("ZetOudTeam");
            if (team == NieuwTeam) throw new TransferException("ZetOudTeam");
            OudTeam = team;
        }
        public void VerwijderNieuwTeam()
        {
            if (OudTeam is null) throw new TransferException("VerwijderNieuwTeam"); //minstens 1 team
            NieuwTeam = null;
        }
        public void ZetNieuwTeam(Team team)
        {
            if (team == null) throw new TransferException("ZetNieuwTeam");
            if (team == OudTeam) throw new TransferException("ZetNieuwTeam");
            NieuwTeam = team;
        }
        public void ZetSpeler(Speler speler)
        {
            if (speler is null) throw new TransferException("ZetSpeler");
            Speler = speler;
        }
    }
}
