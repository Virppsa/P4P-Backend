using P4P.Data;
using P4P.Models;
using P4P.Repositories.Interfaces;

namespace P4P.Repositories;

public class UserRefreshTokenRepository : GenericRepository<UserRefreshToken>, IUserRefreshTokenRepository
{
    public UserRefreshTokenRepository(IP4PContext context) : base(context)
    {
    }
}
