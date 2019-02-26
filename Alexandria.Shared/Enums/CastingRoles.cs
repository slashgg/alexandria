using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.Shared.Enums
{
  public enum CastingRole
  {
    [EnumMember(Value = "Color")]
    Color = 1,
    [EnumMember(Value = "play-by-play")]
    PlayByPlay = 2,
    [EnumMember(Value = "host")]
    Host = 3,
  }
}
