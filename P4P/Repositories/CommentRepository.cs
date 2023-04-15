using P4P.Data;
using P4P.Models;
using P4P.Repositories.Interfaces;

namespace P4P.Repositories;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(IP4PContext context) : base(context)
    {
    }
}
