using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data.Models
{
    public class Cucina
    {
        public int Id { get; set; }

        [Display(Name = "Tipo di cucina")]
        public string Tipo { get; set; }
    }
}
