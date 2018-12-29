using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Alexandria.Shared.Enums;
using Alexandria.Shared.Utils;

namespace Alexandria.EF.Models
{
  [ProtectedResource("team")]
  public class Team : BaseEntity
  {
    public string Name { get; set; }
    public string Abbreviation { get; set; }
    public string LogoURL { get; set; }
    public TeamState TeamState { get; set; } = TeamState.Active;

    /* Foreign Keys */
    public Guid CompetitionId { get; set; }

    /* Relations */
    [ForeignKey("CompetitionId")]
    public virtual Competition Competition { get; set; }
    public virtual ICollection<TeamInvite> TeamInvites { get; set; } = new List<TeamInvite>();
    public virtual ICollection<TeamMembership> TeamMemberships { get; set; } = new List<TeamMembership>();
    public virtual ICollection<TournamentApplicationHistory> TournamentApplicationHistories { get; set; } = new List<TournamentApplicationHistory>();
    public virtual ICollection<TeamMembershipHistory> TeamMembershipHistories { get; set; } = new List<TeamMembershipHistory>();

    public Team(string name)
    {
      this.Name = name;
      this.Abbreviation = string.Join("", name.Where(char.IsUpper).ToArray());
    }

    public Team(Guid competitionId, string name)
    {
      this.CompetitionId = competitionId;
      this.Name = name;
      this.Abbreviation = string.Join("", name.Where(char.IsUpper).ToArray());
    }

    public bool HasMember(Guid userId)
    {
      return this.TeamMemberships.Any(m => m.UserProfileId == userId);
    }

    public bool HasInvite(Guid userId)
    {
      return this.TeamInvites.Any(i => i.UserProfileId == userId && i.State == InviteState.Pending);
    }

    public bool HasInvite(string email)
    {
      return this.TeamInvites.Any(i => i.Email == email && i.State == InviteState.Pending);
    }

    public TeamMembership AddMember(Guid userId, Guid roleId, string notes = "")
    {
      if (this.HasMember(userId))
      {
        return null;
      }

      var membership = new TeamMembership(this.Id, userId, roleId);
      this.TeamMemberships.Add(membership);
      this.TeamMembershipHistories.Add(new TeamMembershipHistory(userId, notes));

      return membership;
    }

    public void RemoveMember(Guid userId, string notes = "")
    {
      var membership = this.TeamMemberships.FirstOrDefault(r => r.UserProfileId == userId);
      if (membership == null)
      {
        return;
      }

      this.TeamMemberships.Remove(membership);
      this.TeamMembershipHistories.Add(new TeamMembershipHistory(userId, notes));
    }
  }
}
