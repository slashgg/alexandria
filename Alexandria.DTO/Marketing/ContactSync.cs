using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.DTO.Marketing
{
  [DataContract]
  public class ContactSync
  {
    [DataMember(Name = "new")]
    public bool New { get; set; }
    [DataMember(Name = "userId")]
    public Guid UserId { get; set; }
    [DataMember(Name = "delete")]
    public bool Delete { get; set; } = false;

    public ContactSync() { }

    public ContactSync(Guid userId, bool newUser = false, bool delete = false)
    {
      this.New = newUser;
      this.UserId = userId;
      this.Delete = delete;
    }
  }
}
