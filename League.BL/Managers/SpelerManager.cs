using League.BL.Domein;
using League.BL.Exceptions;
using League.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.Managers
{
    public class SpelerManager        
    {
        private ISpelerRepository repo;
        public void RegistreerSpeler(string naam,int? lengte,int? gewicht)
        {
            try
            {
                Speler s = new Speler(naam, lengte, gewicht);
                if (!repo.HeeftSpeler(s))
                {
                    repo.SchrijfSpelerInDB(s);
                }
            }
            catch(Exception ex)
            {
                throw new SpelerManagerException("RegistreerSpeler",ex);
            }
        }
    }
}
