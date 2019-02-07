using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Tournament
{
  [DataContract]
  [JsonSchema("TournamentTeamMembership")]
  public class TeamMembership : Team.Membership
  {
    [DataMember(Name = "externalUserName")]
    public UserProfile.ExternalUserName ExternalUserName { get; set; }
  }
}
