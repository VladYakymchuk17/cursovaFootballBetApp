using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactFixtureMongo.Models.Fixture
{
    internal class League
    {
        public int League_id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public int Season { get; set; }
        public string? Round { get; set; }


    }
}
