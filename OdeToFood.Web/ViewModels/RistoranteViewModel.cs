using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Web.ViewModels
{
    public class RistoranteViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Display(Name = "Tipo di cucina")]
        public IEnumerable<int> CucinaIds { get; set; }

        [Display(Name = "Tipo di cucina")]
        public IEnumerable<string> CucinaTipi { get; set; }

        public string Indirizzo { get; set; }

        [Display(Name = "Città")]
        public string Citta { get; set; }
    }
}