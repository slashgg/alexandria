using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Competition
{
  [JsonSchema("CompetitionDetail")]
  [DataContract]
  public class Detail
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "name")]
    public string Name { get; set; }
    [DataMember(Name = "formattedName")]
    public string FormattedName { get; set; }
    [DataMember(Name = "title")]
    public string Title { get; set; }
    [DataMember(Name = "description")]
    public string Description { get; set; }
    [DataMember(Name = "titleCardURL")]
    public string TitleCardImageURL { get; set; }
    [DataMember(Name = "game")]
    public GameData Game { get; set; } = new GameData();

    [JsonSchema("CompetitionGame")]
    [DataContract]
    public class GameData
    {
      [DataMember(Name = "id")]
      public Guid Id { get; set; }
      [DataMember(Name = "name")]
      public string Name { get; set; }
    }
  }
}
