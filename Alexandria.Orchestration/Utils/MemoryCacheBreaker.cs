using System;
using System.Collections.Generic;
using System.Text;
using Alexandria.Interfaces.Processing;
using Microsoft.Extensions.Caching.Memory;

namespace Alexandria.Orchestration.Utils
{
  public class MemoryCacheBreaker : ICacheBreaker
  {
    private IMemoryCache cache;

    public MemoryCacheBreaker(IMemoryCache memoryCache)
    {
      this.cache = memoryCache;
    }
    public void Break(string key)
    {
      try
      {
        this.cache.Remove(key);
      }
      catch { }
    }
  }
}
