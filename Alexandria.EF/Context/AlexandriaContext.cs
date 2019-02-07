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
    public DbSet<PlayerRanking> PlayerRankings { get; set; }
    public DbSet<PlayerRankingGroup> playerRankingGroups { get; set; }
    public DbSet<CompetitionRankingGroupMembership> CompetitionRankingGroupMemberships { get; set; }
    public DbSet<TournamentRound> TournamentRounds { get; set; }
    public DbSet<MatchSeries> MatchSeries { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<MatchParticipant> MatchParticipants { get; set; }
    public DbSet<MatchParticipantResult> MatchParticipantResults { get; set; }
    public DbSet<MatchSeriesScheduleRequest> MatchSeriesScheduleRequests { get; set; }
    public DbSet<ExternalUserName> ExternalUserName { get; set; }
    public DbSet<GameExternalUserNameGenerator> GameUserNameGenerators { get; set; }
    public DbSet<ExternalUserNameGenerator> ExternalUserNameGenerators { get; set; }

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

      builder.Entity<Game>().HasOne(g => g.GameExternalUserNameGenerator).WithOne(geung => geung.Game);

      builder.Entity<Team>().HasIndex(b => b.Slug);
      builder.Entity<Team>().HasMany(t => t.OriginatingScheduleRequests).WithOne(osr => osr.OriginTeam).HasForeignKey(osr => osr.OriginTeamId);
      builder.Entity<Team>().HasMany(t => t.TargetedScheduleRequests).WithOne(tsr => tsr.TargetTeam).HasForeignKey(tsr => tsr.TargetTeamId);

      builder.Entity<Tournament>().HasIndex(b => b.Slug);
      builder.Entity<Tournament>().HasMany(t => t.Tournaments).WithOne(t1 => t1.ParentTournament).HasForeignKey(t1 => t1.ParentTournamentId);

      builder.Entity<MatchSeries>().HasMany(ms => ms.ScheduleRequests).WithOne(sr => sr.MatchSeries).HasForeignKey(sr => sr.MatchSeriesId);

      var foreignKeys = builder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys());

      foreach (var key in foreignKeys)
      {
        key.DeleteBehavior = DeleteBehavior.Restrict;
      }
    }
  }
}
