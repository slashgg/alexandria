using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Casting
{
  [DataContract]
  [JsonSchema("GameCast")]
  public class Cast
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "streamURL")]
    public string StreamURL { get; set; }
    [DataMember(Name = "VODURL")]
    public string VODURL { get; set; }
    [DataMember(Name = "startsAt")]
    public DateTimeOffset? StartsAt { get; set; }
    [DataMember(Name = "matchSeries")]
    public MatchSeries.Detail MatchSeries { get; set; }
    [DataMember(Name = "competition")]
    public Competition.Info Competition { get; set; }
    [DataMember(Name = "casters")]
    public List<CastMember> CastMembers { get; set; }
  }
}
