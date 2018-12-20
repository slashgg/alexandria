using Alexandria.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.EF.Context
{
  public class AlexandriaContext : DbContext
  {
    public AlexandriaContext(DbContextOptions<AlexandriaContext> options) : base(options)
    {
    }

    public DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<UserProfile>().HasIndex(p => p.DisplayName).IsUnique();
      builder.Entity<UserProfile>().HasIndex(p => p.Email).IsUnique();
    }
  }
}
