using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Alexandria.Shared.Enums;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Tournament
{
  [DataContract]
  [JsonSchema("TournamentTeamParticipation")]
  public class TeamParticipation
  {
    [DataMember(Name = "name")]
    public string Name { get; set; }
    [DataMember(Name = "id")]
    public string Id { get; set; }
    [DataMember(Name = "logoURL")]
    public string LogoURL { get; set; }
    [DataMember(Name = "abbreviation")]
    public string Abbreviation { get; set; }
    [DataMember(Name = "state")]
    public TournamentApplicationState State { get; set; }
    [DataMember(Name = "members")]
    public List<Tournament.TeamMembership> Memberships { get; set; } = new List<Tournament.TeamMembership>();
  }
}
