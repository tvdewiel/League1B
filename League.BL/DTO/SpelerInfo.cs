using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.DTO
{
    public class SpelerInfo
    {
        public SpelerInfo(int id, string naam, int? lengte, int? gewicht, int? rugnummer, string teamNaam)
        {
            this.id = id;
            this.naam = naam;
            this.lengte = lengte;
            this.gewicht = gewicht;
            this.rugnummer = rugnummer;
            this.teamNaam = teamNaam;
        }

        public int id { get; set; }
        public string naam { get; set; }
        public int? lengte { get; set; }
        public int? gewicht { get; set; }
        public int? rugnummer { get; set; }
        public string teamNaam { get; set; }
        public override string ToString()
        {
            return $"{naam},{id},{teamNaam}";
        }
    }
}
