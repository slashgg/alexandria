using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.UserProfile
{
  [DataContract]
  [JsonSchema("UserProfileAddFavoriteCompetition")]
  public class AddFavoriteCompetition
  {
    [DataMember(Name = "competitionId")]
    public Guid CompetitionId { get; set; }
  }
}
