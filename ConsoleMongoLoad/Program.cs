using Amazon.Runtime;
using ConsoleMongoLoad.Models;
using FactFixtureMongo.Models.Fixture;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleMongoLoad
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            int stop = 0;
            List<string> germanStates = new List<string>
            {
                "Bayern (Bavaria)",
                "Niedersachsen (Lower Saxony)",
                "Baden-Württemberg",
                "Rheinland-Pfalz (Rhineland-Palatinate)",
                "Sachsen (Saxony)",
                "Thüringen (Thuringia)",
                "Hessen",
                "Nordrhein-Westfalen (North Rhine-Westphalia)",
                "Sachsen-Anhalt (Saxony-Anhalt)",
                "Brandenburg",
                "Mecklenburg-Vorpommern",
                "Schleswig-Holstein",
                "Saarland"
            };

            string mongoConnectionString = "mongodb+srv://vladyslavyakymchukpz2021:w9s3mP0ZGVWSvDwN@cluster0.zjmnvay.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";
            string databaseName = "Football";



            await Console.Out.WriteLineAsync("Entering db");
            var mongoClient = new MongoClient(mongoConnectionString);
            var database = mongoClient.GetDatabase(databaseName);
            await Console.Out.WriteLineAsync("Entering collections");
            var collection = database.GetCollection<BsonDocument>("Fixtures");
            var teamCollection = database.GetCollection<Team>("Team");
            var gameCollection = database.GetCollection<Game>("Games");
            var playerStatCollection = database.GetCollection<GamePlayer>("PlayerStat");
            var revenueCollection = database.GetCollection<GameRevenue>("Revenue");
            var periodsCollection = database.GetCollection<GamePeriods>("GamePeriods");
            IMongoCollection<Bets> betsCollection =database.GetCollection<Bets>("Bets");

            var filterBets = Builders<Bets>.Filter.Empty;
            var betsFiltered = await betsCollection.FindAsync(filterBets);

            var betsList = await betsFiltered.ToListAsync();



            var filter = Builders<BsonDocument>.Filter.Eq("league.season", 2023);

            var fixturesFiltered = await collection.FindAsync(filter);
            var fixtures = await fixturesFiltered.ToListAsync();

            var filterGames = Builders<Game>.Filter.Empty;


            var gamesFiltered = await gameCollection.FindAsync(filterGames);
            var fixturesFacts = await gamesFiltered.ToListAsync();


            var rapidAPIKey = "fc517f3f59msh0003d703ec5f8c8p1b96a2jsn532839b11888";
            var headers = new Dictionary<string, string>
                          {
                              { "x-rapidapi-key", rapidAPIKey },
                              { "x-rapidapi-host", "api-football-v1.p.rapidapi.com" }
                          };

            List<string> collectionNewNames = new List<string> { "Games","PlayerStat", "Team" ,"Revenue"};
            var collectionNames = await database.ListCollectionNamesAsync();
            var collectionList = collectionNames.ToList();


           

            foreach (var name in collectionNewNames)
            {
                bool res = collectionList.Contains(name);
                if (!res)
                {
                    await database.CreateCollectionAsync(name);
                    Console.WriteLine($"Collection '{name}' created successfully.");
                }
                else
                {
                    Console.WriteLine($"Collection '{name}' already exists.");
                }
            }

            List<long> fixtureIds = new List<long>();
            List<long> fixtureFactIds = new List<long>();
            foreach (var fixture in fixtures)
            {
                fixtureIds.Add(fixture["fixture"]["id"].AsInt32);

            }
            foreach(var g in fixturesFacts)
            {
                fixtureFactIds.Add(g.Id);
            }

            Console.WriteLine($"Count: {fixtures.Count}");

            foreach (var fixture in fixtures)
            {

                int existingId = 0;
                bool existingFixture = false;
                if (fixture["fixture"]["status"]["long"].AsString != "Match Finished")
                {
                    continue;
                }
                foreach (var factId in fixtureFactIds)
                {
                    if (factId == fixture["fixture"]["id"].AsInt32)
                    {
                        existingId = fixture["fixture"]["id"].AsInt32;
                        existingFixture = true;
                        break;
                    }
                }
                if (existingFixture)
                {
                    Console.WriteLine($"Fixture {existingId} existed");
                    continue;
                }
                
                if (stop >= 1)
                {
                    return;
                }
                Venue venue = new Venue();
                string date;
                League league = new League();
                Location location = new Location();
                List<Period> periods = new List<Period>();
                Team hometeam = new Team();
                Team away = new Team();
                List<Player> homePlayers = new List<Player>();
                List<Player> awayPlayers = new List<Player>();
                Game game = new Game();
                PlayerStat playerStat = new PlayerStat();
                Revenue revenue = new Revenue();
                GamePlayer gamePlayer = new GamePlayer();   
                GameRevenue gameRevenue = new GameRevenue();
                GamePeriods gamePeriods = new GamePeriods();


                venue.Id = fixture["fixture"]["venue"]["id"].AsInt32;
                venue.City = fixture["fixture"]["venue"]["city"].ToString();
                venue.Name = fixture["fixture"]["venue"]["name"].AsString;

              

                date = fixture["fixture"]["date"].AsString;

                league.League_id = fixture["league"]["id"].AsInt32;
                league.Name = fixture["league"]["name"].AsString;
                league.Country = fixture["league"]["country"].AsString;
                league.Season = fixture["league"]["season"].AsInt32;
                league.Round = fixture["league"]["round"].AsString;


                var existingHomeTeam = await teamCollection.Find(team => team.Id == fixture["teams"]["home"]["id"].AsInt32).FirstOrDefaultAsync();
               
                if (existingHomeTeam == null)
                {
                    Console.WriteLine("Entering home team");
                    hometeam.Id = fixture["teams"]["home"]["id"].AsInt32;
                    hometeam.Name = fixture["teams"]["home"]["name"].AsString;
                    Location homeLoc = new Location();
                    homeLoc.Id = hometeam.Id;
                    homeLoc.City = fixture["fixture"]["venue"]["city"].ToString();
                    homeLoc.Country = "Germany";
                    if (homeLoc.City == "Berlin" || homeLoc.City == "Bremen" || homeLoc.City == "Hamburg")
                    {
                        homeLoc.State = homeLoc.City;
                    }
                    else
                    {
                        homeLoc.State = GetRandomGermanState(germanStates);
                    }
                    hometeam.Location = homeLoc;

                    HttpClient httpClientHomePlayers = new HttpClient();
                    httpClientHomePlayers.DefaultRequestHeaders.Add("x-rapidapi-key", rapidAPIKey);
                    httpClientHomePlayers.DefaultRequestHeaders.Add("x-rapidapi-host", "api-football-v1.p.rapidapi.com");

                    var requestHomePlayers = "https://api-football-v1.p.rapidapi.com/v3/players?team=" + hometeam.Id.ToString()
                        + "&" + "season=" + fixture["league"]["season"].AsInt32.ToString();
                    var responseHomePlayers = await httpClientHomePlayers.GetAsync(requestHomePlayers);

                    if (responseHomePlayers.IsSuccessStatusCode)
                    {
                       // Console.WriteLine(responseHomePlayers.Content);
                        var content = await responseHomePlayers.Content.ReadAsStringAsync();
                        var players = BsonDocument.Parse(content);
                        var playersArray = players["response"].AsBsonArray;
                        foreach (var player in playersArray)
                        {
                            Player currentPlayer = new Player();
                            currentPlayer.TeamId = hometeam.Id;
                            currentPlayer.Id = player["player"]["id"].AsInt32;
                            currentPlayer.Age = player["player"]["age"].AsInt32;
                            currentPlayer.Name = player["player"]["firstname"].AsString;
                            currentPlayer.Last_name = player["player"]["lastname"].AsString;
                            currentPlayer.Nationality = player["player"]["nationality"].AsString;
                            homePlayers.Add(currentPlayer);
                        }

                    }
                    hometeam.Players = homePlayers;
                    Console.WriteLine("Leavving home team");
                    await teamCollection.InsertOneAsync(hometeam);
                    Console.WriteLine("Home team: \n\n");
                    Console.WriteLine(JsonSerializer.Serialize(hometeam));
                }
                else
                {
                    hometeam = existingHomeTeam;
                }
              

                var existingAwayTeam = await teamCollection.Find(team => team.Id == fixture["teams"]["away"]["id"].AsInt32).FirstOrDefaultAsync();
               
                if (existingAwayTeam == null)
                {
                    away.Id = fixture["teams"]["away"]["id"].AsInt32;
                    away.Name = fixture["teams"]["away"]["name"].AsString;
                    Location homeLoc = new Location();
                    foreach(var f in fixtures)
                    {
                        if (f["teams"]["home"]["name"].AsString == away.Name)
                        {
                            homeLoc.Id = away.Id;
                            homeLoc.City = f["fixture"]["venue"]["city"].ToString();
                            homeLoc.Country = "Germany";
                        }
                       
                    }
                   
                    if (homeLoc.City == "Berlin" || homeLoc.City == "Bremen" || homeLoc.City == "Hamburg")
                    {
                        homeLoc.State = homeLoc.City;
                    }
                    else
                    {
                        homeLoc.State = GetRandomGermanState(germanStates);
                    }
                    away.Location = homeLoc;


                    HttpClient httpClientAwayPlayers = new HttpClient();
                    httpClientAwayPlayers.DefaultRequestHeaders.Add("x-rapidapi-key", rapidAPIKey);
                    httpClientAwayPlayers.DefaultRequestHeaders.Add("x-rapidapi-host", "api-football-v1.p.rapidapi.com");

                    var requestAwayPlayers = "https://api-football-v1.p.rapidapi.com/v3/players?team=" + away.Id.ToString()
                        + "&" + "season=" + fixture["league"]["season"].AsInt32.ToString();
                    var responseAwayPlayers = await httpClientAwayPlayers.GetAsync(requestAwayPlayers);

                    if (responseAwayPlayers.IsSuccessStatusCode)
                    {
                        //Console.WriteLine(responseAwayPlayers.Content);
                        var content = await responseAwayPlayers.Content.ReadAsStringAsync();
                        var players = BsonDocument.Parse(content);
                        var playersArray = players["response"].AsBsonArray;
                        foreach (var player in playersArray)
                        {
                            Player currentPlayer = new Player();
                            currentPlayer.TeamId = hometeam.Id;
                            currentPlayer.Id = player["player"]["id"].AsInt32;
                            currentPlayer.Age = player["player"]["age"].AsInt32;
                            currentPlayer.Name = player["player"]["firstname"].AsString;
                            currentPlayer.Last_name = player["player"]["lastname"].AsString;
                            currentPlayer.Nationality = player["player"]["nationality"].AsString;

                            awayPlayers.Add(currentPlayer);
                        }

                    }
                    away.Players = awayPlayers;


                    await teamCollection.InsertOneAsync(away);

                    Console.WriteLine("Away team: \n\n");
                    Console.WriteLine(JsonSerializer.Serialize(away));
                }
                else
                {
                    
                    away = existingAwayTeam;
                }


                game.Id = fixture["fixture"]["id"].AsInt32;
                game.AwayTeam = away;
                game.HomeTeam = hometeam;
                game.Venue = venue;
                game.League = league;
                game.Date = date;
                
                await gameCollection.InsertOneAsync(game);



                HttpClient httpClientPeriods = new HttpClient();
                httpClientPeriods.DefaultRequestHeaders.Add("x-rapidapi-key", rapidAPIKey);
                httpClientPeriods.DefaultRequestHeaders.Add("x-rapidapi-host", "api-football-v1.p.rapidapi.com");

                var request = "https://api-football-v1.p.rapidapi.com/v3/fixtures/events?fixture=" + fixture["fixture"]["id"].AsInt32.ToString();
                var responsePeriod = await httpClientPeriods.GetAsync(request);

                if (responsePeriod.IsSuccessStatusCode)
                {
                   // Console.WriteLine(responsePeriod.Content);
                    var content = await responsePeriod.Content.ReadAsStringAsync();
                    var events = BsonDocument.Parse(content);

                    var eventsArray = events["response"].AsBsonArray;
                    Console.WriteLine($"\n\nEEvents:  ");
                    
                    Period firstHalfHome = new Period { Name="First HalfTime",  Team = hometeam};
                    Period secondHalfHome = new Period { Name = "Second HalfTime", Team = hometeam };
                    Period firstExtraHome = new Period { Name = "First ExtraTime", Team = hometeam };
                    Period secondExtraHome = new Period { Name = "Second ExtraTime", Team = hometeam };
                        
                    Period firstHalfAway = new Period { Name = "First HalfTime" ,  Team = away };
                    Period secondHalfAway = new Period { Name = "Second HalfTime", Team = away};
                    Period firstExtraAway = new Period { Name = "First ExtraTime", Team = away };
                    Period secondExtraAway = new Period { Name = "Second ExtraTime", Team = away};
                     
                    foreach (var eventData in eventsArray)
                    {
                        var elapsed = eventData["time"]["elapsed"].AsInt32;
                        var type = eventData["type"].AsString;
                        var detail = eventData["detail"].AsString;

                        var teamId = eventData["team"]["id"].AsInt32;

                        Console.WriteLine($"\n\nThe event :{elapsed}, {type}, {detail}, {teamId}");

                        
                        Period currentPeriod;
                        if (elapsed <= 45)
                        {
                            currentPeriod = teamId == hometeam.Id ? firstHalfHome : firstHalfAway;
                        }
                        else if (elapsed <= 90)
                        {
                            currentPeriod = teamId == hometeam.Id ? secondHalfHome : secondHalfAway;
                        }
                        else if (elapsed <= 105)
                        {
                            currentPeriod = teamId == hometeam.Id ? firstExtraHome : firstExtraAway;
                        }
                        else
                        {
                            currentPeriod = teamId == hometeam.Id ? secondExtraHome : secondExtraAway;
                        }

                        
                        switch (type)
                        {
                            case "Goal":
                                if (detail == "Own Goal")
                                {
                                    break;
                                }
                                currentPeriod.Goals++;
                               
                                if (eventData["assist"]["id"] != null)
                                {
                                    currentPeriod.Assists++;
                                }
                                break;
                            case "Card":
                                currentPeriod.Fouls++;
                                if (detail == "Yellow Card")
                                {
                                    currentPeriod.Yellow_Cards++;
                                }
                                else if (detail == "Red Card")
                                {
                                    currentPeriod.Red_Cards++;
                                }
                                break;
                            case "subst":
                                currentPeriod.Substitutions++;
                                break;
                            case "Foul":
                                currentPeriod.Fouls++;
                                break;
                            case "Var":
                                break;
                            default:
                                break;
                        }
                    }

                 
                    Console.WriteLine("\nFirst Half Home: " + JsonSerializer.Serialize(firstHalfHome));
                    Console.WriteLine("\nSecond Half Home: " + JsonSerializer.Serialize(secondHalfHome));
                    

                    Console.WriteLine("\nFirst Half Away: " + JsonSerializer.Serialize(firstHalfAway));
                    Console.WriteLine("\nSecond Half Away: " + JsonSerializer.Serialize(secondHalfAway));
                   
                    periods.Add(firstHalfHome); 
                    periods.Add(secondHalfHome);
                    periods.Add(firstHalfAway);
                    periods.Add(secondHalfAway);
                    periods.Add(firstExtraHome);
                    periods.Add(secondExtraHome);
                    periods.Add(firstExtraAway); 
                    periods.Add(secondExtraAway);  

                   

                   


                }
               // Console.WriteLine(" Game: \n\n");
               // Console.WriteLine(JsonSerializer.Serialize(game));
                gamePeriods.Periods = periods;
                gamePeriods.Game = game;
              
                
                await periodsCollection.InsertOneAsync(gamePeriods);
                
                
                
                
                stop = stop + 1;


                var playerStats = new List<PlayerStat>();
                HttpClient httpClientPlayerStats = new HttpClient();
                httpClientPlayerStats.DefaultRequestHeaders.Add("x-rapidapi-key", rapidAPIKey);
                httpClientPlayerStats.DefaultRequestHeaders.Add("x-rapidapi-host", "api-football-v1.p.rapidapi.com");

                var requestStat = "https://api-football-v1.p.rapidapi.com/v3/fixtures/players?fixture=" + fixture["fixture"]["id"].AsInt32.ToString();
                var responseStat = await httpClientPlayerStats.GetAsync(requestStat);

                if (responseStat.IsSuccessStatusCode)
                {
                    Console.WriteLine(responseStat.Content);
                    var content = await responseStat.Content.ReadAsStringAsync();
                    var stats = BsonDocument.Parse(content);

                    var playersArray = stats["response"].AsBsonArray;


                    

                    foreach (var playerData in playersArray)
                    {
                        var teamId = playerData["team"]["id"].AsInt32;
                        var teamName = playerData["team"]["name"].AsString;
                        Team curteam = new Team();
                        if(teamId == hometeam.Id)
                        {
                            curteam = hometeam;
                        }
                        else if (teamId == away.Id)
                        {
                            curteam = away;
                        }

                        Console.WriteLine($"\n\nTeam:   {JsonSerializer.Serialize(curteam)}   \n\n");


                        foreach (var playerStatData in playerData["players"].AsBsonArray)
                        {
                            var playerId = playerStatData["player"]["id"].AsInt32;
                            Console.WriteLine($"\n\nId:   {playerId}   \n\n");
                            var playerName = playerStatData["player"]["name"].AsString;
                            var plList = curteam.Players;
                            //var player  =  curteam.Players.Find(p => p.Id == playerId);
                            var player = plList.Find(p => p.Id == playerId);
                            Thread.Sleep(1000);
                            if (player == null)
                            {
                                Console.WriteLine("Entering finding player");
                                HttpClient httpClientPlayers = new HttpClient();
                                httpClientPlayers.DefaultRequestHeaders.Add("x-rapidapi-key", rapidAPIKey);
                                httpClientPlayers.DefaultRequestHeaders.Add("x-rapidapi-host", "api-football-v1.p.rapidapi.com");

                                var requestPlayers = "https://api-football-v1.p.rapidapi.com/v3/players?id=" + playerId.ToString() + "&season=" +
                                    game.League.Season.ToString();
                                   
                                var responsePlayers = await httpClientPlayers.GetAsync(requestPlayers);

                                if (responsePlayers.IsSuccessStatusCode)
                                {
                                    
                                    var contentPlayer = await responsePlayers.Content.ReadAsStringAsync();
                                    var players = BsonDocument.Parse(contentPlayer); 

                                    
                                    if (players.Contains("response"))
                                    {
                                        var playerInResponse = players["response"][0]; 

                                        Player currentPlayer = new Player();
                                        currentPlayer.TeamId = curteam.Id;
                                        currentPlayer.Id = playerInResponse["player"]["id"].AsInt32;
                                        currentPlayer.Age = playerInResponse["player"]["age"].AsInt32;
                                        currentPlayer.Name = playerInResponse["player"]["firstname"].AsString;
                                        currentPlayer.Last_name = playerInResponse["player"]["lastname"].AsString;
                                        currentPlayer.Nationality = playerInResponse["player"]["nationality"].AsString;
                                        player = currentPlayer;


                                        var filterTeam = Builders<Team>.Filter.Eq(t => t.Id, curteam.Id);
                                        var team = await teamCollection.Find(filterTeam).FirstOrDefaultAsync();

                                        if (team != null)
                                        {
                                           
                                            team.Players.Add(currentPlayer);

                                            var update = Builders<Team>.Update.Set(t => t.Players, team.Players);
                                            await teamCollection.UpdateOneAsync(filterTeam, update);
                                            Console.WriteLine($"\n\nPlayer added successfully:{JsonSerializer.Serialize(currentPlayer)}");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Team not found.");
                                        }
                                    }
                                    

                                }

                            }
                            Thread.Sleep(1000);

                            int minutesPlayed = 0;

                            var gamesData = playerStatData["statistics"][0]["games"];
                            if (!gamesData["minutes"].IsBsonNull)
                            {
                                minutesPlayed = gamesData["minutes"].AsInt32;
                            }

                            

                            var goalsData = playerStatData["statistics"][0]["goals"];
                            int goalsScored = 0;
                            int assists = 0;
                            int foulsCommitted = 0;
                            if (!goalsData["total"].IsBsonNull)
                            {
                                goalsScored = goalsData["total"].AsInt32;
                               
                            }


                            if (!goalsData["assists"].IsBsonNull)
                            {
                                
                                assists = goalsData["assists"].AsInt32;
                            }

                            var foulsData = playerStatData["statistics"][0]["fouls"];
                            if (!foulsData["committed"].IsBsonNull)
                            {
                                foulsCommitted = foulsData["committed"].AsInt32;
                            }
                           

                            playerStats.Add(new PlayerStat
                            {
                                Player =player,
                                Date = date, 
                                Assists = assists,
                                Goals = goalsScored,
                                Fouls = foulsCommitted,
                                Time = minutesPlayed
                            });
                        }
                    }





                }

                gamePlayer.playerStats = playerStats;
                gamePlayer.Game = game;


                await playerStatCollection.InsertOneAsync(gamePlayer);


                Console.WriteLine("\n\nGamePlayers:   \n\n");
                Console.WriteLine(JsonSerializer.Serialize(gamePlayer));

                bool existingInBets = false;
                foreach(var b in betsList)
                {
                    if(b.FixtureData.Id == game.Id)
                    {
                        existingInBets = true;
                        List<Revenue> myRevenues = new List<Revenue>();
                        foreach(var option in b.BetsOptions)
                        {
                            Revenue rev  = new Revenue();
                            if(option.NumberOfBets>0)
                            {
                                rev.Home = hometeam;
                                rev.Away = away;
                                rev.Date = date;
                                int homeScore2 = fixture["score"]["fulltime"]["home"].AsInt32;
                                int awayScore2 = fixture["score"]["fulltime"]["away"].AsInt32;
                                string result2 = "";
                                if (homeScore2 > awayScore2)
                                {
                                    result2 = "Home win: " + homeScore2.ToString() + "-" + awayScore2.ToString();

                                }
                                else if (homeScore2 < awayScore2)
                                {
                                    result2 = "Away win: " + homeScore2.ToString() + "-" + awayScore2.ToString();
                                }
                                else if (homeScore2 == awayScore2)
                                {
                                    result2 = "Draw: " + homeScore2.ToString() + "-" + awayScore2.ToString();
                                }
                                rev.Result = option.Name;
                                rev.Income = option.Income;
                                rev.Number_of_bets = option.NumberOfBets;
                                rev.Coef = option.Coefficient;
                                myRevenues.Add(rev);

                            }else if (option.NumberOfBets==0|| option.NumberOfBets < 0) 
                            {
                                rev.Home = hometeam;
                                rev.Away = away;
                                rev.Date = date;
                                int homeScore2 = fixture["score"]["fulltime"]["home"].AsInt32;
                                int awayScore2 = fixture["score"]["fulltime"]["away"].AsInt32;
                                string result2 = "";
                                if (homeScore2 > awayScore2)
                                {
                                    result2 = "Home win: " + homeScore2.ToString() + "-" + awayScore2.ToString();

                                }
                                else if (homeScore2 < awayScore2)
                                {
                                    result2 = "Away win: " + homeScore2.ToString() + "-" + awayScore2.ToString();
                                }
                                else if (homeScore2 == awayScore2)
                                {
                                    result2 = "Draw: " + homeScore2.ToString() + "-" + awayScore2.ToString();
                                }
                                rev.Result = option.Name;
                                Random randRev2 = new Random();
                                rev.Income = randRev2.Next(10000, 300000);
                                rev.Number_of_bets = randRev2.Next(1000, 10000);
                                rev.Coef = 1 + randRev2.NextDouble();
                                myRevenues.Add(rev);

                            }
                        }
                        gameRevenue.Game = game;
                        gameRevenue.revenues = myRevenues;
                    }
                }

                if (!existingInBets)
                {
                    List<Revenue> revenues = new List<Revenue>();
                    revenue.Home = hometeam;
                    revenue.Away = away;
                    revenue.Date = date;
                    //revenue.Game = game;
                    int homeScore = fixture["score"]["fulltime"]["home"].AsInt32;
                    int awayScore = fixture["score"]["fulltime"]["away"].AsInt32;
                    string result = "";
                    if (homeScore > awayScore)
                    {
                        result = "Home win: " + homeScore.ToString() + "-" + awayScore.ToString();

                    }
                    else if (homeScore < awayScore)
                    {
                        result = "Away win: " + homeScore.ToString() + "-" + awayScore.ToString();
                    }
                    else if (homeScore == awayScore)
                    {
                        result = "Draw: " + homeScore.ToString() + "-" + awayScore.ToString();
                    }

                    revenue.Result = result;
                    Random randRev = new Random();
                    revenue.Income = randRev.Next(10000, 300000);
                    revenue.Number_of_bets = randRev.Next(1000, 10000);
                    revenue.Coef = 1 + randRev.NextDouble();
                    revenues.Add(revenue);
                    List<string> scoresList = new List<string>();
                    scoresList.Add(homeScore.ToString() + "-" + awayScore.ToString());
                    int[] homeGoals = new int[14] { 0, 1, 0, 1, 2, 0, 2, 1, 2, 3, 0, 3, 3, 2 };
                    int[] awayGoals = new int[14] { 0, 0, 1, 1, 0, 2, 1, 2, 2, 0, 3, 3, 2, 3 };
                    Dictionary<int, int> resultsKeys = new Dictionary<int, int>();

                    for (int i = 0; i < 14; i++)
                    {
                        if (homeGoals[i] == homeScore && awayGoals[i] == awayScore)
                        {
                            continue;
                        }
                        Revenue current = new Revenue();
                        Random randCurrent = new Random();
                        current.Home = hometeam;
                        current.Away = away;
                        current.Date = date;

                        int homeNewScore = homeGoals[i];
                        int awayNewScore = awayGoals[i];
                        string resultNew = "";
                        string resultScore = homeNewScore.ToString() + "-" + awayNewScore.ToString();




                        if (homeNewScore > awayNewScore)
                        {
                            resultNew = "Home win: " + resultScore;

                        }
                        else if (homeNewScore < awayNewScore)
                        {
                            resultNew = "Away win: " + resultScore;
                        }
                        else if (homeNewScore == awayNewScore)
                        {
                            resultNew = "Draw: " + resultScore;
                        }

                        current.Result = resultNew;

                        current.Income = randCurrent.Next(10000, 300000);
                        current.Number_of_bets = randRev.Next(1000, 10000);
                        current.Coef = 1 + randRev.NextDouble();
                        revenues.Add(current);
                    }
                    List<string> someres = new List<string>();
                    foreach (var rev in revenues)
                    {
                        someres.Add($"{rev.Result}  :  {rev.Number_of_bets}   : {rev.Coef}");
                    }

                    Console.WriteLine("\n\nRevenue:   \n\n");
                    Console.WriteLine(JsonSerializer.Serialize(someres));
                    gameRevenue.Game = game;
                    gameRevenue.revenues = revenues;
                }
                
                
                await revenueCollection.InsertOneAsync(gameRevenue);




            }

        
        }
        static string GetRandomGermanState(List<string> states)
        {
            Random rand = new Random();
            int index = rand.Next(states.Count);
            return states[index];
        }

    }
}
