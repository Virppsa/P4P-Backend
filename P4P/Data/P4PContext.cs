using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using P4P.Models;

namespace P4P.Data;

public class P4PContext : DbContext, IP4PContext
{
    public P4PContext(DbContextOptions<P4PContext> options)
        : base(options)
    {
    }

    public DbContext Instance => this;
    public DbSet<P4P.Models.Comment> Comment { get; set; } = default!;
    public DbSet<P4P.Models.Location> Location { get; set; } = default!;
    public DbSet<P4P.Models.Post> Post { get; set; } = default!;
    public DbSet<P4P.Models.Like> Like { get; set; } = default!;
    public DbSet<P4P.Models.User> User { get; set; } = default!;
    public DbSet<P4P.Models.UserRefreshToken> RefreshToken { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>().Property(p => p.Ratings)
            .HasConversion(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
            v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null));
    }
}
 