using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Alexandria.Infrastructure.Authorization
{
  public static class AuthorizationPolicies
  {
    public static AuthorizationPolicy Default { get; } = ScopePolicy.Create("@slashgg/alexandria.full_access");
    public static AuthorizationPolicy Backchannel { get; } = ScopePolicy.Create("@slashgg/alexandria.backchannel");
  }
}
