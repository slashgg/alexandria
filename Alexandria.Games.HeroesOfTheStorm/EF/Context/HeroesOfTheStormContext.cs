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
    public DbSet<TournamentMap> TournamentMaps { get; set; }
    public DbSet<Map> Maps { get; set; }
    public DbSet<TournamentSettings> TournamentSettings { get; set; }
    public DbSet<MapBan> MapBans { get; set; }
    public DbSet<MatchReport> MatchReports { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.HasDefaultSchema("heroesofthestorm");
      base.OnModelCreating(builder);

      //builder.Entity<Map>().HasData(new List<Map>
      //{
      //  new Map("Alterac Pass"),
      //  new Map("Battlefield of Eternity"),
      //  new Map("Blackheart's Bay"),
      //  new Map("Braxis Holdout"),
      //  new Map("Cursed Hollow"),
      //  new Map("Dragon Shire"),
      //  new Map("Garden of Terror"),
      //  new Map("Hanamura Temple"),
      //  new Map("Haunted Mines"),
      //  new Map("Infernal Shrines"),
      //  new Map("Lost Caverns"),
      //  new Map("Sky Temple"),
      //  new Map("Tomb of the Spider Queen"),
      //  new Map("Towers of Doom"),
      //  new Map("Volskaya Foundry"),
      //  new Map("Warhead Junction"),
      //});

      var foreignKeys = builder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys());

      foreach (var key in foreignKeys)
      {
        key.DeleteBehavior = DeleteBehavior.Restrict;
      }
    }
  }
}
