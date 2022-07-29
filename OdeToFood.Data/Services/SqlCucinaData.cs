using OdeToFood.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data.Services
{
    public class SqlCucinaData : ICucinaData
    {
        private readonly OdeToFoodDbContext db;

        public SqlCucinaData(OdeToFoodDbContext db)
        {
            this.db = db;
        }

        public void Add(Cucina cucina)
        {
            db.Cucine.Add(cucina);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var cucina = db.Cucine.Find(id);
            db.Cucine.Remove(cucina);
            db.SaveChanges();
        }

        public Cucina Get(int id)
        {
            return db.Cucine.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Cucina> GetAll()
        {
            return from c in db.Cucine
                   orderby c.Id
                   select c;
        }

        public void Update(Cucina cucina)
        {
            var entry = db.Entry(cucina);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
