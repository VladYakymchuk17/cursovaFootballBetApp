using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMongoLoad.Models
{
    internal class GamePeriods
    {
        public Game Game { get; set; }
        public List<Period> Periods { get; set; }
    }
}
