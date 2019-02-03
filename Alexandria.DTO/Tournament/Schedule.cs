using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Tournament
{
  [DataContract]
  [JsonSchema("TournamentSchedule")]
  public class Schedule
  {
    [DataMember(Name = "rounds")]
    public List<ScheduleRound> Rounds { get; set; }
  }
}
