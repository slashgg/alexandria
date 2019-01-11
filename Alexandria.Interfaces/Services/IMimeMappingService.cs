using System;
using System.Collections.Generic;
using System.Text;

namespace Alexandria.Interfaces.Services
{
  public interface IMimeMappingService
  {
    string GetMimeType(string fileName);
    string GetExtension(string mimeType);
  }
}
