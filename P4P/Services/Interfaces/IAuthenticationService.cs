using Microsoft.AspNetCore.Mvc;
using P4P.Models;
using P4P.Models.DTOs.Request;
using P4P.Models.DTOs.Response;

namespace P4P.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);

        Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest);

        Task<ActionResult> Logout(LogoutRequest logoutRequest);

        Task<GenericResponse> Verify(string token);

        string GenerateToken(User user);
    }
}
