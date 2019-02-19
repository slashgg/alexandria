using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Alexandria.Shared.Extensions
{
  public static class HttpContextExtension
  {
    public static Guid? GetUserId(this HttpContext context)
    {
      if (context.User.Identity == null)
      {
        throw new NoNullAllowedException("Identity is null");
      }
      var user = (ClaimsIdentity)context.User.Identity;
      if (user != null)
      {
        var claim = user.FindFirst("sub");
        if (claim != null)
        {
          return Guid.Parse(claim.Value);
        }
      }
      return null;
    }
  }
}
