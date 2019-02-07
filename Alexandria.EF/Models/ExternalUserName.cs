using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Alexandria.Shared.Utils;

namespace Alexandria.EF.Models
{
  [ProtectedResource("external-user-name")]
  public class ExternalUserName : BaseEntity
  {
    public string UserName { get; set; }
    public string LogoURL { get; set; }
    public string ServiceName { get; set; }
    
    /* Foreign Keys */
    public Guid UserProfileId { get; set; }
    public Guid GameId { get; set; }

    /* Relationships */
    [ForeignKey("UserProfileId")]
    public virtual UserProfile UserProfile { get; set; }
    [ForeignKey("GameId")]
    public Game Game { get; set; }

    public ExternalUserName() { }
    public ExternalUserName(string userName, string logoURL, string serviceName, Guid gameId)
    {
      this.UserName = userName;
      this.LogoURL = logoURL;
      this.ServiceName = serviceName;
      this.GameId = gameId;
    }
  }
}
