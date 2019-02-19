using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Alexandria.Shared.Enums;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Tournament
{
  [DataContract]
  [JsonSchema("TournamentStanding")]
  public class Standing<T>
  {
    [DataMember(Name = "type")]
    public TournamentType Type { get; set; }
    [DataMember(Name = "standings")]
    public List<T> Standings { get; set; }

    public Standing()
    {
    }

    public Standing(TournamentType type, List<T> standings)
    {
      this.Type = type;
      this.Standings = standings;
    }
  }
}
