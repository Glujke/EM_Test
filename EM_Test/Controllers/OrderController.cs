using EM_TestRepository.Entity;
using EM_TestRepository.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EM_Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly IRepository<Order> _repository;
        private readonly IRepository<Request> _repositoryReq;
        private readonly ISortable<Order> _sorter;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IRepository<Order> repository, IRepository<Request> repositoryReq,
            ISortable<Order> sorter, ILogger<OrderController> logger)
        {
            _repository = repository;
            _repositoryReq = repositoryReq;
            _sorter = sorter;
            _logger = logger;
        }

        [HttpGet("sort")]
        public async Task<ActionResult<IEnumerable<Order>>> Sort(int idLocation, DateTime date)
        {
            var request = new Request() { LocationId = idLocation, RequestTime = date };
            var orders = await _sorter.Sort(idLocation, date);
            if (orders == null || orders.Count() == 0)
            {
                _logger.LogInformation($"Request {idLocation} from date {date}. Answer: Not found");
                request.Answer = "Not found";
                await _repositoryReq.CreateAsync(request);
                return NotFound();
            }
            _logger.LogInformation($"Request {idLocation} from date {date}. Answer:{JsonSerializer.Serialize(orders)}");
            request.Answer = JsonSerializer.Serialize(orders);
            await _repositoryReq.CreateAsync(request);
            return Ok(orders);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            var orders = await _repository.GetAllAsync();
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(int id)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return order;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Post([FromBody] Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _repository.CreateAsync(order);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Order order)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            await _repository.DeleteAsync(order);
            return Ok();
        }
    }
}
