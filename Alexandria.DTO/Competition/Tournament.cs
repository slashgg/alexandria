using System;
using System.Runtime.Serialization;
using Alexandria.Shared.Enums;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Competition
{
  [JsonSchema("CompetitionTournamentDetail")]
  [DataContract]
  public class Tournament
  {
    [DataMember(Name="id")]
    public Guid Id { get; set; }
    [DataMember(Name="name")]
    public string Name { get; set; }
    [DataMember(Name = "startDate")]
    public DateTimeOffset? StartDate { get; set; }
    [DataMember(Name = "endDate")]
    public DateTimeOffset? EndDate { get; set; }
    [DataMember(Name = "applicationRequired")]
    public bool ApplicationRequired { get; set; }
    [DataMember(Name = "canSignup")]
    public bool CanSignup { get; set; }
    [DataMember(Name = "signupOpenDate")]
    public DateTimeOffset? SignupOpenDate { get; set; }
    [DataMember(Name = "signupCloseDate")]
    public DateTimeOffset? SignupCloseDate { get; set; }
    [DataMember(Name = "tokenImageURL")]
    public string TokenImageURL { get; set; }
    [DataMember(Name = "state")]
    public TournamentState State { get; set; }
    [DataMember(Name = "type")]
    public TournamentType Type { get; set; }
    [DataMember(Name = "parentTournamentId")]
    public Guid? ParentTournamentId { get; set; }
    [DataMember(Name = "parentTournamentName")]
    public string ParentTournamentName { get; set; }
  }
}
