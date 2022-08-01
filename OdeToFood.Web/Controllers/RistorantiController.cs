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
    public class RistorantiController : Controller
    {
        private readonly IRistoranteData ristoranteData;
        private readonly ICucinaData cucinaData;
        private readonly ICucinaRistoranteData cucinaRistoranteData;

        public RistorantiController(IRistoranteData ristoranteData, ICucinaData cucinaData, ICucinaRistoranteData cucinaRistoranteData)
        {
            this.ristoranteData = ristoranteData;
            this.cucinaData = cucinaData;
            this.cucinaRistoranteData = cucinaRistoranteData;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var ristorantiJoin = (from r in ristoranteData.GetAll()
                                  join cr in cucinaRistoranteData.GetAll() on r.Id equals cr.IdRistorante
                                  join c in cucinaData.GetAll() on cr.IdCucina equals c.Id
                                  select new
                                  {
                                      Id = r.Id,
                                      Nome = r.Nome,
                                      CucinaTipi = c.Tipo,
                                      Indirizzo = r.Indirizzo,
                                      Citta = r.Citta
                                  }).ToList();

            var model = new List<RistoranteViewModel>();

            ristorantiJoin.ForEach(x =>
            {
                if (!model.Any(m => x.Id == m.Id))
                    model.Add(new RistoranteViewModel { Id = x.Id, Nome = x.Nome, CucinaTipi = new List<string> { x.CucinaTipi }, Indirizzo = x.Indirizzo, Citta = x.Citta });
                else
                {
                    var temp = model.Where(m => x.Id == m.Id).FirstOrDefault().CucinaTipi.ToList();
                    temp.Add(x.CucinaTipi);
                    model.Where(m => x.Id == m.Id).FirstOrDefault().CucinaTipi = temp;
                }
            });

            return View(model);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var ristorante = ristoranteData.Get(id);
            var cucina = cucinaRistoranteData.Get(id);
            var model = new RistoranteViewModel { Id = ristorante.Id, Nome = ristorante.Nome, CucinaTipi = cucina, Indirizzo = ristorante.Indirizzo, Citta = ristorante.Citta };
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Cucine = cucinaData.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RistoranteViewModel ristorante)
        {
            if (ModelState.IsValid)
            {
                var ristoranteA = new Ristorante() { Id = ristorante.Id, Nome = ristorante.Nome, Indirizzo = ristorante.Indirizzo, Citta = ristorante.Citta };
                var idR = ristoranteData.Add(ristoranteA);
                cucinaRistoranteData.Add(idR, ristorante.CucinaIds);
                return RedirectToAction("Details", new { id = idR });
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.Cucine = cucinaData.GetAll();

            var ristorante = ristoranteData.Get(id);
            var cucinaIds = cucinaRistoranteData.GetIds(id);
            var cucinaTipi = cucinaRistoranteData.Get(id);
            var model = new RistoranteViewModel { Id = ristorante.Id, Nome = ristorante.Nome, CucinaIds = cucinaIds, CucinaTipi = cucinaTipi, Indirizzo = ristorante.Indirizzo, Citta = ristorante.Citta };
            
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RistoranteViewModel ristorante)
        {
            if (ModelState.IsValid)
            {
                var ristoranteU = new Ristorante { Id = ristorante.Id, Nome = ristorante.Nome, Indirizzo = ristorante.Indirizzo, Citta = ristorante.Citta, };
                var idR = ristoranteData.Update(ristoranteU);
                cucinaRistoranteData.Update(idR, ristorante.CucinaIds);
                TempData["Message"] = "Hai salvato le modifiche!";
                return RedirectToAction("Details", new { id = idR });
            }
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model = ristoranteData.Get(id);
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
            ristoranteData.Delete(id);
            cucinaRistoranteData.DeleteRistorante(id);
            return RedirectToAction("Index");
        }
    }
}