using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexandria.Infrastructure.Filters
{
  public class PermissionsRequiredAttribute : Attribute, IAsyncAuthorizationFilter
  {

    public PermissionsRequiredAttribute(params string[] ARNs)
    {
      this.ARNs = ARNs;
    }

    private readonly IEnumerable<string> ARNs;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
      var userId = context.HttpContext.GetUserId();
      if (userId == null)
      {
        context.Result = new UnauthorizedResult();
        return;
      }

      var extractRegex = new Regex(@"(?<=\{)[^}]*(?=\})");

      var populatedARNs = this.ARNs.Select(a =>
      {
        var copyArn = a;
        var resources = extractRegex.Matches(a).Select(part => part.Value);
        foreach (var resource in resources)
        {
          var paramValue = (string)context.RouteData.Values.GetValueOrDefault(resource, null);
          if (paramValue == null)
          {
            throw new NoNullAllowedException("The value cannot be empty");
          }

          copyArn = a.Replace($"{{{resource}}}", paramValue);
        }

        return copyArn;
      });

      var authService = context.HttpContext.RequestServices.GetService(typeof(IAuthorizationService)) as IAuthorizationService;

      var canTask = populatedARNs.Select(async arn => await authService.Can(userId.Value, arn));
      var can = await Task.WhenAll(canTask);

      if (!can.All(hasPermisson => hasPermisson))
      {
        context.Result = new UnauthorizedResult();
      }
    }
  }
}
