using System;
using System.Collections.Generic;
using System.Text;

namespace Alexandria.Shared.Configuration
{
  public class PassportClientConfiguration
  {
    public string BaseUrl { get; set; }
    public string UpdateUserEndpoint { get; set; }
    public string ResendEmailVerificationEndpoint { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string Scope { get; set; }
  }
}
