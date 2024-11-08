using ETLPostgres.ModelsFromMongo;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;
using ETLPostgres.Models;
using Microsoft.EntityFrameworkCore;
using System;


namespace ETLPostgres
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //Console.WriteLine(args);
            FootballContext footballContext = new FootballContext();
            string mongoConnectionString = "mongodb+srv://vladyslavyakymchukpz2021:w9s3mP0ZGVWSvDwN@cluster0.zjmnvay.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";
            string databaseName = "Football";


            int errorId = 0;
            await Console.Out.WriteLineAsync("Entering db");
            var mongoClient = new MongoClient(mongoConnectionString);
            var database = mongoClient.GetDatabase(databaseName);
            await Console.Out.WriteLineAsync("Entering collections");
            var collection = database.GetCollection<BsonDocument>("Fixtures");
            var teamCollection = database.GetCollection<TeamMongo>("Team");
            var gameCollection = database.GetCollection<GameMongo>("Games");
            var playerStatCollection = database.GetCollection<GamePlayerMongo>("PlayerStat");
            var revenueCollection = database.GetCollection<GameRevenueMongo>("Revenue");
            var periodsCollection = database.GetCollection<GamePeriodsMongo>("GamePeriods");



            var filter = Builders<BsonDocument>.Filter.Eq("league.season", 2023);

            var fixturesFiltered = await collection.FindAsync(filter);
            var fixtures = await fixturesFiltered.ToListAsync();




            var filterGames = Builders<GameMongo>.Filter.Empty;


            var gamesFiltered = await gameCollection.FindAsync(filterGames);
            var gamesList = await gamesFiltered.ToListAsync();



            var filterTeam = Builders<TeamMongo>.Filter.Empty;


            var teamsFiltered = await teamCollection.FindAsync(filterTeam);
            var teamsList = await teamsFiltered.ToListAsync();


            var filterGamesPlayers = Builders<GamePlayerMongo>.Filter.Empty;


            var gamePlayersFiltered = await playerStatCollection.FindAsync(filterGamesPlayers);
            var gamesPlayersList = await gamePlayersFiltered.ToListAsync();



            var filterrevenue = Builders<GameRevenueMongo>.Filter.Empty;


            var gamesRevenuesFiltered = await revenueCollection.FindAsync(filterrevenue);
            var revenuesList = await gamesRevenuesFiltered.ToListAsync();



            var filterPeriods = Builders<GamePeriodsMongo>.Filter.Empty;


            var periodsFiltered = await periodsCollection.FindAsync(filterPeriods);
            var periodsList = await periodsFiltered.ToListAsync();


            var rapidAPIKey = "fc517f3f59msh0003d703ec5f8c8p1b96a2jsn532839b11888";
            var headers = new Dictionary<string, string>
                          {
                              { "x-rapidapi-key", rapidAPIKey },
                              { "x-rapidapi-host", "api-football-v1.p.rapidapi.com" }
                          };

            List<string> collectionNewNames = new List<string> { "Games", "PlayerStat", "Team", "Revenue" };
            var collectionNames = await database.ListCollectionNamesAsync();
            var collectionList = collectionNames.ToList();


            List<long> fixtureIds = new List<long>();
            List<long> fixtureFactIds = new List<long>();
            foreach (var fixture in fixtures)
            {
                fixtureIds.Add(fixture["fixture"]["id"].AsInt32);

            }
            foreach (var g in gamesList)
            {
                fixtureFactIds.Add(g.Id);
            }
            if(footballContext.Periods.ToArray().Length == 0)
            {
                Period p1 = new Period()
                {
                    Id =1,
                    PeriodName = "First halftime"
                };
                Period p2 = new Period()
                {
                    Id = 2,
                    PeriodName = "Second halftime"
                };
                Period p3 = new Period()
                {
                    Id = 3,
                    PeriodName = "First extratime"
                };
                Period p4 = new Period()
                {
                    Id = 4,
                    PeriodName = "Second extratime"
                };
                footballContext.Periods.Add(p1);
                footballContext.Periods.Add(p2);
                footballContext.Periods.Add(p3);
                footballContext.Periods.Add(p4);
                await footballContext.SaveChangesAsync();
            }
            var allGames = await footballContext.Games.ToListAsync();


           
                    foreach (var g in gamesList)
                    {
                        errorId = g.Id;
                        bool existing = false;
                        foreach (var gam in allGames)
                        {
                            if (gam.Id == g.Id)
                            {
                                Console.WriteLine($"Game: {gam.Id} is already proccessed");
                                existing = true;
                                break;
                            }

                        }
                        if (existing)
                        {
                            continue;
                        }
                        Date date = new Date();
                        Game game = new Game();
                        GameResult gameResult = new GameResult();
                        BetResult betResult = new BetResult();
                        League league = new League();
                        Location homeLocation = new Location();
                        Location awayLocation = new Location();
                        Period period = new Period();
                        Player player = new Player();
                        Result result = new Result();
                        Team homeTeam = new Team();
                        Team awayTeam = new Team();
                        Venue venue = new Venue();
                        List<PlayerMongo> playersAll = new List<PlayerMongo>();


                        var existingVenue = await footballContext.Venues.FindAsync(g.Venue.Id);
                        if (existingVenue != null)
                        {
                            venue = existingVenue;

                        }
                        else if(existingVenue!=null&&existingVenue.Name != g.Venue.Name)
                        {
                            venue.Id = g.Venue.Id;
                            venue.Name = g.Venue.Name;
                            venue.City = g.Venue.City;
                            footballContext.Venues.Update(venue);

                        }
                        else
                        {
                            venue.Id = g.Venue.Id;
                            venue.City = g.Venue.City;
                            venue.Name = g.Venue.Name;
                            await footballContext.Venues.AddAsync(venue);
                            await footballContext.SaveChangesAsync();

                        }


                        var existingLeague = await footballContext.Leagues.FirstOrDefaultAsync(l => (l.Round == g.League.Round&& l.Season == g.League.Season));
                        if (existingLeague != null)
                        {
                            league = existingLeague;
                        }
                        else if (existingLeague != null && existingLeague.Name != g.League.Name)
                        {
                            league.Id = existingLeague.Id;
                            league.Name = g.League.Name;
                            league.Season = g.League.Season;
                            league.Round = g.League.Round;
                            footballContext.Leagues.Update(league);

                        }
                        else
                        {
                            var leaguesList = await footballContext.Leagues.ToListAsync();
                            league.Id = leaguesList.Count + 1;
                            league.Name = g.League.Name;
                            league.Season = g.League.Season;
                            league.Round = g.League.Round;
                            await footballContext.Leagues.AddAsync(league);
                            await footballContext.SaveChangesAsync();

                        }
                        DateTime gameDate = DateTime.Parse(g.Date.ToString());
                        DateOnly dateOnly = DateOnly.FromDateTime(gameDate);
                        var existingDate = await footballContext.Dates.FirstOrDefaultAsync(d => d.Day == dateOnly);
                        if (existingDate != null)
                        {
                            date = existingDate;
                        }
                        else
                        {

                            date.Year = gameDate.Year;
                            date.Month = gameDate.Month;
                            date.DayOfWeek = gameDate.ToString("dddd");

                            date.Day = DateOnly.FromDateTime(gameDate);
                            await footballContext.Dates.AddAsync(date);
                            await footballContext.SaveChangesAsync();
                        }



                        var existingHomeLoc = await footballContext.Locations.FirstOrDefaultAsync(hl => hl.City == g.HomeTeam.Location.City);
                        if (existingHomeLoc != null)
                        {
                            homeLocation = existingHomeLoc;
                        }
                        else
                        {
                            homeLocation.City = g.HomeTeam.Location.City;
                            homeLocation.Country = g.HomeTeam.Location.Country;
                            homeLocation.State = g.HomeTeam.Location.State;
                            await footballContext.Locations.AddAsync(homeLocation);
                            await footballContext.SaveChangesAsync();
                        }


                        var existingAwayLoc = await footballContext.Locations.FirstOrDefaultAsync(al => al.City == g.AwayTeam.Location.City);
                        if (existingAwayLoc != null)
                        {
                            awayLocation = existingAwayLoc;
                        }
                        else
                        {
                            awayLocation.City = g.AwayTeam.Location.City;
                            awayLocation.Country = g.AwayTeam.Location.Country;
                            awayLocation.State = g.AwayTeam.Location.State;
                            await footballContext.Locations.AddAsync(awayLocation);
                            await footballContext.SaveChangesAsync();
                        }

                        /* foreach(var hplayer in g.HomeTeam.Players)
                         {
                             playersAll.Add(hplayer);
                         }

                         foreach (var aplayer in g.AwayTeam.Players)
                         {
                             playersAll.Add(aplayer);
                         }*/

                        var existingHomeTeam = await footballContext.Teams.FindAsync(g.HomeTeam.Id);
                        if (existingHomeTeam == null)
                        {
                            homeTeam.Id = g.HomeTeam.Id;
                            var homeLoc = await footballContext.Locations.FirstOrDefaultAsync(hl => hl.City == g.HomeTeam.Location.City);
                            homeTeam.LocationId = homeLoc.Id;
                            homeTeam.Name = g.HomeTeam.Name;
                            await footballContext.Teams.AddAsync(homeTeam);
                            await footballContext.SaveChangesAsync();

                        }else if(existingHomeTeam !=null && existingHomeTeam.Name != g.HomeTeam.Name)
                        {
                            homeTeam.Id = existingHomeTeam.Id;
                            var homeLoc = await footballContext.Locations.FirstOrDefaultAsync(hl => hl.City == g.HomeTeam.Location.City);
                            homeTeam.LocationId = homeLoc.Id;
                            homeTeam.Name = g.HomeTeam.Name;
                            footballContext.Teams.Update(homeTeam);
                        }
                        else
                        {
                            homeTeam = existingHomeTeam;
                        }

                        var existingAwayTeam = await footballContext.Teams.FindAsync(g.AwayTeam.Id);
                        if (existingAwayTeam == null)
                        {
                            awayTeam.Id = g.AwayTeam.Id;
                            var awayLoc = await footballContext.Locations.FirstOrDefaultAsync(hl => hl.City == g.AwayTeam.Location.City);
                            awayTeam.LocationId = awayLoc.Id;
                            awayTeam.Name = g.AwayTeam.Name;
                            await footballContext.Teams.AddAsync(awayTeam);
                            await footballContext.SaveChangesAsync();

                        }
                        else
                        {
                            awayTeam = existingAwayTeam;
                        }

                        TeamMongo homeColTeam = new TeamMongo();
                        TeamMongo awayColTeam = new TeamMongo();
                        foreach (var team in teamsList)
                        {
                            if (team.Id == g.HomeTeam.Id)
                            {
                                homeColTeam = team;

                            }
                            else if (team.Id == g.AwayTeam.Id)
                            {
                                awayColTeam = team;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        foreach (var hPl in homeColTeam.Players)
                        {
                            playersAll.Add(hPl);
                        }
                        foreach (var aPl in awayColTeam.Players)
                        {
                            playersAll.Add(aPl);
                        }



                        foreach (var pl in playersAll)
                        {
                            var existingPlayer = footballContext.Players.Find(pl.Id);
                            if (existingPlayer != null)
                            {

                                continue;
                            }
                            else if (existingPlayer != null && existingPlayer.Age != pl.Age&&gameDate.Year>=DateTime.Now.Year)
                            {
                                player.Id = pl.Id;
                                player.FirstName = pl.Name;
                                player.LastName = pl.Last_name;
                                player.Age = pl.Age;
                                player.Nationality = pl.Nationality;
                                player.TeamId = pl.TeamId;
                                footballContext.Players.Update(player);
                                await footballContext.SaveChangesAsync();   
                                Console.WriteLine("Player updated succesfully");

                            }
                            else
                            {
                                player.Id = pl.Id;
                                player.FirstName = pl.Name;
                                player.LastName = pl.Last_name;
                                player.Age = pl.Age;
                                player.Nationality = pl.Nationality;
                                player.TeamId = pl.TeamId;
                                await footballContext.Players.AddAsync(player);
                                await footballContext.SaveChangesAsync();

                            }

                        }

                        var gDate = await footballContext.Dates.FirstOrDefaultAsync(d => d.Day == DateOnly.FromDateTime(gameDate));
                        var dateId = gDate.Id;

                        var gLeague = await footballContext.Leagues.FirstOrDefaultAsync(l => l.Round == g.League.Round);
                        var leagueId = gLeague.Id;


                        var gHomeTeam = await footballContext.Teams.FindAsync(g.HomeTeam.Id);
                        var homeId = gHomeTeam.Id;


                        var gAwayTeam = await footballContext.Teams.FindAsync(g.AwayTeam.Id);
                        var awayId = gAwayTeam.Id;

                        var gVenue = await footballContext.Venues.FindAsync(g.Venue.Id);
                        var venueId = gVenue.Id;


                        game.Id = g.Id;
                        game.VenueId = venueId;
                        game.LeagueId = leagueId;
                        game.HomeTeamId = homeId;
                        game.AwayTeamId = awayId;
                        game.DateId = dateId;
                        await footballContext.Games.AddAsync(game);
                        await footballContext.SaveChangesAsync();


                        foreach (var gamePeriods in periodsList)
                        {

                            if (gamePeriods.Game.Id != g.Id)
                            {
                                continue;
                            }
                            else
                            {
                                foreach (var p in gamePeriods.Periods)
                                {
                                    Period newPeriod = new Period();
                                    string namePeriod = p.Name;
                                    string[] pArr = namePeriod.Split(" ");
                                    string timeName = "";
                                    if (pArr[1] == "HalfTime")
                                    {
                                        timeName = " halftime";
                                    }
                                    else if (pArr[1] == "ExtraTime")
                                    {
                                        timeName = " extratime";
                                    }
                                    var existingPeriod = await footballContext.Periods.FirstOrDefaultAsync(pe => pe.PeriodName == pArr[0] + timeName);
                                    if (existingPeriod != null)
                                    {
                                        newPeriod = existingPeriod;
                                        
                                    }
                                    else
                                    {

                                        newPeriod.PeriodName = pArr[0] + " halftime";
                                        await footballContext.Periods.AddAsync(newPeriod);
                                        //Console.WriteLine($"Added period:  {JsonSerializer.Serialize(footballContext.Periods.ToArray())}");
                                        await footballContext.SaveChangesAsync();
                                    }

                                    var gPeriod = await footballContext.Periods.FirstOrDefaultAsync(per => per.PeriodName == pArr[0] + timeName);
                                    var periodId = gPeriod.Id;
                                    gameResult.TeamId = p.Team.Id;
                                    gameResult.DateId = dateId;
                                    gameResult.PeriodId = periodId;
                                    gameResult.GameId = game.Id;
                                    gameResult.Goals = p.Goals;
                                    gameResult.Assists = p.Assists;
                                    gameResult.Fouls = p.Fouls;
                                    gameResult.RedCards = p.Red_Cards;
                                    gameResult.YellowCards = p.Yellow_Cards;
                                    gameResult.Substitutions = p.Substitutions;
                                    await footballContext.GameResults.AddAsync(gameResult);
                                    await footballContext.SaveChangesAsync();
                                   // Console.WriteLine("Inserted game rsult fact");


                                }
                            }


                        }


                        foreach (var plStat in gamesPlayersList)
                        {

                            if (plStat.Game.Id == game.Id)
                            {
                                foreach (var playerRes in plStat.playerStats)
                                {
                                    PlayerResult playerResult = new PlayerResult();
                                    playerResult.DateId = dateId;
                                    playerResult.PlayerId = playerRes.Player.Id;
                                    playerResult.GameId = game.Id;
                                    playerResult.Goals = playerRes.Goals;
                                    playerResult.Assists = playerRes.Assists;
                                    playerResult.Fouls = playerRes.Fouls;
                                    playerResult.Time = playerRes.Time;
                                    await footballContext.PlayerResults.AddAsync(playerResult);
                                    await footballContext.SaveChangesAsync();
                                   // Console.WriteLine("Inserted player rsult fact");
                                }
                            }

                        }

                        BsonDocument myFixture = new BsonDocument();
                        foreach (var fixture in fixtures)
                        {
                            if (fixture["fixture"]["id"].AsInt32 == g.Id)
                            {
                                myFixture = fixture;
                            }
                        }

                        int homeScore = myFixture["score"]["fulltime"]["home"].AsInt32;
                        int awayScore = myFixture["score"]["fulltime"]["away"].AsInt32;
                        string revResult = "";
                        if (homeScore > awayScore)
                        {
                            revResult = "Home win: " + homeScore.ToString() + "-" + awayScore.ToString();

                        }
                        else if (homeScore < awayScore)
                        {
                            revResult = "Away win: " + homeScore.ToString() + "-" + awayScore.ToString();
                        }
                        else if (homeScore == awayScore)
                        {
                            revResult = "Draw: " + homeScore.ToString() + "-" + awayScore.ToString();
                        }

                        foreach (var gameRevenue in revenuesList)
                        {
                            if (gameRevenue.Game.Id == g.Id)
                            {

                                foreach (var betRes in gameRevenue.revenues)
                                {
                                    BetResult betResult1 = new BetResult();
                                    Result result1 = new Result();
                                    int trueRes = 0;
                                    if (betRes.Result == revResult)
                                    {
                                        trueRes = 1;
                                    }

                                    string fullResult = betRes.Result.ToString();
                                    string[] arrRes = fullResult.Split(": ");

                                    var existingScore = await footballContext.Results.FirstOrDefaultAsync(sc => sc.Score == arrRes[1] && sc.Name == arrRes[0]);
                                    if (existingScore != null)
                                    {
                                        result1 = existingScore;
                                    }
                                    else
                                    {
                                        //  var resList = await footballContext.Results.ToListAsync();
                                        //  result.Id = resList.Count + 1;
                                        result1.Name = arrRes[0];
                                        result1.Score = arrRes[1];
                                        await footballContext.Results.AddAsync(result1);
                                        await footballContext.SaveChangesAsync();

                                    }

                                    var gRes = await footballContext.Results.FirstOrDefaultAsync(sc => sc.Score == arrRes[1] && sc.Name == arrRes[0]);
                                    var resId = gRes.Id;
                                    
                                    betResult1.AwayTeamId = awayId;
                                    betResult1.HomeTeamId = homeId;
                                    betResult1.GameId = game.Id;
                                    betResult1.ResultId = resId;
                                    betResult1.DateId = dateId;
                                    betResult1.NumberOfBets = betRes.Number_of_bets;
                                    betResult1.Income = betRes.Income;
                                    double betOutcome = betRes.Income * betRes.Coef * trueRes;
                                    betResult1.Outcome = (int)betOutcome;
                                    betResult1.Coef = Math.Round(betRes.Coef, 2, MidpointRounding.ToEven);
                                    betResult1.BetResult1 = trueRes;
                                    await footballContext.BetResults.AddAsync(betResult1);
                                    await footballContext.SaveChangesAsync();
                                   // Console.WriteLine("Inserted bet result fact");



                                }

                            }
                   
                        }

                        Console.WriteLine("Game was proccesed");


                    }
        }
    }
}
