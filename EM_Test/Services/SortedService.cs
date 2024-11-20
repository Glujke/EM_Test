using EM_Test.Controllers;
using EM_TestRepository.Entity;
using EM_TestRepository.Repository;
using System.Text.Json;

namespace EM_Test.Services
{
    public class SortedService
    {
        private readonly IRepository<DeliveryOrder> _requestRepository;
        private readonly ISortable<Order> _sorter;
        private readonly ILogger<OrderController> _logger;

        public SortedService(ILogger<OrderController> logger, ISortable<Order> sorter, IRepository<DeliveryOrder> requestRepository)
        {
            _logger = logger;
            _sorter = sorter;
            _requestRepository = requestRepository;
        }


        public async Task<IEnumerable<Order>> Sort(int idLocation, DateTime date)
        {
            var orders = await _sorter.Sort(idLocation, date);
            
            _logger.LogInformation($"Request processed for street ID {idLocation} at time {date}. Result: {JsonSerializer.Serialize(orders)}");
            
            //await _requestRepository.CreateAsync(req);  
            return orders;
        }
    }
}
