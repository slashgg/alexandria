using Alexandria.EF.Context;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexandria.Infrastructure.Filters
{
  public class SaveChangesFilter : IActionFilter
  {
    private readonly AlexandriaContext context;
    public SaveChangesFilter(AlexandriaContext context)
    {
      this.context = context;
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
      if (this.context.ChangeTracker.HasChanges())
      {
        this.context.SaveChanges();
      }
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
      
    }
  }
}
