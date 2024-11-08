using ConsoleMongoLoad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactFixtureMongo.Models.Fixture
{
    internal class Team
    {
       
        
        public int Id { get; set; }
        public string? Name { get; set; }
        public Location Location { get; set; }
        public List<Player>? Players { get; set; }
    }
}
