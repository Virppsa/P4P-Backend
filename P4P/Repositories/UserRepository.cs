using Microsoft.EntityFrameworkCore;
using P4P.Data;
using P4P.Models;
using P4P.Repositories.Interfaces;

namespace P4P.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(IP4PContext context) : base(context)
    {
    }

    public async Task<bool> ExistsByEmail(string email)
    {
        return await DbSet.AnyAsync(u => u.Email == email);
    }
}
