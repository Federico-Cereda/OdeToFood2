using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OdeToFood.Web.ViewModels
{
    public class RicettaModificaViewModel
    {
        [Display(Name = "Ricette")]
        public List<SelectListItem> RicetteIdNome { get; set; }

        [Display(Name = "Tipo di cucina")]
        public string Tipo { get; set; }
    }
}