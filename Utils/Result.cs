namespace PlaylistConverter.Utils;

public class Result<T>
{
    public T Value { get; private set; }
    
    public bool IsSuccess { get; private set; }
    
    public bool IsFailure => !IsSuccess;
    
    public Error Error { get; private set; }

    public static Result<T> Success(T value)
    {
        return new Result<T> { Value = value, IsSuccess = true };
    }

    public static Result<T> Failure(Error error)
    {
        return new Result<T> { Error = error, IsSuccess = false };
    }

    public static Result<T> Failure(int code, string message)
    {
        var error = new Error(message, code);
        return new Result<T>{Error = error, IsSuccess = false};
    }
}
