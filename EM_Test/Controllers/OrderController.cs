using EM_Test.Mappers;
using EM_Test.Models;
using EM_Test.Services;
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
        private readonly IRepository<Order> _orderRepository;
        private readonly SortedService _sortedService;

        public OrderController(IRepository<Order> orderRepository, SortedService sortedService)
        {
            _orderRepository = orderRepository;
            _sortedService = sortedService;
        }

        [HttpGet("sort")]
        public async Task<ActionResult<IEnumerable<OrderModel>>> Sort(int idLocation, DateTime date)
        {
            var request = new RequestModel() { LocationId = idLocation, RequestTime = date };
            await _sortedService.Sort(request);
            if (!request.IsSuccess)
            {
                return NotFound();
            }
            return Ok(request.AnswerModel);
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
