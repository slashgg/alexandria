using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexandria.Infrastructure.Filters
{
  public class ResourceSelectFilterAttribute : Attribute, IActionFilter
  {
    private readonly string resourceParam;
    public ResourceSelectFilterAttribute(string resourceParam)
    {
      this.resourceParam = resourceParam;
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
      if (context.Controller is ResourceBaseController && ((ResourceBaseController)context.Controller).ControllerContext.RouteData.Values.TryGetValue(this.resourceParam, out object value))
      {
        var controller = (ResourceBaseController)context.Controller;
        try {
          var guid = Guid.Parse((string)value);
          controller.resourceId = guid;
        } catch
        {
          var name = (string)value;
          controller.Slug = name;
        }
      }
    }
  }
}
