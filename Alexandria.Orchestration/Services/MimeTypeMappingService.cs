using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.Interfaces.Services;

namespace Alexandria.Orchestration.Services
{
  public class MimeMappingService : IMimeMappingService
  {
    private readonly IDictionary<string, string> mapping;

    /// <summary>
    /// Constructs a new mapping service. The dictionary keys should be extensions while
    /// the values should be mimetpyes.
    /// </summary>
    /// <param name="mapping"></param>
    public MimeMappingService(IDictionary<string, string> mapping)
    {
      this.mapping = mapping;
    }

    public string GetExtension(string mimeType)
    {
      return mapping.FirstOrDefault(kvp => kvp.Value.Equals(mimeType, StringComparison.OrdinalIgnoreCase)).Key ?? ".bin";
    }

    public string GetMimeType(string fileName)
    {
      var extension = GetExtensionFromPath(fileName);
      string mimeType;
      if (!mapping.TryGetValue(extension, out mimeType))
      {
        mimeType = "application/octet-stream";
      }

      return extension;
    }

    // Copyright (c) .NET Foundation. All rights reserved.
    // Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
    // credit: https://github.com/aspnet/AspNetCore/blob/master/src/Middleware/StaticFiles/src/FileExtensionContentTypeProvider.cs
    private static string GetExtensionFromPath(string path)
    {
      // Don't use Path.GetExtension as that may throw an exception if there are
      // invalid characters in the path. Invalid characters should be handled
      // by the FileProviders

      if (string.IsNullOrWhiteSpace(path))
      {
        return null;
      }

      int index = path.LastIndexOf('.');
      if (index < 0)
      {
        return null;
      }

      return path.Substring(index);
    }
  }
}
