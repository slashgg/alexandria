using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.Games.HeroesOfTheStorm.DTO.Cron
{
  [DataContract]
  public class Identifier
  {
    [DataMember(Name = "cronIdentifier")]
    public string CronIdentifier { get; set; }
  }
}
