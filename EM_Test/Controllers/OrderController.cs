using EM_Test.Mappers;
using EM_Test.Models;
using EM_Test.Services;
using EM_TestRepository.Entity;
using EM_TestRepository.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EM_Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Location> _locationRepository;
        private readonly SortedService _sortedService;

        public OrderController(IRepository<Order> orderRepository, SortedService sortedService, IRepository<Location> locationRepository)
        {
            _orderRepository = orderRepository;
            _locationRepository = locationRepository;
            _sortedService = sortedService;
        }

        [HttpGet("Sort")]
        public async Task<ActionResult<IEnumerable<OrderModel>>> Sort(int idLocation, DateTime date)
        {
            if (idLocation < 0) return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "An error occurred while processing your request.",
                detail: "The id field must not be negative");

            var sortedOrders = await _sortedService.Sort(idLocation, date);
            var sortedModels = OrderMapper.ToApiModel(sortedOrders);
            return Ok(sortedModels);
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
            var orderEntity = OrderMapper.FromApiModel(order);
            var localLocation = await _locationRepository.GetByIdAsync(order.Location.Id);
            if (localLocation != null)
            {
                orderEntity.Location = null;
            }
            else
            {
                return BadRequest();
            }

            await _orderRepository.CreateAsync(orderEntity);
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
            var orderEntity = OrderMapper.FromApiModel(order);
            var localLocation = await _locationRepository.GetByIdAsync(order.Location.Id);
            if (localLocation != null)
            {
                orderEntity.Location = null;
            }
            else
            {
                return BadRequest();
            }

            await _orderRepository.UpdateAsync(orderEntity);
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
