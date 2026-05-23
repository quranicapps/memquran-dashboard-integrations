namespace Integrations.Core;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string ErrorMessage { get; }
    public Exception? Exception { get; }

    private Result(bool isSuccess, T value, string errorMessage = "", Exception? exception = null)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = errorMessage;
        Exception = exception;
    }

    public static Result<T> Success() => new(true, default!);
    public static Result<T> SuccessObjectResult(T value) => new(true, value);
    public static Result<T> Failure(string errorMessage) => new(false, default!, errorMessage);
    public static Result<T> Failure(string errorMessage, Exception exception) => new(false, default!, errorMessage, exception);
    public static Result<T> FailureObjectResult(T value, string error) => new(false, value, error);
    public static Result<T> FailureObjectResult(T value, string error, Exception exception) => new(false, value, error, exception);
}