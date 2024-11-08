using FactFixtureMongo.Models.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMongoLoad.Models
{
    internal class Game
    {
        public int Id { get; set; }
        public League League { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public Venue Venue { get; set; }
        public string Date {  get; set; }
    }
}
