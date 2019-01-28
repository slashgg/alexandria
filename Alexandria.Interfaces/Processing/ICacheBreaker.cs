using System;
using System.Collections.Generic;
using System.Text;

namespace Alexandria.Interfaces.Processing
{
  public interface ICacheBreaker
  {
    void Break(string key);
  }
}
