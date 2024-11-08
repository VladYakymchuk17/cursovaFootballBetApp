using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLPostgres.ModelsFromMongo
{
    internal class LeagueMongo
    {
        public int League_id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public int Season { get; set; }
        public string? Round { get; set; }


    }
}
