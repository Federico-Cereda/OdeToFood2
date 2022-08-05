﻿using OdeToFood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data.Services
{
    public interface ICucinaRistoranteData
    {
        IEnumerable<CucinaRistorante> GetAll();
        IEnumerable<string> Get(int id);
        IEnumerable<int> GetIds(int id);
        void DeleteCucina(int id);
        void DeleteRistorante(int id);
        void Add(int id, IEnumerable<string> CucinaTipi);
        void Update(int id, IEnumerable<int> cucinaIds);
    }
}
