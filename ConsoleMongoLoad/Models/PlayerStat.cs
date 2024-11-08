using FactFixtureMongo.Models.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMongoLoad.Models
{
    internal class PlayerStat
    {
        public Player Player { get; set; }
        public string Date { get; set; }
        public int Assists { get; set; }
        public int Goals { get; set; }
        public int Fouls { get; set; }
        public int Time { get; set; }

    }
}
