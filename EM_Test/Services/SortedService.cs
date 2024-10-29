using EM_Test.Controllers;
using EM_Test.Mappers;
using EM_Test.Models;
using EM_TestRepository.Entity;
using EM_TestRepository.Repository;
using Microsoft.AspNetCore.Mvc;
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


        public async Task<RequestModel> Sort(RequestModel request)
        {
            var orders = await _sorter.Sort(request.LocationId, request.RequestTime);
            if (orders == null || orders.Count() == 0)
            {
                _logger.LogInformation($"Request {request.LocationId} from date {request.RequestTime}. Answer: Not found");
                request.Answer = "Not found";
                await _requestRepository.CreateAsync(RequestMapper.FromApiModel(request));
                request.IsSuccess = false;
                return request;
            }
            _logger.LogInformation($"Request {request.LocationId} from date {request.RequestTime}. Answer:{JsonSerializer.Serialize(orders)}");
            request.AnswerModel = OrderMapper.ToApiModel(orders);
            request.Answer = JsonSerializer.Serialize(orders);
            await _requestRepository.CreateAsync(RequestMapper.FromApiModel(request));
            request.IsSuccess = true;
            return request;
        }
    }
}
