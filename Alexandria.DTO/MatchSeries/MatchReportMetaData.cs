using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.MatchSeries
{
  [DataContract]
  [JsonSchema("MatchSeriesResultMetaData")]
  public class MatchReportMetaData
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "participants")]
    public List<Team.Info> Participants { get; set; }
    [DataMember(Name = "matches")]
    public List<MatchInfo> Matches { get; set; }
    [DataMember(Name = "tournament")]
    public virtual TournamentMatchResultMetaData Tournament { get; set; }
    [DataMember(Name = "gameSpecific")]
    public virtual object GameSpecific { get; set; }
    [DataMember(Name = "game")]
    public string Game { get; set; }
    [DataMember(Name = "submitURL")]
    public string SubmitURL { get; set; }
    [DataMember(Name = "minMatches")]
    public int MinMatches { get; set; }
    [DataMember(Name = "maxMatches")]
    public int MaxMatches { get; set; }
    [DataMember(Name = "reportingType")]
    public string ReportingType { get; set; }

    public virtual string CreateSubmitURL()
    {
      return $"/match-series/{this.Id}/reporting?type={this.ReportingType}";
    }
  }

  [DataContract]
  [JsonSchema("MatchSeriesTournamentMatchResultMetaData")]
  public class TournamentMatchResultMetaData
  {

  }
}
