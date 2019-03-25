using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.Games.SuperSmashBros.Configuration;
using Alexandria.Games.SuperSmashBros.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Games.SuperSmashBros.EF.Context
{
  public class SuperSmashBrosContext : DbContext
  {
    public SuperSmashBrosContext(DbContextOptions<SuperSmashBrosContext> options) : base(options)
    {
    }

    public DbSet<MatchReport> MatchReports { get; set; }
    public DbSet<Fighter> Fighters { get; set; }
    public DbSet<FighterPick> FighterPicks { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.HasDefaultSchema(Constants.Schema);
      base.OnModelCreating(builder);

      var foreignKeys = builder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys());

      foreach (var key in foreignKeys)
      {
        key.DeleteBehavior = DeleteBehavior.Restrict;
      }
    }
  }
}
