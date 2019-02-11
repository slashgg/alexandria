using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.Games.HeroesOfTheStorm.DTO.Tournament
{
  [DataContract]
  [JsonSchema("HeroesOfTheStormResultMetaData")]
  public class ResultMetaData
  {
    [DataMember(Name = "mapBanCount")]
    public int MapBanCount { get; set; }
    [DataMember(Name = "replayUploadRequired")]
    public bool ReplayUploadRequired { get; set; }
    [DataMember(Name = "mapPool")]
    public List<Map.Info> MapPool { get; set; }
  }
}
