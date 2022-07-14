using OdeToFood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data.Services
{
    public interface ICucinaData
    {
        IEnumerable<Cucina> GetAll();
        Cucina Get(int id);
        void Add(Cucina cucina);
        void Delete(int id);
    }
}
