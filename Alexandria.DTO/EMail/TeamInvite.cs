using System;
using System.Collections.Generic;
using System.Text;

namespace Alexandria.DTO.EMail
{
  public class TeamInvite
  {
    public Guid TeamId { get; set; }
    public Guid CompetitionId { get; set; }
    public string TeamName { get; set; }
    public string CompetitionName { get; set; }
    public string TeamSlug { get; set; }
    public string CompetitionSlug { get; set; }
    public Guid InviteId { get; set; }

    public TeamInvite()
    {

    }

    public TeamInvite(Guid inviteId, Guid competitionId, string competitionName, string competitionSlug, Guid teamId, string teamName, string teamSlug)
    {
      this.InviteId = inviteId;
      this.CompetitionId = competitionId;
      this.CompetitionName = competitionName;
      this.CompetitionSlug = competitionSlug;
      this.TeamId = teamId;
      this.TeamName = teamName;
      this.TeamSlug = teamSlug;
    }
  }
}
