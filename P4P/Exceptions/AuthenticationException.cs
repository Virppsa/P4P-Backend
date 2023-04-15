namespace P4P.Exceptions;

public class AuthenticationException : Exception
{
    public readonly string email;
    public readonly DateTime time;

    public AuthenticationException() {}

    public AuthenticationException(string message) : base(message) {}

    public AuthenticationException(string message, string email) : base(message)
    {
        this.email = email;
        this.time = DateTime.UtcNow;
    }

    public AuthenticationException(string message, Exception innerException) : base(message, innerException) {}    
}