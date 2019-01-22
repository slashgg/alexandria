using System.Linq;
using Alexandria.EF.Converters;
using Alexandria.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.EF.Context
{
  public class AlexandriaContext : DbContext
  {
    public AlexandriaContext(DbContextOptions<AlexandriaContext> options) : base(options)
    {
    }

    public DbSet<ExternalAccount> ExternalAccount { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Competition> Competitions { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamMembership> TeamMemberships { get; set; }
    public DbSet<TeamInvite> TeamInvites { get; set; }
    public DbSet<TeamRole> TeamRoles { get; set; }
    public DbSet<TournamentApplication> TournamentApplications { get; set; }
    public DbSet<TournamentApplicationQuestion> TournamentApplicationQuestions { get; set; }
    public DbSet<TournamentApplicationQuestionAnswer> TournamentApplicationQuestionAnswers { get; set; }
    public DbSet<TournamentParticipation> TournamentParticipations { get; set; }
    public DbSet<TournamentParticipationHistory> TournamentParticipationHistories { get; set; }
    public DbSet<CompetitionLevel> CompetitionLevels { get; set; }
    public DbSet<FavoriteCompetition> FavoriteCompetitions { get; set; }
    public DbSet<ProfanityFilter> ProfanityFilters { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {

      base.OnModelCreating(builder);

      builder.Entity<UserProfile>().HasIndex(p => p.UserName).IsUnique();
      builder.Entity<UserProfile>().HasIndex(p => p.Email).IsUnique();

      builder.Entity<TeamRole>().Property(b => b.Permissions)
                                .HasConversion(AlexandriaValueConverter.SplitStringConverter);

      builder.Entity<TournamentApplicationQuestion>().Property(b => b.SelectOptions)
                                                     .HasConversion(AlexandriaValueConverter.SplitStringConverter);

      builder.Entity<Competition>().Property(b => b.MinTeamSize)
                                   .HasDefaultValue(1);

      builder.Entity<Competition>().HasIndex(b => b.Slug);
      builder.Entity<Competition>().HasMany(c => c.TeamRoles)
                                   .WithOne(tr => tr.Competition)
                                   .HasForeignKey(tr => tr.CompetitionId);



      builder.Entity<Team>().HasIndex(b => b.Slug);
      builder.Entity<Tournament>().HasIndex(b => b.Slug);

      var foreignKeys = builder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys());

      foreach (var key in foreignKeys)
      {
        key.DeleteBehavior = DeleteBehavior.Restrict;
      }
    }
  }
}
