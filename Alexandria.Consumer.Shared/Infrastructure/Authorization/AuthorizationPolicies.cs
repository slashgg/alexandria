using Microsoft.AspNetCore.Authorization;

namespace Alexandria.Consumer.Shared.Infrastructure.Authorization
{
  public static class AuthorizationPolicies
  {
    public static AuthorizationPolicy Default { get; } = ScopePolicy.Create("@slashgg/alexandria.full_access");
    public static AuthorizationPolicy Backchannel { get; } = ScopePolicy.Create("@slashgg/alexandria.backchannel");
    public static AuthorizationPolicy Admin { get; } = ScopePolicy.Create("@slashgg/alexandria.admin");
  }
}
