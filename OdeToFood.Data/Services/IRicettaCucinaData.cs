using OdeToFood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data.Services
{
    public interface IRicettaCucinaData
    {
        IEnumerable<RicettaCucina> GetAll();
        string Get(int id);
        void Add(RicettaCucina ricettaCucina);
        void Update(int id, string tipo);
        void DeleteRicetta(int id);
        void DeleteCucina(int id);
    }
}
