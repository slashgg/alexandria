using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Alexandria.Shared.Enums;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Tournament
{
  [JsonSchema("TournamentTournamentApplication")]
  [DataContract]
  public class TournamentApplication
  {
    [DataMember(Name = "tournamentId")]
    public Guid TournamentId { get; set; }
    [DataMember(Name = "teamId")]
    public Guid TeamId { get; set; }
    [DataMember(Name = "state")]
    public TournamentApplicationState State { get; set; }
    [DataMember(Name = "answers")]
    public List<TournamentApplicationQuestionAnswer> Answers { get; set; } = new List<TournamentApplicationQuestionAnswer>();
  }
}
