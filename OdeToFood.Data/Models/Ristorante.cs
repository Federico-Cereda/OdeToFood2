using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data.Models
{
    public class Ristorante
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Indirizzo { get; set; }

        [Display(Name = "Città")]
        public string Citta { get; set; }
    }
}
