namespace PlaylistConverter.Utils;

public class Error
{
    public string Message { get; }
    
    public int Code { get; }

    public Error(string message, int code)
    {
        Message = message;
        Code = code;
    }
}