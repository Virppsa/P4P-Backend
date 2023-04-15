using P4P.Models;

namespace P4P.Repositories.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<bool> ExistsByEmail(string email);
}
