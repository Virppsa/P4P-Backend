using Microsoft.EntityFrameworkCore;

namespace P4P.Data
{
    public interface IP4PContext
    {
        public DbSet<P4P.Models.Comment> Comment { get; set; }
        public DbSet<P4P.Models.Location> Location { get; set; }
        public DbSet<P4P.Models.Post> Post { get; set; }
        public DbSet<P4P.Models.Like> Like { get; set; }
        public DbSet<P4P.Models.User> User { get; set; }
        public DbSet<P4P.Models.UserRefreshToken> RefreshToken { get; set; }
        DbContext Instance { get; }
    }
}
