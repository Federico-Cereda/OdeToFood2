using OdeToFood.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data.Services
{
    public class SqlRistoranteData : IRistoranteData
    {
        private readonly OdeToFoodDbContext db;

        public SqlRistoranteData(OdeToFoodDbContext db)
        {
            this.db = db;
        }

        public int Add(Ristorante ristorante, IEnumerable<int> cucinaIds)
        {
            db.Ristoranti.Add(ristorante);
            db.SaveChanges();

            foreach (var id in cucinaIds)
            {
                db.CucineRistoranti.Add(new CucinaRistorante { IdCucina = id, IdRistorante = ristorante.Id });
            }
            db.SaveChanges();

            var idR = ristorante.Id;
            return idR;
        }

        public void Delete(int id)
        {
            var ristorante = db.Ristoranti.Find(id);
            db.Ristoranti.Remove(ristorante);
            db.SaveChanges();
        }

        public Ristorante Get(int id)
        {
            return db.Ristoranti.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Ristorante> GetAll()
        {
            return from r in db.Ristoranti
                   orderby r.Nome
                   select r;
        }

        public int Update(Ristorante ristorante, IEnumerable<int> cucinaIds)
        {
            var ristoranteUpdate = db.Ristoranti.Where(r => r.Id == ristorante.Id).FirstOrDefault();
            ristoranteUpdate.Nome = ristorante.Nome;
            ristoranteUpdate.Indirizzo = ristorante.Indirizzo;
            ristoranteUpdate.Citta = ristorante.Citta;
            var entry = db.Entry(ristoranteUpdate);
            entry.State = EntityState.Modified;
            db.SaveChanges();

            var cucine = from cr in db.CucineRistoranti
                         where cr.IdRistorante == ristorante.Id
                         select cr;
            db.CucineRistoranti.RemoveRange(cucine);
            foreach (var id in cucinaIds)
            {
                db.CucineRistoranti.Add(new CucinaRistorante { IdCucina = id, IdRistorante = ristorante.Id });
            }
            db.SaveChanges();

            var idR = ristorante.Id;
            return idR;
        }
    }
}
