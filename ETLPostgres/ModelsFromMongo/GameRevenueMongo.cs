using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLPostgres.ModelsFromMongo
{
    internal class GameRevenueMongo
    {
        public ObjectId _id { get; set; }
        public GameMongo? Game { get; set; }
        public List<RevenueMongo>? revenues { get; set; }
    }
}
