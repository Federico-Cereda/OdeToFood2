using OdeToFood.Data.Models;
using OdeToFood.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OdeToFood.Web.Api
{
    public class RistorantiController : ApiController
    {
        private readonly IRistoranteData db;

        public RistorantiController(IRistoranteData db)
        {
            this.db = db;
        }

        public IEnumerable<Ristorante> Get()
        {
            var model = db.GetAll();
            return model;
        }
    }
}
