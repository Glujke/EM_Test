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


        public async Task Sort(RequestModel request)
        {
            var orders = await _sorter.Sort(request.LocationId, request.RequestTime);
            if (orders == null || orders.Count() == 0)
            {
                _logger.LogInformation($"Request processed for street ID {request.LocationId} at time {request.RequestTime}. Result: Not found.");
                request.Answer = "Not found";
                await _requestRepository.CreateAsync(RequestMapper.FromApiModel(request));
                request.IsSuccess = false;
                return;
            }
            _logger.LogInformation($"Request processed for street ID {request.LocationId} at time {request.RequestTime}. Result: {JsonSerializer.Serialize(orders)}");
            request.AnswerModel = OrderMapper.ToApiModel(orders);
            request.Answer = JsonSerializer.Serialize(orders);
            await _requestRepository.CreateAsync(RequestMapper.FromApiModel(request));
            request.IsSuccess = true;
            return;
        }
    }
}
