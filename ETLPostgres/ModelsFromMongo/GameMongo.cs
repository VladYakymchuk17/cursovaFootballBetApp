
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLPostgres.ModelsFromMongo
{
    internal class GameMongo
    {
        public int Id { get; set; }
        public LeagueMongo League { get; set; }
        public TeamMongo HomeTeam { get; set; }
        public TeamMongo AwayTeam { get; set; }
        public VenueMongo Venue { get; set; }
        public string Date {  get; set; }
    }
}
