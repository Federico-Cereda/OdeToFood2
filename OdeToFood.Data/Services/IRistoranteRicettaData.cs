using OdeToFood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OdeToFood.Data.Services
{
    public interface IRistoranteRicettaData
    {
        IEnumerable<RistoranteRicetta> GetAll();
        IEnumerable<string> Get(int id);
        void Update(int idR, List<SelectListItem> cucineIdTipo);
        void UpdateRicette(List<List<SelectListItem>> ricetteIdNome, int ristoranteId);
        void Delete(int id);
    }
}
