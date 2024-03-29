﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OdeToFood.Web.ViewModels
{
    public class RistoranteViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Display(Name = "Tipo di cucina")]
        public List<SelectListItem> CucineIdTipo { get; set; }

        public IEnumerable<string> Ricette { get; set; }

        public string Indirizzo { get; set; }

        [Display(Name = "Città")]
        public string Citta { get; set; }
    }
}