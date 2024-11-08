using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMongoLoad.Models
{
    internal class GamePlayer
    {
        public List<PlayerStat> playerStats { get; set; }
        public Game Game { get; set; }
    }
}
