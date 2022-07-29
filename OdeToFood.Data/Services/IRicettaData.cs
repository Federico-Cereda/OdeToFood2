using OdeToFood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data.Services
{
    public interface IRicettaData
    {
        IEnumerable<Ricetta> GetAll();
        Ricetta Get(int id);
        int Add(Ricetta ricetta);
        void Update(Ricetta ricetta);
        void Delete(int id);
    }
}
