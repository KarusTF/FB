namespace FizzBuzz.DTOs
{
    public class ApiErrorResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
}