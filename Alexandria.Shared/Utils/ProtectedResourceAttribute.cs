using System;
using System.Collections.Generic;
using System.Text;

namespace Alexandria.Shared.Utils
{
  public class ProtectedResourceAttribute : Attribute
  {
    public string Name { get; set; }

    public ProtectedResourceAttribute(string name)
    {
      this.Name = name;
    }
  }
}
