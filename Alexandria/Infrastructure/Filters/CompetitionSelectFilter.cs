using System;
using System.Threading.Tasks;
using Alexandria.Controllers.Competition;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexandria.Infrastructure.Filters
{
  public class CompetitionSelectFilterAttribute : Attribute, IAsyncActionFilter
  {
    public async Task OnActionExecuted(ActionExecutedContext context)
    {
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      if (context.Controller is CompetitionBaseController && ((CompetitionBaseController)context.Controller).ControllerContext.RouteData.Values.TryGetValue("competitionId", out object value))
      {
        var controller = (CompetitionBaseController)context.Controller;
        controller.CompetitionId = Guid.Parse((string)value);
      }

      await next();
    }
  }
}
