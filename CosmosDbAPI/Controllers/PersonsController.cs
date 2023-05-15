using CosmosDbAPI.Contracts;
using CosmosDbAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CosmosDbAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly ICosmosDBService _cosmosDbService;
        public PersonsController(ICosmosDBService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
        }
        // GET api/items
        [HttpGet]
        public async Task<IActionResult> List()
        {
            return Ok(await _cosmosDbService.GetAllAsync("SELECT * FROM c"));
        }
        // GET api/items/5
        [HttpGet("{id}/{partitionkey}")]
        public async Task<IActionResult> Get(string id, string partitionkey)
        {
            return Ok(await _cosmosDbService.GetByIdAsync(id, partitionkey));
        }
        // POST api/items
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Persons person)
        {
            person.PersonID = Guid.NewGuid().ToString();
            await _cosmosDbService.AddPersonAsync(person);
            return CreatedAtAction(nameof(Get), new { id = person.PersonID, partitionkey = person.FirstName }, person);
        }
        // PUT api/items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromBody] Persons person)
        {
            await _cosmosDbService.UpdateByIdAsync(person.PersonID, person);
            return NoContent();
        }
        // DELETE api/items/5
        [HttpDelete("{id}/{partitionkey}")]
        public async Task<IActionResult> Delete(string id, string partitionkey)
        {
            await _cosmosDbService.DeleteByIdAsync(id, partitionkey);
            return NoContent();


        }
    }
}
