using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.DTO.Asset
{
  [DataContract]
  public class PresignedURLResponse
  {
    [DataMember(Name = "url")]
    public string Url { get; set; }

    public PresignedURLResponse(string result)
    {
      this.Url = result;
    }
  }
}
