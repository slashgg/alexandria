using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Tournament
{
  [JsonSchema("TournamentTeamTournamentApplicationRequest")]
  [DataContract]
  public class TeamTournamentApplicationRequest
  {
    [DataMember(Name = "tournamentId")]
    public Guid TournamentId { get; set; }
    [DataMember(Name = "answers")]
    public List<TournamentApplicationQuestionAnswer> Answers { get; set; } = new List<TournamentApplicationQuestionAnswer>();
  }
}
