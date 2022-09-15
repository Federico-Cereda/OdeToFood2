using OdeToFood.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data.Services
{
    public class OdeToFoodDbContext : DbContext
    {
        public DbSet<Ristorante> Ristoranti { get; set; }
        public DbSet<Ricetta> Ricette { get; set; }
        public DbSet<Cucina> Cucine { get; set; }
        public DbSet<CucinaRistorante> CucineRistoranti { get; set; }
        public DbSet<RicettaCucina> RicetteCucine { get; set; }
        public DbSet<RistoranteRicetta> RistorantiRicette { get; set; }
    }
}
