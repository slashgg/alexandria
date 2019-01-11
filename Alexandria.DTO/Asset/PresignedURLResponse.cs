using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Asset
{
  [DataContract]
  [JsonSchema("AssetPresignedURLResponse")]
  public class PresignedURLResponse
  {
    [DataMember(Name = "url")]
    public string Url { get; set; }
    [DataMember(Name = "correlationId")]
    public uint CorrelationId { get; set; }

    public PresignedURLResponse(string result, uint correlationId)
    {
      this.Url = result;
      this.CorrelationId = correlationId;
    }
  }
}
