using OdeToFood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OdeToFood.Data.Services
{
    public interface ICucinaRistoranteData
    {
        IEnumerable<CucinaRistorante> GetAll();
        List<SelectListItem> Get(int id);
        List<SelectListItem> GetSelected(int id);
        void DeleteCucina(int id);
        void DeleteRistorante(int id);
        void Add(int id, List<SelectListItem> CucineIdTipo);
        void Update(int id, List<SelectListItem> CucineIdTipo);
    }
}
