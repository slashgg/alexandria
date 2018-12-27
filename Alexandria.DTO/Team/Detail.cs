using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Alexandria.Shared.Enums;

namespace Alexandria.DTO.Team
{
  [DataContract]
  public class Detail
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "name")]
    public string Name { get; set; }
    [DataMember(Name = "abbreviation")]
    public string Abbreviation { get; set; }
    [DataMember(Name = "members")]
    public List<Membership> Members { get; set; }
    [DataMember(Name = "state")]
    public TeamState TeamState { get; set; }
    [DataMember(Name = "logoURL")]
    public string LogoURL { get; set; }
  }
}
