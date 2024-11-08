using Microsoft.AspNetCore.Mvc;
using MongoCursovaWebApi.Data;
using MongoCursovaWebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MongoCursovaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        // GET: api/<TeamController>
        MongoDbContext db = new MongoDbContext();



        [HttpGet]
        public async Task<IEnumerable<Team>> Get()
        {
            var teams = await db.getTeams();
            return teams;
        }

        [HttpGet]
        [Route("AllStat")]
        public async Task<IEnumerable<TeamStat>> GetStatAll()
        {
            var teamsStats = await db.getTeamStats();
            return teamsStats;
        }

        [HttpPost]
        [Route("RoundStat")]
        public async Task<IEnumerable<TeamStat>> PostStatRound([FromBody] string round)
        {
            var teamsStats = await db.getTeamStatsRound(round);
            return teamsStats;
        }
        // GET api/<TeamController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TeamController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TeamController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TeamController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
