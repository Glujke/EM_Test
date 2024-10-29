using EM_Test.Mappers;
using EM_Test.Models;
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
        public async Task<ActionResult<IEnumerable<LocationModel>>> GetAll()
        {
            var locations = await _repository.GetAllAsync();
            if (locations == null)
            {
                return NotFound();
            }
            return Ok(LocationMapper.ToApiModel(locations));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LocationModel>> GetById(int id)
        {
            var location = await _repository.GetByIdAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            return LocationMapper.ToApiModel(location);
        }

        [HttpPost]
        public async Task<ActionResult<LocationModel>> Post([FromBody] LocationModel location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var loc = LocationMapper.FromApiModel(location);
            await _repository.CreateAsync(loc);
            return CreatedAtAction(nameof(GetById), new { id = loc.Id }, loc);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] LocationModel location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != location.Id)
            {
                return BadRequest();
            }
            var loc = LocationMapper.FromApiModel(location);
            await _repository.UpdateAsync(loc);
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
