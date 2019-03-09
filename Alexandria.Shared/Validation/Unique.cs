using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace Alexandria.Shared.Validation
{
  public class EFUniqueAttribute : Attribute
  {
    public Type EFType { get; set; }
    public string Column { get; set; }
    public string Table { get; set; }
    public Type ColumnType { get; set; }

    public EFUniqueAttribute(Type type, string column)
    {
      this.EFType = type;
      this.Column = column;
    }

    public EFUniqueAttribute(string table, string column, Type columnType)
    {
      this.Table = table;
      this.Column = column;
      this.ColumnType = columnType;
    }
  }
}
