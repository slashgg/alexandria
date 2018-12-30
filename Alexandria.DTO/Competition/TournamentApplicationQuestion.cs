using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Alexandria.Shared.Enums;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Competition
{
  [DataContract]
  [JsonSchema("CompetitionTournamentApplicationQuestion")]
  public class TournamentApplicationQuestion
  {
    [DataMember(Name = "question")]
    public string Question { get; set; }
    [DataMember(Name = "fieldType")]
    public FieldType FieldType { get; set; }
    [DataMember(Name = "selectOptions")]
    public List<string> SelectOptions { get; set; }
    [DataMember(Name = "optional")]
    public bool Optional { get; set; }
    [DataMember(Name = "defaultValue")]
    public string DefaultValue { get; set; }
  }
}
