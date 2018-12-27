using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.Shared.Enums
{
  public enum TournamentState
  {
    [EnumMember(Value = "pending")]
    Pending = 1,
    [EnumMember(Value = "preSeason")]
    PreSeason = 2,
    [EnumMember(Value = "active")]
    Active = 3,
    [EnumMember(Value = "postSeason")]
    PostSeason = 4,
    [EnumMember(Value = "concluded")]
    Concluded = 5
  }
}
