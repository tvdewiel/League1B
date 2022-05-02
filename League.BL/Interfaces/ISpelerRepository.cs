using League.BL.Domein;
using League.BL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.Interfaces
{
    public interface ISpelerRepository
    {
        Speler SchrijfSpelerInDB(Speler s);
        bool BestaatSpeler(Speler s);
        void UpdateSpeler(Speler speler);
        IReadOnlyList<SpelerInfo> SelecteerSpelers(int? id, string naam);
        bool BestaatSpeler(int spelerId);
        Speler SelecteerSpeler(int id);
    }
}
