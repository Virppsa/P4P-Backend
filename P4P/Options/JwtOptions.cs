namespace P4P.Options;

public class JwtOptions
{
    internal const string ObjectKey = "Jwt";

    public string Key { get; set; } = "";

    public string Issuer { get; set; } = "";

    public string Audience { get; set; } = "";

    public int JwtValidityInMinutes { get; set; }

    public int RefreshTokenValidityInMinutes { get; set; }
}
