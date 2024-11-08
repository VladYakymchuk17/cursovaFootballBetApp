using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLPostgres.ModelsFromMongo
{
    internal class GamePlayerMongo
    {
        public ObjectId _id { get; set; }
        public List<PlayerStatMongo> playerStats { get; set; }
        public GameMongo Game { get; set; }
    }
}
