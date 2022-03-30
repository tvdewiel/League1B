using League.BL.Exceptions;
using System;

namespace League.BL.Domein
{
    public class Speler
    {
        internal Speler(int id, string naam,  int? lengte, int? gewicht) : this(naam,lengte,gewicht)
        {
            ZetId(id);
        }
        internal Speler(string naam, int? lengte, int? gewicht)
        {
            ZetNaam(naam);
            if (lengte != null) ZetLengte(lengte.Value);
            if (gewicht.HasValue) ZetGewicht(gewicht.Value);
        }

        public int Id { get; private set; }
        public string Naam { get; private set; }
        public Team Team { get; private set; }
        public int? Rugnummer { get; private set; }
        public int? Lengte { get; private set; }
        public int? Gewicht { get; private set; }

        public void ZetNaam(string naam)
        {
            if (string.IsNullOrWhiteSpace(naam)) throw new SpelerException("ZetNaam");
            Naam=naam.Trim();
        }
        public void ZetLengte(int lengte)
        {
            if (lengte<150)
            {
                throw new SpelerException("ZetLengte");
                //SpelerException ex = new SpelerException("ZetLengte");
                //ex.Data.Add("lengte", lengte);
                //throw ex;
            }
            Lengte = lengte;
        }
        public void ZetId(int id)
        {
            if (id <= 0) throw new SpelerException("ZetId");
            Id = id;
        }
        public void ZetGewicht(int gewicht)
        {
            if (gewicht < 50) throw new SpelerException("Zetgewicht");
            Gewicht=gewicht;
        }
        public void ZetRugnummer(int rugnummer)
        {
            if ((rugnummer <= 0) || (rugnummer > 99)) throw new SpelerException("ZetRugnummer");
            Rugnummer = rugnummer;
        }
        internal void VerwijderTeam()
        {
            //Team t = Team;
            //Team = null;
            //if (t!=null) t.VerwijderSpeler(this);
            if (Team.HeeftSpeler(this))
                Team.VerwijderSpeler(this);
            Team = null;
        }
        internal void ZetTeam(Team team)
        {
            if (team == null) throw new SpelerException("ZetTeam");
            if (team == Team) throw new SpelerException("ZetTeam");
            if (Team != null)
            {
                if (Team.HeeftSpeler(this))
                    Team.VerwijderSpeler(this);
            }
            if (!team.HeeftSpeler(this)) team.VoegSpelerToe(this);
            Team = team;
        }

        public override bool Equals(object obj)
        {
            return obj is Speler speler &&
                   Id == speler.Id;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
