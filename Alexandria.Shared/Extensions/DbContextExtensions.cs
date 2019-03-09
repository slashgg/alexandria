using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Shared.Extensions
{
  public static class DbContextExtensions
  {
    public static DbCommand LoadStoredProc(this DbContext context, string storedProcName)
    {
      var cmd = context.Database.GetDbConnection().CreateCommand();
      cmd.CommandText = storedProcName;
      cmd.CommandType = System.Data.CommandType.StoredProcedure;
      return cmd;
    }
  }
}
