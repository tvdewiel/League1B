using League.BL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.Domein
{
    public class Team
    {
        public int Stamnummer { get; private set; }
        public string Naam { get; private set; }
        public string Bijnaam { get; private set; }
        private List<Speler> _spelers = new List<Speler>();

        public Team(int stamnummer, string naam)
        {
            ZetStamnummer(stamnummer);
            ZetNaam(naam);
        }

        public void ZetStamnummer(int stamnummer)
        {
            if (stamnummer <= 0) throw new TeamException("ZetStamnummer");
            Stamnummer = stamnummer;
        }
        public void ZetNaam(string naam)
        {
            if (string.IsNullOrWhiteSpace(naam)) throw new TeamException("ZetNaam");
            Naam = naam.Trim();
        }
        public void ZetBijnaam(string bijnaam)
        {
            if (string.IsNullOrWhiteSpace(bijnaam)) throw new TeamException("ZetBijnaam");
            Bijnaam = bijnaam.Trim();
        }
        public void VerwijderSpeler(Speler speler)
        {
            if (speler == null) throw new TeamException("Verwijderspeler");
            if (!_spelers.Contains(speler))
            {
                throw new TeamException("Verwijderspeler");
            }
            speler.VerwijderTeam();
            _spelers.Remove(speler); 
        }
        public void VoegSpelerToe(Speler speler)
        {
            if (speler == null) throw new TeamException("VoegspelerToe");
            if (_spelers.Contains(speler)) throw new TeamException("VoegspelerToe");
            speler.ZetTeam(this);
            _spelers.Add(speler);
        }

        public override bool Equals(object obj)
        {
            return obj is Team team &&
                   Stamnummer == team.Stamnummer;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Stamnummer);
        }
    }
}
