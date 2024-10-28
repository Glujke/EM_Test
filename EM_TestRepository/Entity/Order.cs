namespace EM_TestRepository.Entity
{
    public class Order
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public float Weight { get; set; }

        public Location Location{ get; set; }
        public int LocationId { get; set; }
        public DateTime Date { get; set; }
    }
}
