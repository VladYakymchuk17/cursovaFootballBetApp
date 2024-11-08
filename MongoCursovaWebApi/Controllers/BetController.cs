using Microsoft.AspNetCore.Mvc;
using MongoCursovaWebApi.Data;
using MongoCursovaWebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MongoCursovaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetController : ControllerBase
    {

        MongoDbContext db = new MongoDbContext();
        // GET: api/<BetController>
        [HttpGet]
        public async Task<IEnumerable<Bets>> Get()
        {
            var bets  = await db.getSomeBets();
            return bets;
        }

        // GET api/<BetController>/5
        [HttpGet("{id}")]
        public async Task<List<Bet>> Get(int id)
        {
            var bets = await db.getBetsOptions(id);
            return bets;
        }

        // POST api/<BetController>
        [HttpPost]
        public async Task<List<Bets>> Post([FromBody] string value)
        {
           var results = await db.fillBets();
            return results;

        }


        // POST api/<BetController>
        [HttpPost]
        [Route("AddNew")]
        public async Task<Bets> PostNew(AddBet addBet)
        {
            var result  = await db.addNewBet(addBet);
            return result;
        }


        // POST api/<BetController>
        [HttpPost]
        [Route("Filtered")]
        public async Task<List<Bets>> PostFilteredBets([FromBody] BetFilter betFilter)
        {

            var results = await db.filterBets(betFilter);
            return results;

        }

        // PUT api/<BetController>/5
        [HttpPut("{id}")]
        public async Task<Bet?> Put([FromBody] Bet value, int id)
        {
            var result = await db.updateBet(value, id);
            return result;

        }

        // DELETE api/<BetController>/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            var res = await db.deleteAllBets();
            return res;
        }
    }
}
