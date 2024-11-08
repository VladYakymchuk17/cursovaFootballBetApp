
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLPostgres.ModelsFromMongo
{
    internal class RevenueMongo
    {
        public TeamMongo Home { get; set; }
        public TeamMongo Away { get; set; }
        public string Date { get; set; }
        public string Result { get; set; }
        public int Number_of_bets { get; set; }
        public int Income { get; set; }
        public int Outbet {  get; set; }

        public double Coef {  get; set; }

      
    }
}
