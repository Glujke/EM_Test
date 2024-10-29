using EM_Test.Models;
using EM_TestRepository.Entity;

namespace EM_Test.Mappers
{
    public static class RequestMapper
    {
        public static RequestModel ToApiModel(Request request)
        {
            return new RequestModel()
            {
                Id = request.Id,
                LocationId = request.LocationId,
                RequestTime = request.RequestTime,
                Answer = request.Answer
            };
        }

        public static Request FromApiModel(RequestModel request)
        {
            return new Request()
            {
                Id = request.Id,
                LocationId = request.LocationId,
                RequestTime = request.RequestTime,
                Answer = request.Answer
            };
        }
        public static IEnumerable<RequestModel> ToApiModel(IEnumerable<Request> requests)
        {
            return requests.Select(order => ToApiModel(order));
        }

        public static IEnumerable<Request> FromApiModel(IEnumerable<RequestModel> requests)
        {
            return requests.Select(orderModel => FromApiModel(orderModel));
        }
    }
}
