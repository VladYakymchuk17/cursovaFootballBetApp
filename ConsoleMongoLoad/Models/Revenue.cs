using FactFixtureMongo.Models.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMongoLoad.Models
{
    internal class Revenue
    {
        public Team Home { get; set; }
        public Team Away { get; set; }
        public string Date { get; set; }
        public string Result { get; set; }
        public int Number_of_bets { get; set; }
        public int Income { get; set; }
        public int Outbet {  get; set; }

        public double Coef {  get; set; }

      
    }
}
