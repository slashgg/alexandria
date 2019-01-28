using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.Games.HeroesOfTheStorm.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Games.HeroesOfTheStorm.EF.Context
{
  public class HeroesOfTheStormContext : DbContext
  {
    public HeroesOfTheStormContext(DbContextOptions<HeroesOfTheStormContext> options) : base(options) { }

    public DbSet<ExternalRanking> ExternalRankings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.HasDefaultSchema("heroesofthestorm");
      base.OnModelCreating(builder);

      var foreignKeys = builder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys());

      foreach (var key in foreignKeys)
      {
        key.DeleteBehavior = DeleteBehavior.Restrict;
      }
    }
  }
}
