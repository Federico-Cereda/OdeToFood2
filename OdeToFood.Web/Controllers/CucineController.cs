using OdeToFood.Data.Models;
using OdeToFood.Data.Services;
using OdeToFood.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OdeToFood.Web.Controllers
{
    public class CucineController : Controller
    {
        private readonly ICucinaData cucinaData;
        private readonly ICucinaRistoranteData cucinaRistoranteData;
        private readonly IRicettaCucinaData ricettaCucinaData;
        private readonly IRicettaData ricettaData;

        public CucineController(ICucinaData cucinaData, ICucinaRistoranteData cucinaRistoranteData, IRicettaCucinaData ricettaCucinaData, IRicettaData ricetteData)
        {
            this.cucinaData = cucinaData;
            this.cucinaRistoranteData = cucinaRistoranteData;
            this.ricettaCucinaData = ricettaCucinaData;
            this.ricettaData = ricetteData;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new List<CucinaViewModel>();

            var idRicette = (from c in cucinaData.GetAll()
                             join rc in ricettaCucinaData.GetAll() on c.Id equals rc.IdCucina into ricetteId
                             from i in ricetteId.DefaultIfEmpty()
                             select new
                             {
                                 Id = c.Id,
                                 Tipo = c.Tipo,
                                 IdRicette = i?.IdRicetta ?? null
                             }).ToList();

            var ricette = (from ir in idRicette
                           join r in ricettaData.GetAll() on ir.IdRicette equals r.Id into ricetteNome
                           from n in ricetteNome.DefaultIfEmpty()
                           select new
                           {
                               Id = ir.Id,
                               Tipo = ir.Tipo,
                               Ricette = n?.Nome ?? "Nessuna ricetta"
                           }).ToList();

            ricette.ForEach(x =>
            {
                if (!model.Any(m => x.Id == m.Id))
                    model.Add(new CucinaViewModel { Id = x.Id, Tipo = x.Tipo, Ricette = new List<string> { x.Ricette } });
                else
                {
                    var temp = model.Where(m => x.Id == m.Id).FirstOrDefault().Ricette.ToList();
                    temp.Add(x.Ricette);
                    model.Where(m => x.Id == m.Id).FirstOrDefault().Ricette = temp;
                }
            });

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cucina cucina)
        {
            if (ModelState.IsValid)
            {
                cucinaData.Add(cucina);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = cucinaData.Get(id);

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cucina cucina)
        {
            if (ModelState.IsValid)
            {
                cucinaData.Update(cucina);
                TempData["Message"] = "Hai salvato le modifiche!";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model = cucinaData.Get(id);
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection form)
        {
            cucinaData.Delete(id);
            cucinaRistoranteData.DeleteCucina(id); 
            ricettaCucinaData.DeleteCucina(id);
            return RedirectToAction("Index");
        }
    }
}