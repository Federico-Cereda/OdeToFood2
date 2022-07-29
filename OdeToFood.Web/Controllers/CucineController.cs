using OdeToFood.Data.Models;
using OdeToFood.Data.Services;
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

        public CucineController(ICucinaData cucinaData, ICucinaRistoranteData cucinaRistoranteData, IRicettaCucinaData ricettaCucinaData)
        {
            this.cucinaData = cucinaData;
            this.cucinaRistoranteData = cucinaRistoranteData;
            this.ricettaCucinaData = ricettaCucinaData;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = cucinaData.GetAll();
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