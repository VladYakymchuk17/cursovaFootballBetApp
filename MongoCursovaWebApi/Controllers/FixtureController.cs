using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoCursovaWebApi.Data;
using MongoCursovaWebApi.Models;
using MongoDB.Bson;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MongoCursovaWebApi.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    [ApiController]
    public class FixtureController : ControllerBase
    {
        MongoDbContext db = new MongoDbContext();
        [HttpGet]
        public async Task<IEnumerable<FixtureData>> Get()
        {
            var fixtures = await db.fillData();
            return fixtures;
        }

        [HttpGet]
        [Route("Finished")]
        public async Task<IEnumerable<FixtureData>> GetFinished()
        {
            var fixtures = await db.getFinished();
            return fixtures;
        }


       




        // GET api/<FixtureController>/5
        [HttpGet("{id}")]
        public async Task<FixtureData?> Get(int id)
        {
            var fixtures = await db.fillData();

            return fixtures.Find(f => f.Id ==id);
        }

        // POST api/<FixtureController>
        [HttpPost]
        public async Task<string[]> Post()
        {

            var results = await db.fillFiles();
            return results;


        }


        [HttpPost]
        [Route("FinishedByRound")]
        public async Task<IEnumerable<FixtureData>> PostFinishedByRound([FromBody] string round)
        {

            var fixtures = await db.getFinishedByRound(round);
            return fixtures;


        }


        [HttpPost]
        [Route("Filtered")]

        public async Task<IEnumerable<FixtureData>?> PostFiltered(GameFilter gameFilter)
        {
            int month =  Int32.Parse(gameFilter.month);
            string team_name = gameFilter.team_name;
            List<FixtureData> results = new List<FixtureData> ();
            if (month!=0&& team_name=="")
            {
                results = await db.getFixturesByMonth(month);
                Console.WriteLine(month);
            }
            else if (month != 0 && team_name != "")
            {
                results = await db.getFixturesByTeamMonth(month, team_name);
            }
            else if (month == 0 && team_name != "")
            {
                results = await db.getFixturesByTeam(team_name);
            }
            else
            {
                results = await db.fillData();
            }


            return results;


        }

        // PUT api/<FixtureController>/5
        [HttpPut]
        public IActionResult Put()
        {
           int exitCode = db.RunConsoleProgram(@"C:\Users\symbi\source\repos\ETLPostgres\bin\Debug\net7.0\ETLPostgres.exe", "arg1 arg2");
            if (exitCode == 0)
            {
                return Ok("Console program executed successfully.");
            }
            else
            {
                return BadRequest("Console program encountered an error.");
            }
        }

    }
}
