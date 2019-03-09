using System;


namespace Alexandria.Shared.Validation
{
  public class EFUniqueAttribute : Attribute
  {
    public string Column { get; set; }
    public string Table { get; set; }

    public EFUniqueAttribute(string table, string column)
    {
      this.Table = table;
      this.Column = column;
    }
  }
}
