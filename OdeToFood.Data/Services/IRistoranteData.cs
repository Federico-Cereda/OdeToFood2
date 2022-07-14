using OdeToFood.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data.Services
{
    public interface IRistoranteData
    {
        IEnumerable<Ristorante> GetAll();
        Ristorante Get(int id);
        int Add(Ristorante ristorante, IEnumerable<int> cucinaIds);
        int Update(Ristorante ristorante, IEnumerable<int> cucinaIds);
        void Delete(int id);
    }
}
