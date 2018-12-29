using System;
using System.Collections.Generic;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.UserProfile
{
  /// <summary>
  /// Base User Data
  /// </summary>
  [JsonSchema("UserProfileCreate")]
  public class Create
  {
    /// <summary>
    /// Corresponding ID from Passport
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// UserName including Hash
    /// </summary>
    public string DisplayName { get; set; }
    /// <summary>
    /// Users Email
    /// </summary>
    public string Email { get; set; }
  }
}
