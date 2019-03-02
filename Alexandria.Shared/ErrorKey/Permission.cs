using System;
using System.Collections.Generic;
using System.Text;
using Svalbard;

namespace Alexandria.Shared.ErrorKey
{
  public static class Permission
  {
    public static ServiceError InsufficientPermission = new ServiceError("PERMISSION.INSUFFICIENT", 403);
  }
}
