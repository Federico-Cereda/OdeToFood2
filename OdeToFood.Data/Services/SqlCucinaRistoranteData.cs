using OdeToFood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public IEnumerable<string> Get(int id)
        {
            return from cr in db.CucineRistoranti
                   join c in db.Cucine on cr.IdCucina equals c.Id
                   where cr.IdRistorante == id
                   select c.Tipo;
        }

        public IEnumerable<int> GetIds(int id)
        {
            return from cr in db.CucineRistoranti
                   where cr.IdRistorante == id
                   select cr.IdCucina;
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

        public void Add(int id, IEnumerable<string> CucinaTipi)
        {
            foreach (var tipo in CucinaTipi)
            {
                var cucinaId = db.Cucine.FirstOrDefault(x => x.Tipo == tipo).Id;
                db.CucineRistoranti.Add(new CucinaRistorante { IdCucina = cucinaId, IdRistorante = id });
            }
            db.SaveChanges();
        }

        public void Update(int id, IEnumerable<int> cucinaIds)
        {
            var cucine = from cr in db.CucineRistoranti
                         where cr.IdRistorante == id
                         select cr;
            db.CucineRistoranti.RemoveRange(cucine);
            foreach (var idC in cucinaIds)
            {
                db.CucineRistoranti.Add(new CucinaRistorante { IdCucina = idC, IdRistorante = id });
            }
            db.SaveChanges();
        }
    }
}
