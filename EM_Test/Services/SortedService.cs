using EM_Test.Controllers;
using EM_Test.Mappers;
using EM_Test.Models;
using EM_TestRepository.Entity;
using EM_TestRepository.Repository;
using System.Text.Json;

namespace EM_Test.Services
{
    public class SortedService
    {
        private readonly IRepository<Request> _requestRepository;
        private readonly ISortable<Order> _sorter;
        private readonly ILogger<OrderController> _logger;

        public SortedService(ILogger<OrderController> logger, ISortable<Order> sorter, IRepository<Request> requestRepository)
        {
            _logger = logger;
            _sorter = sorter;
            _requestRepository = requestRepository;
        }


        public async Task<IEnumerable<Order>> Sort(int idLocation, DateTime date)
        {
            var orders = await _sorter.Sort(idLocation, date);
            
            _logger.LogInformation($"Request processed for street ID {idLocation} at time {date}. Result: {JsonSerializer.Serialize(orders)}");
            var req = new Request()
            {
                LocationId = idLocation,
                Answer = JsonSerializer.Serialize(orders),
                RequestTime = date
            };
            await _requestRepository.CreateAsync(req);
            return orders;
        }
    }
}
