using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alexandria.DTO.Marketing
{
  [DataContract]
  public class Contact
  {
    [DataMember(Name ="email")]
    public string Email { get; set; }
    [DataMember(Name = "user_name")]
    public string UserName { get; set; }
    [JsonIgnore]
    public List<string> Competitions = new List<string>();
    [DataMember(Name = "competitions")]
    public string FormattedCompetition { get
      {
        return string.Join(';', this.Competitions); 
      }
    }
    [DataMember(Name = "team_leader")]
    public int TeamLeader { get; set; }
  }
}
