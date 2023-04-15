using P4P.Data;
using P4P.Models;
using P4P.Repositories.Interfaces;

namespace P4P.Repositories;

public class LikeRepository : GenericRepository<Like>, ILikeRepository
{
    public LikeRepository(IP4PContext context) : base(context)
    {
    }
}
