﻿using League.BL.Exceptions;
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

        internal Team(int stamnummer, string naam)
        {
            ZetStamnummer(stamnummer);
            ZetNaam(naam);
        }
        public IReadOnlyList<Speler> Spelers()
        {
            return _spelers.AsReadOnly();
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
        public void VerwijderBijnaam()
        {
            Bijnaam = null;
        }
        internal void VerwijderSpeler(Speler speler)
        {
            if (speler == null) throw new TeamException("Verwijderspeler");
            if (!_spelers.Contains(speler))
            {
                throw new TeamException("Verwijderspeler");
            }
            _spelers.Remove(speler);
            if (speler.Team==this)
                speler.VerwijderTeam();            
        }
        internal void VoegSpelerToe(Speler speler)
        {
            if (speler == null) throw new TeamException("VoegspelerToe");
            if (_spelers.Contains(speler)) throw new TeamException("VoegspelerToe");
            _spelers.Add(speler);
            if (speler.Team!=this)
                speler.ZetTeam(this);
        }
        public bool HeeftSpeler(Speler speler)
        {
            return _spelers.Contains(speler);
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
