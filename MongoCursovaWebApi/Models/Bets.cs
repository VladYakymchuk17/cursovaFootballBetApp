using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoCursovaWebApi.Models
{
    public class Bets
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public FixtureData FixtureData { get; set; }
        public List<Bet> BetsOptions { get; set; }
    }
}
