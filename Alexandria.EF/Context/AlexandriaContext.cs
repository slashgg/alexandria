using System.Linq;
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
    public DbSet<Game> Games { get; set; }
    public DbSet<Competition> Competitions { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamMembership> TeamMemberships { get; set; }
    public DbSet<TeamInvite> TeamInvites { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {

      base.OnModelCreating(builder);

      builder.Entity<UserProfile>().HasIndex(p => p.DisplayName).IsUnique();
      builder.Entity<UserProfile>().HasIndex(p => p.Email).IsUnique();

      var foreignKeys = builder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys());

      foreach (var key in foreignKeys)
      {
        key.DeleteBehavior = DeleteBehavior.Restrict;
      }
    }
  }
}
