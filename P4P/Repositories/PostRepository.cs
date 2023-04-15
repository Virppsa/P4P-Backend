using Microsoft.EntityFrameworkCore;
using P4P.Data;
using P4P.Extensions;
using P4P.Filters;
using P4P.Models;
using P4P.Repositories.Interfaces;
using P4P.Wrappers;

namespace P4P.Repositories;

public class PostRepository : GenericRepository<Post>, IPostRepository
{
    public PostRepository(IP4PContext context) : base(context)
    {
    }
}
