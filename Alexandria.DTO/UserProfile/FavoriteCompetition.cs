using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.UserProfile
{
  [DataContract]
  [JsonSchema("UserProfileFavoriteCompetition")]
  public class FavoriteCompetition
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "name")]
    public string Name { get; set; }
    [DataMember(Name = "competitionId")]
    public Guid CompetitionId { get; set; }
  }
}
