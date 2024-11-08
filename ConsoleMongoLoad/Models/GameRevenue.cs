using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMongoLoad.Models
{
    internal class GameRevenue
    {
        public Game? Game { get; set; }
        public List<Revenue>? revenues { get; set; }
    }
}
