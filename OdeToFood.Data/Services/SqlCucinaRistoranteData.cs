using OdeToFood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OdeToFood.Data.Services
{
    public class SqlCucinaRistoranteData : ICucinaRistoranteData
    {
        private readonly OdeToFoodDbContext db;

        public SqlCucinaRistoranteData(OdeToFoodDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<CucinaRistorante> GetAll()
        {
            return from cr in db.CucineRistoranti
                   orderby cr.IdRistorante
                   select cr;
        }

        public List<SelectListItem> Get(int id)
        {
            var idCucine = from cr in db.CucineRistoranti
                           join c in db.Cucine on cr.IdCucina equals c.Id
                           where cr.IdRistorante == id
                           select c.Id;

            var cucine = (from c in db.Cucine
                          select new SelectListItem
                          {
                              Value = c.Id.ToString(),
                              Text = c.Tipo,
                              Selected = false
                          }).ToList();

            foreach (var cucina in cucine)
            {
                foreach (var idC in idCucine)
                {
                    if (int.Parse(cucina.Value) == idC)
                    {
                        cucina.Selected = true;
                    }
                }
            }

            return cucine;
        }
        public List<SelectListItem> GetSelected(int id)
        {
            return (from cr in db.CucineRistoranti
                    join c in db.Cucine on cr.IdCucina equals c.Id
                    where cr.IdRistorante == id
                    select new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Tipo,
                        Selected = true
                    }).ToList();
        }

        public void DeleteCucina(int id)
        {
            var cucine = from cr in db.CucineRistoranti
                         where cr.IdCucina == id
                         select cr;
            db.CucineRistoranti.RemoveRange(cucine);
            db.SaveChanges();
        }

        public void DeleteRistorante(int id)
        {
            var ristoranti = from cr in db.CucineRistoranti
                             where cr.IdRistorante == id
                             select cr;
            db.CucineRistoranti.RemoveRange(ristoranti);
            db.SaveChanges();
        }

        public void Add(int id, List<SelectListItem> CucineIdTipo)
        {
            foreach (var tipo in CucineIdTipo)
            {
                if (tipo.Selected == true)
                {
                    var cucinaId = int.Parse(tipo.Value);
                    db.CucineRistoranti.Add(new CucinaRistorante { IdCucina = cucinaId, IdRistorante = id });
                }
            }
            db.SaveChanges();
        }

        public void Update(int id, List<SelectListItem> CucineIdTipo)
        {
            var cucine = from cr in db.CucineRistoranti
                         where cr.IdRistorante == id
                         select cr;
            db.CucineRistoranti.RemoveRange(cucine);
            foreach (var c in CucineIdTipo)
            {
                if (c.Selected == true)
                {
                    db.CucineRistoranti.Add(new CucinaRistorante { IdCucina = int.Parse(c.Value), IdRistorante = id });
                }
            }
            db.SaveChanges();
        }
    }
}
