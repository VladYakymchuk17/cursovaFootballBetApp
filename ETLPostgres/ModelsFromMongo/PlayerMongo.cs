using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLPostgres.ModelsFromMongo
{
    internal class PlayerMongo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Last_name { get; set; }
        public int TeamId { get; set; }
        public int Age { get; set; }
        public string? Nationality { get; set; }
        
    }
}
