using League.BL.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.Interfaces
{
    public interface ISpelerRepository
    {
        void SchrijfSpelerInDB(Speler s);
        bool HeeftSpeler(Speler s);
    }
}
