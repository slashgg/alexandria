using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.DTO.Util
{
  [DataContract]
  public class Cron
  {
    [DataMember(Name = "cron")]
    public Shared.Enums.Cron CronType { get; set; }
  }
}
