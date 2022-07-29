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
    public class RicetteController : Controller
    {
        private readonly IRicettaData ricettaData;
        private readonly IRicettaCucinaData ricettaCucinaData;
        private readonly ICucinaData cucinaData;

        public RicetteController(IRicettaData ricettaData, IRicettaCucinaData ricettaCucinaData, ICucinaData cucinaData)
        {
            this.ricettaData = ricettaData;
            this.ricettaCucinaData = ricettaCucinaData;
            this.cucinaData = cucinaData;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = (from r in ricettaData.GetAll()
                         join rc in ricettaCucinaData.GetAll() on r.Id equals rc.IdRicetta
                         join c in cucinaData.GetAll() on rc.IdCucina equals c.Id
                         orderby r.Id
                         select new RicettaViewModel
                         {
                             Id = r.Id,
                             Nome = r.Nome,
                             Tipo = c.Tipo,
                             Ingredienti = r.Ingredienti,
                             Tempo = r.Tempo,
                             Procedimento = r.Procedimento
                         }).ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var ricetta = ricettaData.Get(id);
            var tipo = ricettaCucinaData.Get(id);
            var model = new RicettaViewModel { Id = ricetta.Id, Nome = ricetta.Nome, Tipo = tipo, Ingredienti = ricetta.Ingredienti, Tempo = ricetta.Tempo, Procedimento = ricetta.Procedimento };
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Cucine = cucinaData.GetAll().Select(x => x.Tipo);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RicettaViewModel ricetta)
        {
            if (ModelState.IsValid)
            {
                var ricettaA = new Ricetta() { Id = ricetta.Id, Nome = ricetta.Nome, Ingredienti = ricetta.Ingredienti, Tempo = ricetta.Tempo, Procedimento = ricetta.Procedimento };
                var ricettaId = ricettaData.Add(ricettaA);
                var cucinaId = cucinaData.GetAll().FirstOrDefault(x => x.Tipo == ricetta.Tipo).Id;
                var ricettaCucina = new RicettaCucina() { IdRicetta = ricettaId, IdCucina = cucinaId };
                ricettaCucinaData.Add(ricettaCucina);

                return RedirectToAction("Details", new { id = ricettaId });
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.Cucine = cucinaData.GetAll().Select(x => x.Tipo);
            var ricetta = ricettaData.Get(id);
            var tipo = ricettaCucinaData.Get(id);
            var model = new RicettaViewModel { Id = ricetta.Id, Nome = ricetta.Nome, Tipo = tipo, Ingredienti = ricetta.Ingredienti, Tempo = ricetta.Tempo, Procedimento = ricetta.Procedimento };
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RicettaViewModel ricetta)
        {
            if (ModelState.IsValid)
            {
                var ricettaU = new Ricetta { Id = ricetta.Id, Nome = ricetta.Nome, Ingredienti = ricetta.Ingredienti, Tempo = ricetta.Tempo, Procedimento = ricetta.Procedimento };
                ricettaData.Update(ricettaU);
                ricettaCucinaData.Update(ricetta.Id, ricetta.Tipo);
                TempData["Message"] = "Hai salvato le modifiche!";
                return RedirectToAction("Details", new { id = ricetta.Id });
            }
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model = ricettaData.Get(id);
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
            ricettaData.Delete(id);
            ricettaCucinaData.DeleteRicetta(id);
            return RedirectToAction("Index");
        }
    }
}