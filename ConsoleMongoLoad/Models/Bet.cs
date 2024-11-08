using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMongoLoad.Models
{
    public class Bet
    {
        public string Name { get; set; }
        public double Coefficient { get; set; }
        public int NumberOfBets { get; set; }
        public int Income { get; set; }
    }
}
