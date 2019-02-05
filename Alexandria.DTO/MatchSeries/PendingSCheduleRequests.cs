using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.MatchSeries
{
  [DataContract]
  [JsonSchema("MatchSeriesPendingScheduleRequests")]
  public class PendingScheduleRequests
  {
    [DataMember(Name = "outbound")]
    public IList<ScheduleRequest> Outbound { get; set; }
    [DataMember(Name = "inbound")]
    public IList<ScheduleRequest> Inbound { get; set; }

    public PendingScheduleRequests() { }
    public PendingScheduleRequests(IList<ScheduleRequest> outbound, IList<ScheduleRequest> inbound)
    {
      this.Outbound = outbound;
      this.Inbound = inbound;
    }
  }
}
