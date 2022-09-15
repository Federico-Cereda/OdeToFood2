using OdeToFood.Data.Models;
using OdeToFood.Data.Services;
using OdeToFood.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace OdeToFood.Web.Controllers
{
    public class RistorantiController : Controller
    {
        private readonly IRistoranteData ristoranteData;
        private readonly ICucinaData cucinaData;
        private readonly ICucinaRistoranteData cucinaRistoranteData;
        private readonly IRicettaData ricettaData;
        private readonly IRistoranteRicettaData ristoranteRicettaData;
        private readonly IRicettaCucinaData ricettaCucinaData;

        public RistorantiController(IRistoranteData ristoranteData, ICucinaData cucinaData, ICucinaRistoranteData cucinaRistoranteData, IRicettaData ricettaData, IRistoranteRicettaData ristoranteRicettaData, IRicettaCucinaData ricettaCucinaData)
        {
            this.ristoranteData = ristoranteData;
            this.cucinaData = cucinaData;
            this.cucinaRistoranteData = cucinaRistoranteData;
            this.ricettaData = ricettaData;
            this.ristoranteRicettaData = ristoranteRicettaData;
            this.ricettaCucinaData = ricettaCucinaData;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var ristorantiJoinId = (from r in ristoranteData.GetAll()
                                    join cr in cucinaRistoranteData.GetAll() on r.Id equals cr.IdRistorante
                                    join c in cucinaData.GetAll() on cr.IdCucina equals c.Id
                                    join rr in ristoranteRicettaData.GetAll() on r.Id equals rr.IdRistorante into RicetteId
                                    from i in RicetteId.DefaultIfEmpty()
                                    select new
                                    {
                                        Id = r.Id,
                                        Nome = r.Nome,
                                        CucineIdTipo = new SelectListItem { Value = c.Id.ToString(), Text = c.Tipo, Selected = true },
                                        IdRicetta = i?.IdRicetta ?? 0,
                                        Indirizzo = r.Indirizzo,
                                        Citta = r.Citta
                                    }).ToList();

            var ristorantiJoin = (from r in ristorantiJoinId
                                  join rc in ricettaData.GetAll() on r.IdRicetta equals rc.Id into ricetteNome
                                  from n in ricetteNome.DefaultIfEmpty()
                                  select new
                                  {
                                      Id = r.Id,
                                      Nome = r.Nome,
                                      CucineIdTipo = r.CucineIdTipo,
                                      Ricette = n?.Nome ?? "Nessuna ricetta",
                                      Indirizzo = r.Indirizzo,
                                      Citta = r.Citta
                                  }).ToList();

            var model = new List<RistoranteViewModel>();

            ristorantiJoin.ForEach(x =>
            {
                if (!model.Any(m => x.Id == m.Id))
                    model.Add(new RistoranteViewModel { Id = x.Id, Nome = x.Nome, CucineIdTipo = new List<SelectListItem> { x.CucineIdTipo } , Ricette = new List<string> { x.Ricette }, Indirizzo = x.Indirizzo, Citta = x.Citta });
                else
                {
                    var cucine = cucinaRistoranteData.GetSelected(x.Id);
                    model.Where(m => x.Id == m.Id).FirstOrDefault().CucineIdTipo = cucine;

                    if (x.Ricette != "Nessuna ricetta")
                    {
                        var ricette = (from r in ristorantiJoin
                                       where r.Id == x.Id
                                       select r.Ricette).Distinct();
                        model.Where(m => x.Id == m.Id).FirstOrDefault().Ricette = ricette;
                    }
                }
            });

            return View(model);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var ristorante = ristoranteData.Get(id);
            var cucine = cucinaRistoranteData.GetSelected(id);
            var ricette = ristoranteRicettaData.Get(id);
            if (ricette.Count() == 0)
            {
                ricette = new List<string> { "Nessuna ricetta" };
            }
            var model = new RistoranteViewModel { Id = ristorante.Id, Nome = ristorante.Nome, CucineIdTipo = cucine, Ricette = ricette, Indirizzo = ristorante.Indirizzo, Citta = ristorante.Citta };
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new RistoranteViewModel { CucineIdTipo = new List<SelectListItem>() };

            foreach (var tipo in cucinaData.GetAll())
            {
                model.CucineIdTipo.Add(new SelectListItem { Value = tipo.Id.ToString(), Text = tipo.Tipo });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RistoranteViewModel ristorante)
        {
            if (ModelState.IsValid)
            {
                var ristoranteA = new Ristorante() { Id = ristorante.Id, Nome = ristorante.Nome, Indirizzo = ristorante.Indirizzo, Citta = ristorante.Citta };
                var idR = ristoranteData.Add(ristoranteA);
                cucinaRistoranteData.Add(idR, ristorante.CucineIdTipo);
                return RedirectToAction("Details", new { id = idR });
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var ristorante = ristoranteData.Get(id);
            var cucinaIdTipo = cucinaRistoranteData.Get(id);
            var model = new RistoranteViewModel { Id = ristorante.Id, Nome = ristorante.Nome, CucineIdTipo = cucinaIdTipo, Indirizzo = ristorante.Indirizzo, Citta = ristorante.Citta };
            
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
                cucinaRistoranteData.Update(idR, ristorante.CucineIdTipo);
                ristoranteRicettaData.Update(idR, ristorante.CucineIdTipo);
                TempData["Message"] = "Hai salvato le modifiche!";
                return RedirectToAction("Details", new { id = idR });
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditRicette(int id)
        {
            var model = new List<RicettaModificaViewModel>();

            var ricetteId =(from r in ristoranteRicettaData.GetAll()
                           where r.IdRistorante == id
                           select r.IdRicetta).ToList();

            var cucine = (from c in cucinaData.GetAll()
                          join cr in cucinaRistoranteData.GetAll() on c.Id equals cr.IdCucina
                          where cr.IdRistorante == id
                          select new Cucina
                          {
                              Id = c.Id,
                              Tipo = c.Tipo,
                          }).ToList();

            foreach (var idC in cucine)
            {
                var ricetteIdNome = (from r in ricettaData.GetAll()
                                     join rc in ricettaCucinaData.GetAll() on r.Id equals rc.IdRicetta
                                     join cr in cucinaRistoranteData.GetAll() on rc.IdCucina equals cr.IdCucina
                                     where cr.IdRistorante == id && rc.IdCucina == idC.Id
                                     select new SelectListItem
                                     {
                                         Value = r.Id.ToString(),
                                         Text = r.Nome,
                                         Selected = ricetteId.Contains(r.Id) ? true : false
                                     }).ToList();
                var ricette = new RicettaModificaViewModel { RicetteIdNome = ricetteIdNome, Tipo = idC.Tipo };
                model.Add(ricette);
            };

            if (model == null)
            {
                return HttpNotFound();
            }

            var ristorante = (from r in ristoranteData.GetAll()
                              where r.Id == id
                              select new Ristorante
                              {
                                  Id = r.Id,
                                  Nome = r.Nome
                              }).FirstOrDefault();
            TempData["Id"] = ristorante.Id;
            ViewBag.RistoranteNome = ristorante.Nome;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRicette(List<RicettaModificaViewModel> ricette)
        {
            if (ModelState.IsValid)
            {
                int ristoranteId = (int)TempData["Id"];
                var ricetteIdNome = (from r in ricette
                                     select r.RicetteIdNome).ToList();
                ristoranteRicettaData.UpdateRicette(ricetteIdNome, ristoranteId);
                TempData["Message"] = "Hai salvato le modifiche!";
                return RedirectToAction("Details", new { id = ristoranteId });
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
            ristoranteRicettaData.Delete(id);
            return RedirectToAction("Index");
        }
    }
}