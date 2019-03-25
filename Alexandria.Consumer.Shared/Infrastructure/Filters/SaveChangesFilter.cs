using System.Threading.Tasks;
using Alexandria.EF.Context;
using Microsoft.AspNetCore.Mvc.Filters;
using Svalbard;

namespace Alexandria.Consumer.Shared.Infrastructure.Filters
{
  public class SaveChangesFilter : IAsyncActionFilter
  {
    private readonly AlexandriaContext context;
    public SaveChangesFilter(AlexandriaContext context)
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

      var res = result.Result;
      if (res is OperationResult os && os.IsError())
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
