
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLPostgres.ModelsFromMongo
{
    internal class TeamMongo
    {
       
        
        public int Id { get; set; }
        public string? Name { get; set; }
        public LocationMongo Location { get; set; }
        public List<PlayerMongo>? Players { get; set; }
    }
}
