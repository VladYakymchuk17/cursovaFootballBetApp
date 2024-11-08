using Microsoft.AspNetCore.Mvc;
using MongoCursovaWebApi.Data;
using MongoCursovaWebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MongoCursovaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {

        MongoDbContext db = new MongoDbContext();
        // GET: api/<StatsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<StatsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<StatsController>
        [HttpPost]
        public async Task<bool> Post([FromBody] ExportData value)
        {
            var res = await db.writeToCSV(value);
            return res;
        }

        // PUT api/<StatsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StatsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
