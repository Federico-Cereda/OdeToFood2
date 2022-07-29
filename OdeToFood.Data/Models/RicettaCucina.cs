using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data.Models
{
    public class RicettaCucina
    {
        public int Id { get; set; }
        public int IdRicetta { get; set; }
        public int IdCucina { get; set; }
    }
}
