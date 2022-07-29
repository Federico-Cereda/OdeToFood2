using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OdeToFood.Web.ViewModels
{
    public class RicettaViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        [Display(Name = "Tipo di cucina")]
        public string Tipo { get; set; }
        public string Ingredienti { get; set; }
        public int Tempo { get; set; }
        public string Procedimento { get; set; }
    }
}