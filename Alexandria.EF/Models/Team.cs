using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Alexandria.Shared.Enums;

namespace Alexandria.EF.Models
{
  public class Team : BaseEntity
  {
    public string Name { get; set; }
    public string Abbreviation { get; set; }
    public string LogoURL { get; set; }
    public TeamState TeamState { get; set; }

    /* Foreign Keys */
    public Guid CompetitionId { get; set; }

    /* Relations */
    [ForeignKey("CompetitionId")]
    public virtual Competition Competition { get; set; }
    public virtual ICollection<TeamMembership> TeamMemberships { get; set; } = new List<TeamMembership>();
    public virtual ICollection<TournamentApplicationHistory> TournamentApplicationHistories { get; set; } = new List<TournamentApplicationHistory>();
    public virtual ICollection<TeamMembershipHistory> TeamMembershipHistories { get; set; } = new List<TeamMembershipHistory>();
  }
}
