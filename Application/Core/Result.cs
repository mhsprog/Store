namespace Application.Core
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; }
        public string Message { get; set; }

        public static Result<T> Success(T value, string message = null) => new() { IsSuccess = true, Value = value, Message = message };
        public static Result<T> Failure(string message) => new() { IsSuccess = false, Message = message };
    }
}
