namespace Mic.Api.Models
{
    public class ResponseResultDto<TResult>
    {
        public bool IsSuccess { get; set; }
        public TResult Result { get; set; }
        public string ErrorMessage { get; set; }
    }
}