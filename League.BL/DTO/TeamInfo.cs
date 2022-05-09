using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.DTO
{
    public class TeamInfo
    {
        public TeamInfo(int stamnummer, string naam, string bijnaam)
        {
            this.stamnummer = stamnummer;
            this.naam = naam;
            this.bijnaam = bijnaam;
        }

        public int stamnummer { get; set; }
        public string naam { get; set; }
        public string bijnaam { get; set; }
        public override string ToString()
        {
            return $"{naam},{stamnummer},{bijnaam}";
        }
    }
}
