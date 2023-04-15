namespace P4P.Exceptions;

public class HttpException : Exception
{
    public int StatusCode { get; set; }

    public string Message { get; set; }

    public HttpException(int statusCode, string message, Exception? innerException = null) :
        base(message, innerException)
    {
        StatusCode = statusCode;
        Message = message;
    }
}
