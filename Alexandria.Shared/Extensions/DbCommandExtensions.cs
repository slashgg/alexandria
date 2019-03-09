using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Alexandria.Shared.Extensions
{
  public static class DbCommandExtensions
  {
    public static DbCommand WithSqlParam(this DbCommand cmd, string paramName, object paramValue)
    {
      if (string.IsNullOrEmpty(cmd.CommandText))
        throw new InvalidOperationException("Call LoadStoredProc before using this method");

      var param = cmd.CreateParameter();
      param.ParameterName = paramName;
      param.Value = paramValue;
      cmd.Parameters.Add(param);
      return cmd;
    }

    public static async  Task<IList<T>> ExecuteStoredProc<T>(this DbCommand command)
    {
      using (command)
      {
        if (command.Connection.State == System.Data.ConnectionState.Closed)
          command.Connection.Open();
        try
        {
          using (var reader = await command.ExecuteReaderAsync())
          {
            return reader.MapToList<T>();
          }
        }
        finally
        {
          command.Connection.Close();
        }
      }
    }
  }
}
