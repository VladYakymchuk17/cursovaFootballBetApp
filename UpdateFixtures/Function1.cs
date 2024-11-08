using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Transform;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace UpdateFixtures
{
    public class Function1
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly string mongoDBConnectionString = "mongodb+srv://vladyslavyakymchukpz2021:w9s3mP0ZGVWSvDwN@cluster0.zjmnvay.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";
        private static readonly IMongoClient mongoClient = new MongoClient(mongoDBConnectionString);
        private static readonly IMongoDatabase database = mongoClient.GetDatabase("Football"); 
        private static readonly IMongoCollection<BsonDocument> fixturesCollection = database.GetCollection<BsonDocument>("Fixtures"); 
        private static readonly Dictionary<string, int> attendance = new Dictionary<string, int>()
        {

            {"Bayer Leverkusen",  29978},
            {"Bayern Munich" , 75000},
            {"VfB Stuttgart", 53600 },
            {"Borussia Dortmund", 81280 },
            {"RB Leipzig", 44904 },
            {"Eintracht Frankfurt", 56645 },
            {"1899 Hoffenheim", 22925 },
            {"Werder Bremen", 41613 },
            {"SC Freiburg", 34303 },
            {"FC Heidenheim", 15000 },
            {"FC Augsburg", 28812 },
            {"Borussia Monchengladbach", 50350 },
            {"VfL Wolfsburg", 26161 },
            {"Union Berlin", 21748 },
            {"Vfl Bochum", 25650 },
            {"1.FC Köln", 49827 },
            {"FSV Mainz 05", 29529 },
            {"SV Darmstadt 98", 17713 }

        };

        [FunctionName("Function1")]
        public static async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            try
            {
              
                var apiUrl = "https://api-football-v1.p.rapidapi.com/v3/fixtures?season=2023&league=78";
                var apiKey = "fc517f3f59msh0003d703ec5f8c8p1b96a2jsn532839b11888";
                var headers = new Dictionary<string, string>
                {
                    { "x-rapidapi-key", apiKey },
                    { "x-rapidapi-host", "api-football-v1.p.rapidapi.com" }
                };

                httpClient.DefaultRequestHeaders.Add("x-rapidapi-key", apiKey);
                httpClient.DefaultRequestHeaders.Add("x-rapidapi-host", "api-football-v1.p.rapidapi.com");

                var response = await httpClient.GetAsync("https://api-football-v1.p.rapidapi.com/v3/fixtures?season=2023&league=78");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.Content);
                    var content = await response.Content.ReadAsStringAsync();
                    var fixtures = BsonDocument.Parse(content);


                    var filter = Builders<BsonDocument>.Filter.Eq("league.season", 2023);
                    var fixt =await fixturesCollection.FindAsync(filter);
                    
                    await fixturesCollection.DeleteManyAsync(filter);


                    var fixturesArray = fixtures["response"].AsBsonArray;
                    var bsonDocuments = new List<BsonDocument>();


                    foreach (var fixture in fixturesArray)
                    {
                        var fixtureDoc = fixture.AsBsonDocument;
                        var homeTeamName = fixtureDoc["teams"]["home"]["name"].AsString;

                        
                        var clubAttendances = attendance;
                        Random random = new Random();
                        int rand = random.Next(100, 1000);

                        if (clubAttendances.ContainsKey(homeTeamName))
                        {
                            fixtureDoc.Add("attendance", clubAttendances[homeTeamName]-rand);
                        }

                        bsonDocuments.Add(fixtureDoc);
                    }

                    await fixturesCollection.InsertManyAsync(bsonDocuments);


                    log.LogInformation("Fixtures updated successfully.");
                }
                else
                {
                    log.LogError($"Failed to fetch fixtures: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                log.LogError($"Error: {ex.Message}");
            }
        }
    }
}
