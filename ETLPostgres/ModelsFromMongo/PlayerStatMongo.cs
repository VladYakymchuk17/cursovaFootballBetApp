
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLPostgres.ModelsFromMongo
{
    internal class PlayerStatMongo
    {
        public PlayerMongo Player { get; set; }
        public string Date { get; set; }
        public int Assists { get; set; }
        public int Goals { get; set; }
        public int Fouls { get; set; }
        public int Time { get; set; }

    }
}
