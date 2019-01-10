using System;
using System.Collections.Generic;
using System.Text;
using Svalbard;

namespace Alexandria.Shared.ErrorKey
{
  public static class Asset
  {
    public static readonly ServiceError InvalidBucket = new ServiceError("ASSET.INVALID_BUCKET", 400); 
  }
}
