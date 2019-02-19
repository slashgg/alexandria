using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;

namespace Alexandria.Utils
{
  public class QueryStringConstraintAttribute : ActionMethodSelectorAttribute
  {
    public string ValueName { get; }
    public bool ValuePresent { get; }
    public string ValueMatch { get; set; }

    public QueryStringConstraintAttribute(string valueName, bool valuePresent, string valueMatch = null)
    {
      this.ValueName = valueName;
      this.ValuePresent = valuePresent;
      this.ValueMatch = valueMatch;
    }

    public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
    {
      var value = routeContext.HttpContext.Request.Query[this.ValueName];
      if (this.ValuePresent)
      {
        if (this.ValueMatch != null)
        {
          return this.ValueMatch.Equals(value);
        }

        return !StringValues.IsNullOrEmpty(value);
      }

      return StringValues.IsNullOrEmpty(value);
    }
  }
}
