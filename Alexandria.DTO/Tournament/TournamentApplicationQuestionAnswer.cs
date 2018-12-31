using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Tournament
{
  [JsonSchema("TournamentApplicationQuestionAnswer")]
  [DataContract]
  public class TournamentApplicationQuestionAnswer
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "answer")]
    public string Answer { get; set; }
  }
}
