using OdeToFood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OdeToFood.Web.Models
{
    public class GreetingViewModel
    {
        public IEnumerable<Ristorante> Ristoranti { get; set; }
        public string Messaggio { get; set; }
        public string Nome { get; set; }
    }
}