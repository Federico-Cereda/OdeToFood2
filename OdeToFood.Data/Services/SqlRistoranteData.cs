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

        public int Add(Ristorante ristorante)
        {
            db.Ristoranti.Add(ristorante);
            db.SaveChanges();
            return ristorante.Id;
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

        public int Update(Ristorante ristorante)
        {
            var ristoranteUpdate = db.Ristoranti.Where(r => r.Id == ristorante.Id).FirstOrDefault();
            ristoranteUpdate.Nome = ristorante.Nome;
            ristoranteUpdate.Indirizzo = ristorante.Indirizzo;
            ristoranteUpdate.Citta = ristorante.Citta;
            var entry = db.Entry(ristoranteUpdate);
            entry.State = EntityState.Modified;
            db.SaveChanges();
            return ristorante.Id;
        }
    }
}
