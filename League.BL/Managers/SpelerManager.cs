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
    public class SpelerManager
    {
        private ISpelerRepository repo;

        public SpelerManager(ISpelerRepository repo)
        {
            this.repo = repo;
        }

        //dit zijn spelers die nog niet tot een bepaald team behoren of behoord hebben
        public Speler RegistreerSpeler(string naam, int? lengte, int? gewicht)
        {
            try
            {
                Speler s = new Speler(naam, lengte, gewicht);
                if (!repo.BestaatSpeler(s))
                {
                    s = repo.SchrijfSpelerInDB(s);
                    return s;
                }
                else
                {
                    throw new SpelerManagerException("RegistreerSpeler - speler bestaat al");
                }
            }
            catch (SpelerManagerException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SpelerManagerException("RegistreerSpeler", ex);
            }
        }
        public void UpdateSpeler(SpelerInfo spelerInfo)
        {
            if (spelerInfo == null) throw new SpelerManagerException("UpdateSpeler - speler is null");
            try
            {
                if (repo.BestaatSpeler(spelerInfo.id))
                {
                    Speler speler = repo.SelecteerSpeler(spelerInfo.id);
                    bool changed = false;
                    if (speler.Naam != spelerInfo.naam) { 
                        speler.ZetNaam(spelerInfo.naam); 
                        changed = true; 
                    }
                    if ((speler.Lengte.HasValue) && (speler.Lengte != spelerInfo.lengte))
                    {
                        speler.ZetLengte((int)spelerInfo.lengte);
                        changed = true;
                    }
                    if ((speler.Gewicht.HasValue) && (speler.Gewicht != spelerInfo.gewicht))
                    { 
                        speler.ZetGewicht((int)spelerInfo.gewicht); 
                        changed = true; 
                    }
                    if ((speler.Rugnummer.HasValue) && (speler.Rugnummer != spelerInfo.rugnummer))
                    {
                        speler.ZetRugnummer((int)spelerInfo.rugnummer); 
                        changed = true;
                    }
                    if (!changed) throw new SpelerManagerException("UpdateSpeler - no changes");
                    repo.UpdateSpeler(speler);
                }
                else
                    {
                        throw new SpelerManagerException("UpdateSpeler - speler niet gevonden");
                    }
                }
            catch (SpelerManagerException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SpelerManagerException("UpdateSpeler", ex);
            }
        }
        public IReadOnlyList<SpelerInfo> SelecteerSpelers(int? id, string naam)
        {
            if ((!id.HasValue) && string.IsNullOrWhiteSpace(naam)) throw new SpelerManagerException("SelecteerSpelers - no input");
            try
            {
                return repo.SelecteerSpelers(id, naam);
            }
            catch (Exception ex)
            {
                throw new SpelerManagerException("SelecteerSpelers", ex);
            }
        }
    }
}
