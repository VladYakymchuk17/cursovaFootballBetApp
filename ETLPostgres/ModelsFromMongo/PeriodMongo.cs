
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLPostgres.ModelsFromMongo
{
    internal class PeriodMongo
    {
        public TeamMongo Team { get; set; }
        public string? Name { get; set; }
        public int Assists { get; set; }
        public int Goals { get; set; }
        public int Fouls { get; set; }

        public int Yellow_Cards { get; set; }
        public int Red_Cards { get; set; }
        public int Substitutions { get; set; }
        
    }
}
