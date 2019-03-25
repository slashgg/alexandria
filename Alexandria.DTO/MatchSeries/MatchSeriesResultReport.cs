using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.MatchSeries
{
  [DataContract]
  [JsonSchema("MatchSeriesResultReport")]
  public class MatchSeriesResultReport
  {
    [DataMember(Name = "results")]
    public virtual IList<MatchResultReport> MatchResults { get; set; }
  }
}
