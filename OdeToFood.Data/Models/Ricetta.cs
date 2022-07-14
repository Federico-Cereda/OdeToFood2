using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data.Models
{
    public class Ricetta
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Ingredienti { get; set; }
        public int Tempo { get; set; }
        public string Procedimento { get; set; }
    }
}
