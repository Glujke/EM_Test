using EM_TestRepository.Entity;

namespace EM_Test.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public float Weight { get; set; }
        public LocationModel Location { get; set; }
        public DateTime Date { get; set; }
    }
}
