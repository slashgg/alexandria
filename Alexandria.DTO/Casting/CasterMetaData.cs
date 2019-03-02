using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Casting
{
  [DataContract]
  [JsonSchema("CastingUserMetaData")]
  public class CasterMetaData
  {
    [DataMember(Name = "twitchChannel")]
    public string TwitchChannel { get; set; }
  }
}
