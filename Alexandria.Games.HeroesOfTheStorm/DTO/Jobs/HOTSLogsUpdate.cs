using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.Games.HeroesOfTheStorm.DTO.Jobs
{
  [DataContract]
  public class HOTSLogsUpdate
  {
    [DataMember(Name = "userProfileId")]
    public Guid UserProfileId { get; set; }
    [DataMember(Name = "region")]
    public Shared.Enums.BattleNetRegion Region { get; set; }

    public HOTSLogsUpdate(Guid userId, Shared.Enums.BattleNetRegion region = Shared.Enums.BattleNetRegion.NorthAmerica)
    {
      this.UserProfileId = userId;
      this.Region = region;
    }
  }
}
