using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.DTO.Team
{
  [DataContract]
  public class Create
  {
    [DataMember(Name = "name")]
    public string Name { get; set; }
    [DataMember(Name = "invites")]
    public List<string> Invites { get; set; }
    [DataMember(Name = "abbreviation")]
    public string Abbreviation { get; set; }
  }
}
