using OdeToFood.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data.Services
{
    public class SqlRicettaData : IRicettaData
    {
        private readonly OdeToFoodDbContext db;

        public SqlRicettaData(OdeToFoodDbContext db)
        {
            this.db = db;
        }

        public void Add(Ricetta ricetta)
        {
            db.Ricette.Add(ricetta);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var ricetta = db.Ricette.Find(id);
            db.Ricette.Remove(ricetta);
            db.SaveChanges();
        }

        public Ricetta Get(int id)
        {
            return db.Ricette.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Ricetta> GetAll()
        {
            return from r in db.Ricette
                   orderby r.Nome
                   select r;
        }

        public void Update(Ricetta ricetta)
        {
            var entry = db.Entry(ricetta);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
