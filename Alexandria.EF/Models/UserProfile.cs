using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Alexandria.Shared.Enums;

namespace Alexandria.EF.Models
{
  public class UserProfile : BaseEntity
  {
    [Required]
    public string UserName { get; set; }
    [Required]
    public string DisplayName { get; set; }
    [Required]
    public string Email { get; set; }
    public DateTimeOffset? Birthday { get; set; }
    public string AvatarURL { get; set; }

    /* Relations */
    public virtual ICollection<ExternalAccount> ExternalAccounts { get; set; } = new List<ExternalAccount>();
    public virtual ICollection<TeamMembership> TeamMemberships { get; set; } = new List<TeamMembership>();
    public virtual ICollection<TeamMembershipHistory> TeamMembershipHistories { get; set; } = new List<TeamMembershipHistory>();
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    public virtual ICollection<TeamInvite> TeamInvites { get; set; } = new List<TeamInvite>();
    public virtual ICollection<FavoriteCompetition> FavoriteCompetitions { get; set; } = new List<FavoriteCompetition>();
    public virtual ICollection<PlayerRanking> PlayerRankings { get; set; } = new List<PlayerRanking>();
    public virtual ICollection<ExternalUserName> ExternalUserNames { get; set; } = new List<ExternalUserName>();

    public UserProfile(Guid id, string userName, string email)
    {
      var splitName = userName.Split('#').ToList();
      splitName.RemoveAt(splitName.Count - 1);
      var displayName = string.Join('#', splitName);

      this.Id = id;
      this.UserName = userName;
      this.DisplayName = displayName;
      this.Email = email;
    }

    public bool HasTeam(Guid competitionId)
    {
      return TeamMemberships.Any(m => m.Team.CompetitionId == competitionId);
    }

    public bool HasPermission(string ARN)
    {
      return this.Permissions.Any(c => c.ARN == ARN);
    }

    public bool HasExternalUserName(Game game)
    {
      return this.ExternalUserNames.Any(eun => eun.Game.Id == game.Id);
    }

    public bool HasExternalUserName(Guid gameId)
    {
      return this.ExternalUserNames.Any(eun => eun.GameId == gameId);
    }

    public ExternalUserName GetUserNameForGame(Game game)
    {
      return this.ExternalUserNames.FirstOrDefault(eun => eun.GameId == game.Id);
    }

    public ExternalUserName GetUserNameForGame(Guid gameId)
    {
      return this.ExternalUserNames.FirstOrDefault(eun => eun.GameId == gameId);
    }

    public ExternalAccount GetExternalAccount(ExternalProvider provider)
    {
      return this.ExternalAccounts.FirstOrDefault(ea => ea.Provider == provider);
    }

    public bool TryGetExternalAccount(ExternalProvider provider, out ExternalAccount account)
    {
      account = this.ExternalAccounts.FirstOrDefault(ea => ea.Provider == provider);
      return account != null;
    }
  }
}
