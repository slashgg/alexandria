using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexandria.Games.HeroesOfTheStorm.EF.Context;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexandria.Games.HeroesOfTheStorm.Infrastructure
{
  public class HeroesOfTheStormSaveChangesFilter : Attribute, IAsyncActionFilter
  {
    private readonly HeroesOfTheStormContext context;
    public HeroesOfTheStormSaveChangesFilter(HeroesOfTheStormContext context)
    {
      this.context = context;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      var result = await next();
      if (result.Exception != null)
      {
        return;
      }

      if (this.context.ChangeTracker.HasChanges())
      {
        await this.context.SaveChangesAsync();
      }
    }
  }
}
