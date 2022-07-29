using OdeToFood.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data.Services
{
    public class SqlRicettaCucinaData : IRicettaCucinaData
    {
        private readonly OdeToFoodDbContext db;

        public SqlRicettaCucinaData(OdeToFoodDbContext db)
        {
            this.db = db;
        }

        public void Add(RicettaCucina ricettaCucina)
        {
            db.RicetteCucine.Add(ricettaCucina);
            db.SaveChanges();
        }

        public void DeleteRicetta(int id)
        {
            var idRC = db.RicetteCucine.FirstOrDefault(x => x.IdRicetta == id).Id;
            var ricettaCucina = db.RicetteCucine.Find(idRC);
            db.RicetteCucine.Remove(ricettaCucina);
            db.SaveChanges();
        }

        public void DeleteCucina(int id)
        {
            var ricettaCucina = from rc in db.RicetteCucine
                                where rc.IdCucina == id
                                select rc;
            db.RicetteCucine.RemoveRange(ricettaCucina);
            db.SaveChanges();
        }

        public string Get(int id)
        {
            return (from rc in db.RicetteCucine
                    join c in db.Cucine on rc.IdCucina equals c.Id
                    where rc.IdRicetta == id
                    select c.Tipo).FirstOrDefault();
        }

        public IEnumerable<RicettaCucina> GetAll()
        {
            return from rc in db.RicetteCucine
                   orderby rc.IdRicetta
                   select rc;
        }

        public void Update(int id, string tipo)
        {
            var idRC = (from rc in db.RicetteCucine
                        where rc.IdRicetta == id
                        select rc.Id).FirstOrDefault();
            var idC = db.Cucine.Where(x => x.Tipo == tipo).FirstOrDefault().Id;
            var ricettaCucina = new RicettaCucina { Id = idRC, IdRicetta = id, IdCucina = idC };
            var entry = db.Entry(ricettaCucina);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
