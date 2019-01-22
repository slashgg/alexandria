using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alexandria.Shared.Utils
{
  public class AsyncLazy<T> : Lazy<Task<T>>
  {
    public AsyncLazy(Func<Task<T>> taskFactory) : base(
      () => Task.Factory.StartNew(taskFactory).Unwrap())
    { }
  }
}
