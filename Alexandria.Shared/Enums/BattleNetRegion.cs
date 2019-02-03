using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.Shared.Enums
{
  public enum BattleNetRegion
  {
    [EnumMember(Value = "north-amertica")]
    NorthAmerica = 1,
    [EnumMember(Value = "europe")]
    Europe = 2,
    [EnumMember(Value = "korea")]
    Korea = 3,
    [EnumMember(Value = "china")]
    China = 4
  }
}
