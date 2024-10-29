namespace EM_Test.Models
{
    public class RequestModel
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public DateTime RequestTime { get; set; }
        public string Answer { get; set; }
        public IEnumerable<OrderModel> AnswerModel { get; set; }
        public bool IsSuccess { get; set; }
    }
}
