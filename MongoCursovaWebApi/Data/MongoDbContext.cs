using Microsoft.VisualBasic;
using MongoCursovaWebApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Npgsql;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace MongoCursovaWebApi.Data
{
    public class MongoDbContext
    {
        string mongoConnectionString = "mongodb+srv://vladyslavyakymchukpz2021:w9s3mP0ZGVWSvDwN@cluster0.zjmnvay.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";
        string databaseName = "Football";
        MongoClient mongoClient;
        IMongoDatabase database;
        IMongoCollection<BsonDocument> collection;
        IMongoCollection<Team> teamCollection;
        List<FixtureData> fixtureDatas = new List<FixtureData>();
        IMongoCollection<Bets> betsCollection;
        IMongoCollection<BsonDocument> revenueCollection;

        public MongoDbContext()
        {
            mongoClient = new MongoClient(mongoConnectionString);
            database = mongoClient.GetDatabase(databaseName);
            collection = database.GetCollection<BsonDocument>("Fixtures");
            teamCollection = database.GetCollection<Team>("Team");
            betsCollection = database.GetCollection<Bets>("Bets");
            revenueCollection = database.GetCollection<BsonDocument>("Revenue");
            Trace.WriteLine("Connected to db");
           
        } 

        public async Task<List<FixtureData>> fillData()
        {
            List<FixtureData> fixturesData = new List<FixtureData>();
            var filter = Builders<BsonDocument>.Filter.Eq("league.season", 2023);
            var filter2 = Builders<BsonDocument>.Filter.Eq("fixture.status.short", "NS");
            var fixturesFiltered = await collection.FindAsync(filter2);




            var fixtures = await fixturesFiltered.ToListAsync();
            foreach (var fixture in fixtures) { 
            
                FixtureData fixtureData = new FixtureData();
                fixtureData.Id = fixture["fixture"]["id"].AsInt32;
                fixtureData.City =  fixture["fixture"]["venue"]["city"].AsString;
                fixtureData.Stadium = fixture["fixture"]["venue"]["name"].AsString;
                fixtureData.Round = fixture["league"]["round"].AsString;
                fixtureData.HomeName = fixture["teams"]["home"]["name"].AsString;
                fixtureData.AwayName = fixture["teams"]["away"]["name"].AsString;
                fixtureData.HomeLogo = fixture["teams"]["home"]["logo"].AsString;
                fixtureData.AwayLogo = fixture["teams"]["away"]["logo"].AsString;
                fixtureData.Date = fixture["fixture"]["date"].AsString;


                fixturesData.Add(fixtureData);

            }
            fixtureDatas = fixturesData;
            return fixturesData;
            
        }

        public async Task<List<FixtureData>> getFixturesByTeamMonth(int month, string teamName)
        {
            List<FixtureData> fixturesData = new List<FixtureData>();
            var filter = Builders<BsonDocument>.Filter.Eq("league.season", 2023);
            var filter2 = Builders<BsonDocument>.Filter.Eq("fixture.status.short", "NS");
            var fixturesFiltered = await collection.FindAsync(filter2);




            var fixtures = await fixturesFiltered.ToListAsync();
            foreach (var fixture in fixtures)
            {
                DateTime fdate = DateTime.Parse(fixture["fixture"]["date"].AsString);
               string homeName=  fixture["teams"]["home"]["name"].AsString;
               string away_name = fixture["teams"]["away"]["name"].AsString;

                if (fdate.Month == month && (teamName == homeName|| teamName == away_name))
                {
                    FixtureData fixtureData = new FixtureData();
                    fixtureData.Id = fixture["fixture"]["id"].AsInt32;
                    fixtureData.City = fixture["fixture"]["venue"]["city"].AsString;
                    fixtureData.Stadium = fixture["fixture"]["venue"]["name"].AsString;
                    fixtureData.Round = fixture["league"]["round"].AsString;
                    fixtureData.HomeName = fixture["teams"]["home"]["name"].AsString;
                    fixtureData.AwayName = fixture["teams"]["away"]["name"].AsString;
                    fixtureData.HomeLogo = fixture["teams"]["home"]["logo"].AsString;
                    fixtureData.AwayLogo = fixture["teams"]["away"]["logo"].AsString;
                    fixtureData.Date = fixture["fixture"]["date"].AsString;


                    fixturesData.Add(fixtureData);
                }


            }

            return fixturesData;

        }


        public async Task<List<FixtureData>> getFixturesByTeam( string teamName)
        {

            List<FixtureData> fixturesData = new List<FixtureData>();
            var filter = Builders<BsonDocument>.Filter.Eq("league.season", 2023);
            var filter2 = Builders<BsonDocument>.Filter.Eq("fixture.status.short", "NS");
            var fixturesFiltered = await collection.FindAsync(filter2);





            var fixtures = await fixturesFiltered.ToListAsync();
            foreach (var fixture in fixtures)
            {
                DateTime fdate = DateTime.Parse(fixture["fixture"]["date"].AsString);
                string homeName = fixture["teams"]["home"]["name"].AsString;
                string away_name = fixture["teams"]["away"]["name"].AsString;

                if (teamName == homeName || teamName == away_name)
                {
                    FixtureData fixtureData = new FixtureData();
                    fixtureData.Id = fixture["fixture"]["id"].AsInt32;
                    fixtureData.City = fixture["fixture"]["venue"]["city"].AsString;
                    fixtureData.Stadium = fixture["fixture"]["venue"]["name"].AsString;
                    fixtureData.Round = fixture["league"]["round"].AsString;
                    fixtureData.HomeName = fixture["teams"]["home"]["name"].AsString;
                    fixtureData.AwayName = fixture["teams"]["away"]["name"].AsString;
                    fixtureData.HomeLogo = fixture["teams"]["home"]["logo"].AsString;
                    fixtureData.AwayLogo = fixture["teams"]["away"]["logo"].AsString;
                    fixtureData.Date = fixture["fixture"]["date"].AsString;


                    fixturesData.Add(fixtureData);
                }


            }

            return fixturesData;

        }

        public async Task<List<FixtureData>> getFixturesByMonth(int month)
        {

            List<FixtureData> fixturesData = new List<FixtureData>();
            var filter = Builders<BsonDocument>.Filter.Eq("league.season", 2023);
            var filter2 = Builders<BsonDocument>.Filter.Eq("fixture.status.short", "NS");
            var fixturesFiltered = await collection.FindAsync(filter2);





            var fixtures = await fixturesFiltered.ToListAsync();
            foreach (var fixture in fixtures)
            {
                DateTime fdate = DateTime.Parse(fixture["fixture"]["date"].AsString);
                string homeName = fixture["teams"]["home"]["name"].AsString;
                string away_name = fixture["teams"]["away"]["name"].AsString;

                if (fdate.Month == month)
                {
                    FixtureData fixtureData = new FixtureData();
                    fixtureData.Id = fixture["fixture"]["id"].AsInt32;
                    fixtureData.City = fixture["fixture"]["venue"]["city"].AsString;
                    fixtureData.Stadium = fixture["fixture"]["venue"]["name"].AsString;
                    fixtureData.Round = fixture["league"]["round"].AsString;
                    fixtureData.HomeName = fixture["teams"]["home"]["name"].AsString;
                    fixtureData.AwayName = fixture["teams"]["away"]["name"].AsString;
                    fixtureData.HomeLogo = fixture["teams"]["home"]["logo"].AsString;
                    fixtureData.AwayLogo = fixture["teams"]["away"]["logo"].AsString;
                    fixtureData.Date = fixture["fixture"]["date"].AsString;


                    fixturesData.Add(fixtureData);
                }


            }

            return fixturesData;

        }

        public async Task<List<Bets>> fillBets()
        {
            List<FixtureData> fixturesData = new List<FixtureData>();
            
            var filter = Builders<BsonDocument>.Filter.Eq("league.season", 2023);
            var filter2 = Builders<BsonDocument>.Filter.Eq("fixture.status.short", "NS");
            var fixturesFiltered = await collection.FindAsync(filter2);
            
            var filterBets = Builders<Bets>.Filter.Empty;
            var betsFiltered = await betsCollection.FindAsync(filterBets);

            var betsList = await betsFiltered.ToListAsync();

            List<Bets> bets2 = new List<Bets>();






            var fixtures = await fixturesFiltered.ToListAsync();
            foreach (var fixture in fixtures)
            {
                Bets bets1 = new Bets();
                List<Bet> bets = new List<Bet>();
                FixtureData fixtureData = new FixtureData();
                fixtureData.Id = fixture["fixture"]["id"].AsInt32;
                fixtureData.City = fixture["fixture"]["venue"]["city"].AsString;
                fixtureData.Stadium = fixture["fixture"]["venue"]["name"].AsString;
                fixtureData.Round = fixture["league"]["round"].AsString;
                fixtureData.HomeName = fixture["teams"]["home"]["name"].AsString;
                fixtureData.AwayName = fixture["teams"]["away"]["name"].AsString;
                fixtureData.HomeLogo = fixture["teams"]["home"]["logo"].AsString;
                fixtureData.AwayLogo = fixture["teams"]["away"]["logo"].AsString;
                fixtureData.Date = fixture["fixture"]["date"].AsString;
                List<string> results = new List<string>()
                {
                    "Home win: 2-0",
                    "Draw: 0-0",
                    "Home win: 1-0",
                    "Away win: 0-1",
                    "Draw: 1-1",
                    "Away win: 0-2",
                    "Home win: 2-1",
                    "Away win: 1-2",
                    "Draw: 2-2",
                    "Home win: 3-0",
                    "Away win: 0-3",
                    "Draw: 3-3",
                    "Home win: 3-2",
                    "Away win: 2-3"
                };
                foreach(var result in results) {
                    Bet bet = new Bet();
                    bet.Name = result;
                    bet.Coefficient = 0.0;
                    bet.NumberOfBets = 0;
                    bet.Income = 0;
                    bets.Add(bet);
                
                
                }
                bets1.BetsOptions = bets;
                bets1.FixtureData = fixtureData;
                bool existingFixture = false;
                foreach(var betsFixture in betsList)
                {
                    if(betsFixture.FixtureData.Id == fixtureData.Id)
                    {
                        existingFixture = true;
                        break;
                    }
                }
                if(!existingFixture)
                {
                    await betsCollection.InsertOneAsync(bets1);
                    bets2.Add(bets1);
                }

                fixturesData.Add(fixtureData);

            }
            return bets2;

        }
        public async Task<Bets> addNewBet(AddBet addBet)
        {
            List<FixtureData> fixturesData = new List<FixtureData>();
            Bets final = new Bets();
            var filter = Builders<BsonDocument>.Filter.Eq("league.season", 2023);
            var filter2 = Builders<BsonDocument>.Filter.Eq("fixture.status.short", "NS");
            var fixturesFiltered = await collection.FindAsync(filter2);
            var fixtures = await fixturesFiltered.ToListAsync();

            var filterBets = Builders<Bets>.Filter.Empty;
            var betsFiltered = await betsCollection.FindAsync(filterBets);
            var betsList = await betsFiltered.ToListAsync();

            foreach(var b in betsList)
            {
                if(b.FixtureData.Id == addBet.GameId)
                {
                    Bet newObject = new Bet();
                    newObject.Coefficient = addBet.Coefficient;
                    newObject.NumberOfBets = addBet.NumberOfBets;
                    newObject.Name = addBet.Name;
                    newObject.Income = addBet.Income;
                    var filterBet = Builders<Bets>.Filter.Eq("FixtureData._id", addBet.GameId);

                    // Define the update operation to add the new Bet object to the array
                    var update = Builders<Bets>.Update.Push("BetsOptions", newObject);

                   
                    var updateResult = await betsCollection.UpdateOneAsync(filterBet, update);


                    if (updateResult.ModifiedCount > 0)
                    {
                        Console.WriteLine("Document updated successfully.");
                    }
                    final = b;

                }
            }

           return final;

            

            
        }

        public async Task<List<Bets>> filterBets(BetFilter betFilter)
        {
            List<FixtureData> fixturesData = new List<FixtureData>();

            var filter = Builders<BsonDocument>.Filter.Eq("league.season", 2023);
            var filter2 = Builders<BsonDocument>.Filter.Eq("fixture.status.short", "NS");
            var fixturesFiltered = await collection.FindAsync(filter2);

            var filterBets = Builders<Bets>.Filter.Empty;
            var betsFiltered = await betsCollection.FindAsync(filterBets);
            var betsList = await betsFiltered.ToListAsync();

            var revFilter = Builders<BsonDocument>.Filter.Empty;
            var revenuesFiltered = await revenueCollection.FindAsync(revFilter);
            var revList = await revenuesFiltered.ToListAsync();

            List<Bets> bets2 = new List<Bets>();

            var fixtures = await fixturesFiltered.ToListAsync();

            foreach (var myBets in betsList)
            {
                if(myBets.FixtureData.AwayName ==betFilter.team_name|| myBets.FixtureData.AwayName.Equals(betFilter.team_name)) { 
                    
                    bets2.Add(myBets);
                }
            }

            List<Bets> filteredTeam = new List<Bets>();
            
            
            if (bets2.Count == 0)
            {
               bets2 = betsList.ToList();
                    

            }
                
                
                    
                

            
            foreach (var myBets in bets2)
            {
                if (DateTime.Parse(myBets.FixtureData.Date).Month.ToString() ==betFilter.month)
                {

                    filteredTeam.Add(myBets);
                }
            }
            if(filteredTeam.Count == 0)
            {
                filteredTeam = bets2;
            }



            List<Bets> filteredInBets = new List<Bets>();
            foreach (var myBets in filteredTeam)
            {
                string coefBet = betFilter.coef;
                int more2 = 0;
                if (coefBet == "moreThanTwo")
                {
                    more2 =1;
                }else if(coefBet == "lessThanTwo")
                {
                    more2 =2;
                }
                List<Bet> newBets = new List<Bet>();
                foreach(var myBet in myBets.BetsOptions)
                {
                    if (more2==1)
                    {
                        if (myBet.Coefficient>=2)
                        {
                            newBets.Add(myBet);
                        }
                    }
                    else if(more2==2){

                        if (myBet.Coefficient < 2)
                        {
                            newBets.Add(myBet);
                        }


                    }
                    else
                    {
                        newBets.Add(myBet);
                    }
                            
                }
                if(newBets.Count > 0)
                {

                    myBets.BetsOptions = newBets;
                    filteredInBets.Add(myBets);
                }               
            }


            List<Bets> filterRevTeam = new List<Bets>();

            List<Bets> revenueMonthFilter = new List<Bets>();
            /*
            if (betFilter.coef == "lessThanTwo"||betFilter.coef =="")
            {
               
                foreach (var rev in revList)
                {
                    Bets newBets = new Bets();
                    // fixture["teams"]["home"]["name"].AsString;
                    if (rev["Game"]["HomeTeam"]["Name"].AsString == betFilter.team_name || rev["Game"]["AwayTeam"]["Name"].AsString == betFilter.team_name)
                    {
                        FixtureData fData = new FixtureData();
                        foreach (var f in fixtures)
                        {
                            if (f["fixture"]["id"].AsInt32 == rev["Game"]["_id"].AsInt32)
                            {
                                fData.Id = f["fixture"]["id"].AsInt32;
                                fData.City = f["fixture"]["venue"]["city"].AsString;
                                fData.Stadium = f["fixture"]["venue"]["name"].AsString;
                                fData.Round = f["league"]["round"].AsString;
                                fData.HomeName = f["teams"]["home"]["name"].AsString;
                                fData.AwayName = f["teams"]["away"]["name"].AsString;
                                fData.HomeLogo = f["teams"]["home"]["logo"].AsString;
                                fData.AwayLogo = f["teams"]["away"]["logo"].AsString;
                                fData.Date = f["fixture"]["date"].AsString;
                                break;

                            }
                        }
                        List<Bet> revBets = new List<Bet>();
                        newBets.FixtureData = fData;
                        var revenues = rev["revenues"].AsBsonArray;
                        foreach (var revenue in revenues)
                        {
                            Bet r = new Bet();
                            r.Coefficient = revenue["Coef"].AsDouble;
                            r.NumberOfBets = revenue["Number_of_bets"].AsInt32;
                            r.Name = revenue["Result"].AsString;
                            r.Income = revenue["Income"].AsInt32;
                            revBets.Add(r);


                        }
                        newBets.BetsOptions = revBets;
                        filterRevTeam.Add(newBets);


                    }

                }

                
               
                foreach (var rev in revList)
                {
                    Bets newBets = new Bets();
                    // fixture["teams"]["home"]["name"].AsString;
                    if (DateTime.Parse(rev["Game"]["Date"].AsString).Month.ToString() == betFilter.month)
                    {
                        FixtureData fData = new FixtureData();
                        foreach (var f in fixtures)
                        {
                            if (f["fixture"]["id"].AsInt32 == rev["Game"]["_id"].AsInt32)
                            {
                                fData.Id = f["fixture"]["id"].AsInt32;
                                fData.City = f["fixture"]["venue"]["city"].AsString;
                                fData.Stadium = f["fixture"]["venue"]["name"].AsString;
                                fData.Round = f["league"]["round"].AsString;
                                fData.HomeName = f["teams"]["home"]["name"].AsString;
                                fData.AwayName = f["teams"]["away"]["name"].AsString;
                                fData.HomeLogo = f["teams"]["home"]["logo"].AsString;
                                fData.AwayLogo = f["teams"]["away"]["logo"].AsString;
                                fData.Date = f["fixture"]["date"].AsString;
                                break;

                            }
                        }
                        List<Bet> revBets = new List<Bet>();
                        newBets.FixtureData = fData;
                        var revenues = rev["revenues"].AsBsonArray;
                        foreach (var revenue in revenues)
                        {
                            Bet r = new Bet();
                            r.Coefficient = revenue["Coef"].AsDouble;
                            r.NumberOfBets = revenue["Number_of_bets"].AsInt32;
                            r.Name = revenue["Result"].AsString;
                            r.Income = revenue["Income"].AsInt32;
                            revBets.Add(r);


                        }
                        newBets.BetsOptions = revBets;
                        revenueMonthFilter.Add(newBets);


                    }

                }
            }*/


            filteredInBets.AddRange(filterRevTeam);
            filteredInBets.AddRange(revenueMonthFilter);
            foreach(var bet in filteredInBets)
            {
                bet.BetsOptions.RemoveAll(x => x.NumberOfBets == 0);
            }
            filteredInBets.RemoveAll(x=>x.BetsOptions.Count == 0);

            List<int> dulicatesIds = new List<int>();
            foreach(var b in bets2)
            {
                dulicatesIds.Add(b.FixtureData.Id);
            }

            List<int> noDuplicateIds = dulicatesIds.Distinct().ToList();
            List<Bets> noDuplicates = new List<Bets>();
            foreach(var id in noDuplicateIds)
            {
                Bets bets = bets2.FirstOrDefault(x=> x.FixtureData.Id == id);
                if(bets != null)
                {
                    noDuplicates.Add(bets); 
                }
            }







            return filteredInBets;

        }


        public async Task<bool> writeToCSV(ExportData exportData)
        {

            string connectionString = "Host=localhost;Username=postgres;Password=16042004Db;Database=Football";
            bool result = false;
            var stringsWithDot = exportData.columns.Where(s => s.Contains('.')).ToList();

           
            HashSet<string> uniqueStrings = new HashSet<string>(stringsWithDot);

           
            List<string> uniqueStringsList = uniqueStrings.ToList();
            StringBuilder sb = new StringBuilder();
            StringBuilder groupBy = new StringBuilder();
            int i = 0;
            foreach(var column in uniqueStringsList)
            {
                sb.Append(column);
                i++;
                if (i != uniqueStringsList.Count)
                {
                    sb.Append(',');
                }
                
                

            }
            

            string betQuery = $"select {sb.ToString()} \r\nfrom bet_result br\r\njoin game g on g.id = br.game_id\r\njoin league l on l.id = g.league_id\r\njoin date d on d.id = br.date_id\r\njoin venue v on v.id =g.venue_id\r\njoin result r on r.id = br.result_id\r\njoin team th on g.home_team_id = th.id\r\njoin team ta on g.away_team_id = ta.id\r\nlimit {exportData.limit}";
            string playerQuery = $"select {sb.ToString()} \r\nfrom player_result pr\r\njoin game g on g.id = pr.game_id\r\njoin league l on l.id = g.league_id\r\njoin date d on d.id = pr.date_id\r\njoin venue v on v.id =g.venue_id\r\njoin team th on g.home_team_id = th.id\r\njoin team ta on g.away_team_id = ta.id\r\njoin player pl on pl.id = pr.player_id\r\nlimit {exportData.limit}";
            string gameQuery = $"select {sb.ToString()} \r\nfrom game_result gr\r\njoin game g on g.id = gr.game_id\r\njoin league l on l.id = g.league_id\r\njoin date d on d.id = gr.date_id\r\njoin venue v on v.id =g.venue_id\r\njoin team th on g.home_team_id = th.id\r\njoin team ta on g.away_team_id = ta.id\r\njoin period p on p.id = gr.period_id\r\njoin team t on t.id = gr.team_id\r\nlimit {exportData.limit}";


            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                if (exportData.fact == "Player")
                {

                    if (exportData.type == "csv")
                    {
                        using (var cmd = new NpgsqlCommand($"COPY ({playerQuery}) TO 'C:\\Users\\symbi\\Desktop\\kursova\\src\\CSVFiles\\player_fact.csv' DELIMITER ',' CSV HEADER", conn))
                        {
                            cmd.ExecuteNonQuery();
                            result = true;

                        }
                    }
                    else if (exportData.type == "json")
                    {

                        using (var cmd = new NpgsqlCommand($"COPY (\r\n  SELECT json_agg(row_to_json( data_row )) \r\n  FROM (\r\n\t{playerQuery}\r\n\t\t\r\n\t\t) data_row\r\n  \r\n) to 'C:\\Users\\symbi\\Desktop\\kursova\\src\\JSONFiles\\player_fact.json'", conn))
                        {
                            cmd.ExecuteNonQuery();
                            result = true;

                        }

                    }
                }

                else if (exportData.fact == "Bet")
                {
                    if (exportData.type == "csv")
                    {
                        using (var cmd = new NpgsqlCommand($"COPY ({betQuery}) TO 'C:\\Users\\symbi\\Desktop\\kursova\\src\\CSVFiles\\bet_fact.csv' DELIMITER ',' CSV HEADER", conn))
                        {
                            cmd.ExecuteNonQuery();
                            result = true;

                        }
                    }else if(exportData.type == "json")
                    {
                        using (var cmd = new NpgsqlCommand($"COPY (\r\n  SELECT json_agg(row_to_json( data_row )) \r\n  FROM (\r\n\t{betQuery}\r\n\t\t\r\n\t\t) data_row\r\n  \r\n) to 'C:\\Users\\symbi\\Desktop\\kursova\\src\\JSONFiles\\bet_fact.json'", conn))
                        {
                            cmd.ExecuteNonQuery();
                            result = true;

                        }

                    }
                }
                else if (exportData.fact == "Game")
                {
                    if (exportData.type == "csv")
                    {
                        using (var cmd = new NpgsqlCommand($"COPY ({gameQuery}) TO 'C:\\Users\\symbi\\Desktop\\kursova\\src\\CSVFiles\\game_fact.csv' DELIMITER ',' CSV HEADER", conn))
                        {
                            cmd.ExecuteNonQuery();
                            result = true;

                        }
                    }
                    else if(exportData.type == "json")
                    {
                        using (var cmd = new NpgsqlCommand($"COPY (\r\n  SELECT json_agg(row_to_json( data_row )) \r\n  FROM (\r\n\t{gameQuery}\r\n\t\t\r\n\t\t) data_row\r\n  \r\n) to 'C:\\Users\\symbi\\Desktop\\kursova\\src\\JSONFiles\\game_fact.json'", conn))
                        {
                            cmd.ExecuteNonQuery();
                            result = true;

                        }
                    }
                }


            }



            return result;
        }


        public async Task<string[]> fillFiles()
        {
            string connectionString = "Host=localhost;Username=postgres;Password=16042004Db;Database=Football";
            string[] filePathes = new string[3];
           
           
           

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("COPY public.player_result TO 'C:\\Users\\symbi\\Desktop\\OLAP_CSV\\player_result_fact.csv' DELIMITER ',' CSV HEADER", conn))
                {
                    cmd.ExecuteNonQuery();
                    filePathes[0] = @"C:\Users\symbi\Desktop\OLAP_CSV\player_result_fact.csv";
                }
                using (var cmd = new NpgsqlCommand("COPY public.bet_result TO 'C:\\Users\\symbi\\Desktop\\OLAP_CSV\\bet_result_fact.csv' DELIMITER ',' CSV HEADER", conn))
                {
                    cmd.ExecuteNonQuery();
                    filePathes[1] = @"C:\Users\symbi\Desktop\OLAP_CSV\bet_result_fact.csv";
                }
                using (var cmd = new NpgsqlCommand("COPY public.game_result TO 'C:\\Users\\symbi\\Desktop\\OLAP_CSV\\game_result_fact.csv' DELIMITER ',' CSV HEADER", conn))
                {
                    cmd.ExecuteNonQuery();
                    filePathes[2] = @"C:\Users\symbi\Desktop\OLAP_CSV\game_result_fact.csv";
                }


            }



            Console.WriteLine("Data copied successfully.");
            return filePathes;

        }


        public async Task<List<TeamStat>> getTeamStats()
        {
            string connectionString = "Host=localhost;Username=postgres;Password=16042004Db;Database=Football";
            List<TeamStat> teamsData = new List<TeamStat>();
            var filter = Builders<Team>.Filter.Empty;

            var teamsFiltered = await teamCollection.FindAsync(filter);
            var teams = await teamsFiltered.ToListAsync();
            foreach (var team in teams)
            {
                
                
                if (team.Name == "FC Schalke 04")
                {
                    continue;
                }
                TeamStat teamStat = new TeamStat();
                string query = "SELECT SUM(gr.goals), SUM(gr.assists), SUM(gr.fouls), SUM(gr.substitutions) " +
                       "FROM game_result gr " +
                       "JOIN team t ON t.id = gr.team_id " +
                       $"WHERE t.name = '{team.Name}'";

                try
                {
                    using (var conn = new NpgsqlConnection(connectionString))
                    {
                        conn.Open();

                        using (var cmd = new NpgsqlCommand(query, conn))
                        {
                           
                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    
                                    int sumOfGoals = reader.GetInt32(0);
                                    int sumOfAssists = reader.GetInt32(1);
                                    int sumOfFouls = reader.GetInt32(2);
                                    int sumOfSubstitutions = reader.GetInt32(3);
                                    teamStat.team_name = team.Name;
                                    teamStat.goals = sumOfGoals;
                                    teamStat.assists = sumOfAssists;
                                    teamStat.fouls = sumOfFouls;
                                    teamStat.substitutions = sumOfSubstitutions;
                                    teamsData.Add(teamStat);    

                                   
                                }
                                else
                                {
                                    Console.WriteLine("No data found for FC Heidenheim.");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return teamsData;
            
        }


        public async Task<List<TeamStat>> getTeamStatsRound(string round)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=16042004Db;Database=Football";
            List<TeamStat> teamsData = new List<TeamStat>();
            var filter = Builders<Team>.Filter.Empty;

            var teamsFiltered = await teamCollection.FindAsync(filter);
            var teams = await teamsFiltered.ToListAsync();
            foreach (var team in teams)
            {


                if (team.Name == "FC Schalke 04")
                {
                    continue;
                }
                TeamStat teamStat = new TeamStat();
                string query = "SELECT SUM(gr.goals), SUM(gr.assists), SUM(gr.fouls), SUM(gr.substitutions) " +
                       "FROM game_result gr " +
                       "JOIN team t ON t.id = gr.team_id " +
                       "JOIN game g on g.id = gr.game_id "+
                       "JOIN league l on l.id = g.league_id "+
                       $"WHERE t.name = '{team.Name}' AND l.round = '{round}'";

                try
                {
                    using (var conn = new NpgsqlConnection(connectionString))
                    {
                        conn.Open();

                        using (var cmd = new NpgsqlCommand(query, conn))
                        {
                           
                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                   
                                    int sumOfGoals = reader.GetInt32(0);
                                    int sumOfAssists = reader.GetInt32(1);
                                    int sumOfFouls = reader.GetInt32(2);
                                    int sumOfSubstitutions = reader.GetInt32(3);
                                    teamStat.team_name = team.Name;
                                    teamStat.goals = sumOfGoals;
                                    teamStat.assists = sumOfAssists;
                                    teamStat.fouls = sumOfFouls;
                                    teamStat.substitutions = sumOfSubstitutions;
                                    teamsData.Add(teamStat);


                                }
                                else
                                {
                                    Console.WriteLine("No data found for FC Heidenheim.");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return teamsData;

        }

        public bool runSQL()
        {
            string connectionString = "Host=localhost;Username=postgres;Password=16042004Db;Database=Football";
            string filePath = "C:\\Users\\symbi\\Desktop\\kursova\\src\\OLAP\\createDatabase.sql";
            bool result = false;

                try
                {
                string query = File.ReadAllText(filePath);
                    using (var conn = new NpgsqlConnection(connectionString))
                    {
                        conn.Open();

                        using (var cmd = new NpgsqlCommand(query, conn))
                        {

                        cmd.ExecuteNonQuery();
                        result = true;
                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    result = false;
                }


            return result; ;

        }

        public int RunConsoleProgram(string programPath, string arguments)
        {
            bool isSqlRun = runSQL();
            if (isSqlRun)
            {
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = programPath,
                        Arguments = arguments,
                        UseShellExecute = false, // Do not use shell execution
                        RedirectStandardOutput = true, // Redirect standard output
                        RedirectStandardError = true // Redirect standard error
                    };

                    using (Process process = new Process())
                    {
                        process.StartInfo = startInfo;
                        process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data); // Output data handler
                        process.ErrorDataReceived += (sender, e) => Console.WriteLine($"Error: {e.Data}"); // Error data handler

                        process.Start();
                        process.BeginOutputReadLine(); // Start asynchronous read operations on the redirected StandardOutput stream
                        process.BeginErrorReadLine(); // Start asynchronous read operations on the redirected StandardError stream
                        process.WaitForExit(); // Wait for the process to exit

                        int exitCode = process.ExitCode;
                        return exitCode;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing console program: {ex.Message}");
                    return -1; // Return a custom error code
                }
            }
            return -1;
        }






        public async Task<List<Team>> getTeams()
        {
            List<Team> teamsData = new List<Team>();
            var filter = Builders<Team>.Filter.Empty;
           
            var teamsFiltered = await teamCollection.FindAsync(filter);




            var teams = await teamsFiltered.ToListAsync();
            foreach( var team in teams)
            {
                if(team.Name == "FC Schalke 04")
                {
                    continue;
                }
                teamsData.Add(team);
            }
            return teamsData;
        }

        public async Task<Bet?> updateBet(Bet bet, int id)
        {
            var filterBets = Builders<Bets>.Filter.Empty;
            var betsFiltered = await betsCollection.FindAsync(filterBets);
            Bets newBets = new Bets();

            var betsList = await betsFiltered.ToListAsync();
            foreach ( var bets in betsList)
            {
                if(bets.FixtureData.Id == id)
                {
                    newBets = bets;
                    foreach(var b in bets.BetsOptions)
                    {
                        if(b.Name == bet.Name)
                        {
                            b.NumberOfBets += bet.NumberOfBets;
                            b.Income = b.Income + bet.Income * bet.NumberOfBets;
                            b.Coefficient = bet.Coefficient;
                          
                            var filter = Builders<Bets>.Filter.And(
                                Builders<Bets>.Filter.Eq("FixtureData._id", id),
                                Builders<Bets>.Filter.ElemMatch(x => x.BetsOptions, Builders<Bet>.Filter.Eq(x => x.Name, bet.Name))
                            );

                            var update = Builders<Bets>.Update
                                .Set("BetsOptions.$.Coefficient", b.Coefficient)
                                .Set("BetsOptions.$.NumberOfBets", b.NumberOfBets)
                                .Set("BetsOptions.$.Income", b.Income);

                            await betsCollection.UpdateOneAsync(filter, update);
                            return b;

                        }


                    }
                }
            }

            return null;

        }

        public async Task<List<Bets>> getSomeBets()
        {
            var filterBets = Builders<Bets>.Filter.Empty;
            var betsFiltered = await betsCollection.FindAsync(filterBets);
            List<Bets> fullBets = new List<Bets>();

            var betsList = await betsFiltered.ToListAsync();
            foreach (var bets in betsList)
            {
                List<Bet> someBets = new List<Bet>();
                foreach(var b in bets.BetsOptions)
                {
                    if(b.NumberOfBets > 0)
                    {
                        someBets.Add(b);
                    }
                }
                if (someBets.Count > 0)
                {
                    Bets bets1 = new Bets();    
                    bets1.BetsOptions = someBets;
                    bets1.FixtureData = bets.FixtureData;
                    fullBets.Add(bets1);
                }
            }
            List<Bets> finalBets = new List<Bets>();
            foreach(var bets in fullBets)
            {
                DateTime fDate = DateTime.Parse(bets.FixtureData.Date);
                if(fDate.Day> DateTime.Now.Day&& fDate.Month >= DateTime.Now.Month)
                {
                    finalBets.Add(bets);
                }

            }

            return finalBets;
        }

        public async Task<List<Bet>> getBetsOptions(int id)
        {
            var filterBets = Builders<Bets>.Filter.Empty;
            var betsFiltered = await betsCollection.FindAsync(filterBets);
            List<Bet> fullBetsOptions = new List<Bet>();

            var betsList = await betsFiltered.ToListAsync();
            foreach (var bets in betsList)
            {
               
                if (bets.FixtureData.Id == id)
                {
                    foreach(var bet in bets.BetsOptions)
                    {
                        fullBetsOptions.Add(bet);

                    }
                }
               
            }
            

            return fullBetsOptions;
        }

        public async Task<List<FixtureData>> getFinished()
        {
            List<FixtureData> fixturesData = new List<FixtureData>();
            var filter = Builders<BsonDocument>.Filter.Eq("league.season", 2023);
            var filter2 = Builders<BsonDocument>.Filter.Eq("fixture.status.short", "NS");
            var fixturesFiltered = await collection.FindAsync(filter);




            var fixtures = await fixturesFiltered.ToListAsync();
            foreach (var fixture in fixtures)
            {
                if (fixture["fixture"]["status"]["short"].AsString == "FT")
                {
                    FixtureData fixtureData = new FixtureData();
                    fixtureData.Id = fixture["fixture"]["id"].AsInt32;
                    fixtureData.City = fixture["fixture"]["venue"]["city"].AsString;
                    fixtureData.Stadium = fixture["fixture"]["venue"]["name"].AsString;
                    fixtureData.Round = fixture["league"]["round"].AsString;
                    fixtureData.HomeName = fixture["teams"]["home"]["name"].AsString;
                    fixtureData.AwayName = fixture["teams"]["away"]["name"].AsString;
                    fixtureData.HomeLogo = fixture["teams"]["home"]["logo"].AsString;
                    fixtureData.AwayLogo = fixture["teams"]["away"]["logo"].AsString;
                    fixtureData.Date = fixture["fixture"]["date"].AsString;
                    int homeRes = fixture["score"]["fulltime"]["home"].AsInt32;
                    int awayRes = fixture["score"]["fulltime"]["away"].AsInt32;
                    string result = homeRes.ToString()+"-"+awayRes.ToString();  
                    fixtureData.Result = result;


                    DateTime fixturedate = DateTime.Parse(fixtureData.Date);
                    DateTime now = DateTime.Now;

                    if (fixturedate.Month == now.Month || fixturedate.Month == now.Month - 1)
                    {
                        fixturesData.Add(fixtureData);
                    }
                }

            }

            List<FixtureData> lastMatches = fixturesData.Skip(fixturesData.Count -18).ToList();
            fixtureDatas = fixturesData;
            return lastMatches;
        }


        public async Task<List<FixtureData>> getFinishedByRound(string round)
        {
            List<FixtureData> fixturesData = new List<FixtureData>();
            var filter = Builders<BsonDocument>.Filter.Eq("league.season", 2023);
            var filter2 = Builders<BsonDocument>.Filter.Eq("fixture.status.short", "NS");
            var fixturesFiltered = await collection.FindAsync(filter);




            var fixtures = await fixturesFiltered.ToListAsync();
            foreach (var fixture in fixtures)
            {
                if (fixture["fixture"]["status"]["short"].AsString == "FT" && fixture["league"]["round"] == round)
                {
                    FixtureData fixtureData = new FixtureData();
                    fixtureData.Id = fixture["fixture"]["id"].AsInt32;
                    fixtureData.City = fixture["fixture"]["venue"]["city"].AsString;
                    fixtureData.Stadium = fixture["fixture"]["venue"]["name"].AsString;
                    fixtureData.Round = fixture["league"]["round"].AsString;
                    fixtureData.HomeName = fixture["teams"]["home"]["name"].AsString;
                    fixtureData.AwayName = fixture["teams"]["away"]["name"].AsString;
                    fixtureData.HomeLogo = fixture["teams"]["home"]["logo"].AsString;
                    fixtureData.AwayLogo = fixture["teams"]["away"]["logo"].AsString;
                    fixtureData.Date = fixture["fixture"]["date"].AsString;
                    int homeRes = fixture["score"]["fulltime"]["home"].AsInt32;
                    int awayRes = fixture["score"]["fulltime"]["away"].AsInt32;
                    string result = homeRes.ToString() + "-" + awayRes.ToString();
                    fixtureData.Result = result;


                  

                    
                        fixturesData.Add(fixtureData);
                    
                }

            }

           // List<FixtureData> lastMatches = fixturesData.Skip(fixturesData.Count - 18).ToList();
           // fixtureDatas = fixturesData;
            return fixturesData;
        }


        public async Task<bool> deleteAllBets()
        {
            var filter = Builders<Bets>.Filter.Empty;

            
            await betsCollection.DeleteManyAsync(filter);
            return true;
        }



        public async Task<List<FixtureData>?> getFixtures()
        {
            return fixtureDatas;

        }
    }
}
