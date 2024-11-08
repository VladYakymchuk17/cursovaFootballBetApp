using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLPostgres.ModelsFromMongo
{
    internal class GamePeriodsMongo
    {
        public ObjectId _id { get; set; }
        public GameMongo Game { get; set; }
        public List<PeriodMongo> Periods { get; set; }
    }
}
