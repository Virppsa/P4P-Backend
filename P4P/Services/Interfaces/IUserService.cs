using P4P.Models;
using P4P.Models.DTOs.Request;
using P4P.Models.DTOs.Response;

namespace P4P.Services.Interfaces;

public interface IUserService
{
    Task<UserResponse> Create(RegisterRequest registerRequest);

    Task<User?> GetSingle(int id);

    Task<bool> ExistsByEmail(string email);

    Task<User?> GetCurrent();
}
