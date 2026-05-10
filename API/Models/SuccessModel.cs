namespace API.Models
{
    public class SuccessModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int SuccessId { get; set; }
        public dynamic Data { get; set; }
    }
}
