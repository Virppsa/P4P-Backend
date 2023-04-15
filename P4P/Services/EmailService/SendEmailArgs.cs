namespace P4P.Services.EmailService;

public class SendEmailArgs : EventArgs
{
    public string Email;
    public string Token;
    public string Type;

    public SendEmailArgs(string email, string token, string type)
    {
        Email = email;
        Token = token;
        Type = type;
    }
}
