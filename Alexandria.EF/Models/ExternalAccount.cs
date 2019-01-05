using System;
using System.ComponentModel.DataAnnotations.Schema;
using Alexandria.Shared.Enums;
using Alexandria.Shared.Utils;

namespace Alexandria.EF.Models
{
  [ProtectedResource("external-account")]
  public class ExternalAccount : BaseEntity
  {
    public ExternalProvider Provider { get; set; }
    public string Name { get; set; }
    public bool Verified { get; set; } = false;
    public string Token { get; set; }
    public string ExternalId { get; set; }

    private ExternalAccount()
    {
    }

    public ExternalAccount(ExternalProvider provider, string name, string externalId, Guid profileId)
    {
      Provider = provider;
      Name = name;
      ExternalId = externalId;
      UserProfileId = profileId;
    }

    /* Foreign Keys */
    public Guid UserProfileId { get; set; }

    /* Relations */
    [ForeignKey("UserProfileId")]
    public virtual UserProfile UserProfile { get; set; }
  }
}
