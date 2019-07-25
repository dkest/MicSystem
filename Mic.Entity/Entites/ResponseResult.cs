namespace Mic.Entity
{
    public class ResponseResult<TResult>
    {
        public bool IsSuccess { get; set; }

        public TResult Result { get; set; }

        public string ErrorMessage { get; set; }
    }
}
