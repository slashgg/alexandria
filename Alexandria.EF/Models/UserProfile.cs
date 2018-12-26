using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
    public DateTime? Birthday { get; set; }
    public string AvatarURL { get; set; }

    /* Relations */
    public virtual ICollection<ExternalAccount> ExternalAccounts { get; set; } = new List<ExternalAccount>();
    public virtual ICollection<TeamMembership> TeamMemberships { get; set; } = new List<TeamMembership>();
    public virtual ICollection<TeamMembershipHistory> TeamMembershipHistories { get; set; } = new List<TeamMembershipHistory>();
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    public virtual ICollection<TeamInvite> TeamInvites { get; set; } = new List<TeamInvite>();

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

    public bool HasTeam(Guid competition)
    {
      return TeamMemberships.Any(m => m.Team.CompetitionId == competition);
    }

    public bool HasPermission(string ARN)
    {
      return this.Permissions.Any(c => c.ARN == ARN);
    }
  }
}
