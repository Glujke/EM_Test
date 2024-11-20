using EM_TestRepository.Entity;
using EM_TestRepository.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EM_Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : Controller
    {
        private readonly IRepository<Location> _repository;

        public LocationController(IRepository<Location> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetAll()
        {
            var locations = await _repository.GetAllAsync();
            if (locations == null)
            {
                return NotFound();
            }
            return Ok(locations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetById(int id)
        {
            var location = await _repository.GetByIdAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            return location;
        }

        [HttpPost]
        public async Task<ActionResult<Location>> Post([FromBody] Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _repository.CreateAsync(location);
            return CreatedAtAction(nameof(GetById), new { id = location.Id }, location);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != location.Id)
            {
                return BadRequest();
            }
            await _repository.UpdateAsync(location);
            return CreatedAtAction(nameof(GetById), new { id = location.Id }, location);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var location = await _repository.GetByIdAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            await _repository.DeleteAsync(location);
            return Ok();
        }
    }
}
