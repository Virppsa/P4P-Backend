namespace P4P.Models.DTOs.Response;

public class LoginResponse : GenericDataResponse<UserResponse>
{
    public string Token { get; set; }

    public string RefreshToken { get; set; }
}
