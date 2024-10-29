using EM_Test.Models;
using EM_TestRepository.Entity;

namespace EM_Test.Mappers
{
    public static class OrderMapper
    {
        public static OrderModel ToApiModel(Order order)
        {
            return new OrderModel()
            {
                Id = order.Id,
                Number = order.Number,
                Location = LocationMapper.ToApiModel(order.Location),
                Weight = order.Weight,
                Date = order.Date
            };
        }

        public static Order FromApiModel(OrderModel order)
        {
            return new Order()
            {
                Id = order.Id,
                Number = order.Number,
                Location = LocationMapper.FromApiModel(order.Location),
                LocationId = order.Location.Id,
                Weight = order.Weight,
                Date = order.Date
            };
        }
        public static IEnumerable<OrderModel> ToApiModel(IEnumerable<Order> orders)
        {
            return orders.Select(order => ToApiModel(order));
        }

        public static IEnumerable<Order> FromApiModel(IEnumerable<OrderModel> orders)
        {
            return orders.Select(orderModel => FromApiModel(orderModel));
        }
    }
}
