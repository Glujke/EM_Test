using EM_TestRepository.Entity;
using EM_TestRepository.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace EM_Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly IRepository<Order> _repository;
        private readonly ISortable<Order> _sorter;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IRepository<Order> repository, ISortable<Order> sorter, ILogger<OrderController> logger)
        {
            _repository = repository;
            _sorter = sorter;
            _logger = logger;
        }

        [HttpPut("sort")]
        public async Task<ActionResult<IEnumerable<Order>>> Sort(int idLocation, DateTime date)
        {
            try
            {
                var orders = await _sorter.Sort(idLocation, date);
                if (orders == null)
                {
                    return NotFound();
                }
                _logger.LogInformation($"Request {idLocation} from date {date}");
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception");
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            try
            {
                var orders = await _repository.GetAllAsync();
                if (orders == null)
                {
                    return NotFound();
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(int id)
        {
            try
            {
                var order = await _repository.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }
                return order;
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Post([FromBody] Order order)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _repository.CreateAsync(order);
                return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Order order)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != order.Id)
                {
                    return BadRequest();
                }
                await _repository.UpdateAsync(order);
                return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var order = await _repository.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }
                await _repository.DeleteAsync(order);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}
