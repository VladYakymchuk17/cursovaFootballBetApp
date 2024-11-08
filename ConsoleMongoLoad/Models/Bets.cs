using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMongoLoad.Models
{
    public class Bets
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public FixtureData FixtureData { get; set; }
        public List<Bet> BetsOptions { get; set; }
    }
}
