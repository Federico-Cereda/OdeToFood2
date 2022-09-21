using OdeToFood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OdeToFood.Data.Services
{
    public class SqlRistoranteRicettaData : IRistoranteRicettaData
    {
        private readonly OdeToFoodDbContext db;

        public SqlRistoranteRicettaData(OdeToFoodDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<RistoranteRicetta> GetAll()
        {
            return from r in db.RistorantiRicette
                   orderby r.IdRistorante
                   select r;
        }

        public IEnumerable<string> Get(int id)
        {
            return from r in db.Ricette
                   join rr in db.RistorantiRicette on r.Id equals rr.IdRicetta
                   where rr.IdRistorante == id
                   select r.Nome;
        }

        public void Update(int idR, List<SelectListItem> cucineIdTipo)
        {
            foreach (var c in cucineIdTipo)
            {
                if (c.Selected == false)
                {
                    var cucina = int.Parse(c.Value);
                    var ricette = (from rc in db.RicetteCucine
                                   where rc.IdCucina == cucina
                                   select rc.IdRicetta).ToList();
                    foreach (var r in ricette)
                    {
                        var ristorantiRicette = from rr in db.RistorantiRicette
                                                where rr.IdRicetta == r && rr.IdRistorante == idR
                                                select rr;
                        db.RistorantiRicette.RemoveRange(ristorantiRicette);
                    }
                }
            }
            db.SaveChanges();
        }

        public void UpdateRicette(List<List<SelectListItem>> ricetteIdNome, int ristoranteId)
        {
            var ricette = from r in db.RistorantiRicette
                          where r.IdRistorante == ristoranteId
                          select r;
            db.RistorantiRicette.RemoveRange(ricette);
            foreach (var item in ricetteIdNome)
            {
                foreach (var i in item)
                {
                    if (i.Selected == true)
                    {
                        db.RistorantiRicette.Add(new RistoranteRicetta { IdRicetta = int.Parse(i.Value), IdRistorante = ristoranteId });
                    }
                }
            }
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var ristoranteRicetta = from r in db.RistorantiRicette
                                    where r.IdRistorante == id
                                    select r;
            db.RistorantiRicette.RemoveRange(ristoranteRicetta);
            db.SaveChanges();
        }

    }
}
