using EM_Test.Models;
using EM_TestRepository.Entity;

namespace EM_Test.Mappers
{
    public static class LocationMapper
    {
        public static LocationModel ToApiModel(Location location)
        {
            return new LocationModel()
            {
                Id = location.Id,
                Name= location.Name
            };
        }

        public static Location FromApiModel(LocationModel location)
        {
            return new Location()
            {
                Id = location.Id,
                Name = location.Name
            };
        }
        public static IEnumerable<LocationModel> ToApiModel(IEnumerable<Location> locations)
        {
            return locations.Select(order => ToApiModel(order));
        }

        public static IEnumerable<Location> FromApiModel(IEnumerable<LocationModel> locations)
        {
            return locations.Select(orderModel => FromApiModel(orderModel));
        }
    }
}
