using EM_Test.Mappers;
using EM_Test.Models;
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
        private readonly ILogger<OrderController> _logger;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Request> _requestRepository;
        private readonly ISortable<Order> _sorter;

        public OrderController(ILogger<OrderController> logger, IRepository<Order> orderRepository, IRepository<Request> requestRepository, ISortable<Order> sorter)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _requestRepository = requestRepository;
            _sorter = sorter;
        }

        [HttpGet("sort")]
        public async Task<ActionResult<IEnumerable<OrderModel>>> Sort(int idLocation, DateTime date)
        {
            var request = new RequestModel() { LocationId = idLocation, RequestTime = date };
            var orders = await _sorter.Sort(idLocation, date);
            if (orders == null || orders.Count() == 0)
            {
                _logger.LogInformation($"Request {idLocation} from date {date}. Answer: Not found");
                request.Answer = "Not found";
                await _requestRepository.CreateAsync(RequestMapper.FromApiModel(request));
                return NotFound();
            }
            _logger.LogInformation($"Request {idLocation} from date {date}. Answer:{JsonSerializer.Serialize(orders)}");
            request.Answer = JsonSerializer.Serialize(orders);
            await _requestRepository.CreateAsync(RequestMapper.FromApiModel(request));
            return Ok(OrderMapper.ToApiModel(orders));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetAll()
        {
            var orders = await _orderRepository.GetAllAsync();
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(OrderMapper.ToApiModel(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderModel>> GetById(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return OrderMapper.ToApiModel(order);
        }

        [HttpPost]
        public async Task<ActionResult<OrderModel>> Post([FromBody] OrderModel order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _orderRepository.CreateAsync(OrderMapper.FromApiModel(order));
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] OrderModel order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != order.Id)
            {
                return BadRequest();
            }
            await _orderRepository.UpdateAsync(OrderMapper.FromApiModel(order));
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            await _orderRepository.DeleteAsync(order);
            return Ok();
        }
    }
}
