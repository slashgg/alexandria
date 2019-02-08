using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.Shared.Enums
{

  public enum Cron
  {
    [EnumMember(Value = "external-user-name")]
    ExternalUserName = 1,
  }
}
