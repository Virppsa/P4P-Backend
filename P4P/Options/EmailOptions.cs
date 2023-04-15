namespace P4P.Options;

public class EmailOptions
{
    internal const string ObjectKey = "EmailConnection";

    public string Host { get; set; } = "";

    public int Port { get; set; }

    public bool EnableSsl { get; set; }

    public string FromName { get; set; } = "";

    public string FromAddress { get; set; } = "";

    public string Password { get; set; } = "";
    
    public string LinkDomain { get; set; } = "";
}
