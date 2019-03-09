using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Alexandria.Shared.Extensions
{
  public static class DbDataReaderExtensions
  {
    public static IList<T> MapToList<T>(this DbDataReader dr)
    {
      var objList = new List<T>();
      var props = typeof(T).GetRuntimeProperties();

      var colMapping = dr.GetColumnSchema()
        .Where(x => props.Any(y => y.Name.ToLower() == x.ColumnName.ToLower()))
        .ToDictionary(key => key.ColumnName.ToLower());

      if (dr.HasRows)
      {
        while (dr.Read())
        {
          T obj = Activator.CreateInstance<T>();
          foreach (var prop in props)
          {
            var val = dr.GetValue(colMapping[prop.Name.ToLower()].ColumnOrdinal.Value);
            prop.SetValue(obj, val == DBNull.Value ? null : val);
          }
          objList.Add(obj);
        }
      }
      return objList;
    }
  }
}
