using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Tournament
{
  [DataContract]
  [JsonSchema("TournamentRound")]
  public class RoundDetail
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "name")]
    public string Name { get; set; }
    [DataMember(Name = "slug")]
    public string Slug { get; set; }
    [DataMember(Name = "startDate")]
    public DateTimeOffset? StartDate { get; set; }
    [DataMember(Name = "endDate")]
    public DateTimeOffset? EndDate { get; set; }
    [DataMember(Name = "seriesCount")]
    public int SeriesCount { get; set; }
    [DataMember(Name = "seriesGameCount")]
    public int SeriesGameCount { get; set; }
  }
}
