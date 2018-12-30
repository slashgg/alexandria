using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Competition
{
  [DataContract]
  [JsonSchema("CompetitionTournamentApplication")]
  public class TournamentApplication
  {
    [DataMember(Name = "tournament")]
    public TournamentData Tournament { get; set; } = new TournamentData();
    [DataMember(Name = "signupOpenDate")]
    public DateTimeOffset? SignupOpenDate { get; set; }
    [DataMember(Name = "signupCloseDate")]
    public DateTimeOffset? SignupCloseDate { get; set; }
    [DataMember(Name = "applicationRequired")]
    public bool ApplicationRequired { get; set; }
    [DataMember(Name = "questions")]
    public List<TournamentApplicationQuestion> Questions { get; set; } = new List<TournamentApplicationQuestion>();

    [DataContract]
    [JsonSchema("CompetitionTournamentApplicationTournamentData")]
    public class TournamentData
    {
      [DataMember(Name = "id")]
      public Guid Id { get; set; }
      [DataMember(Name = "name")]
      public string Name { get; set; }
      [DataMember(Name = "tokenImageURL")]
      public string TokenImageURL { get; set; }
    }
  }
}
