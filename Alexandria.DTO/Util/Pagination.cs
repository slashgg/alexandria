using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.DTO.Util
{
  [DataContract]
  public class Pagination
  {
    [DataMember(Name = "page")]
    public int Page { get; set; } = 1;

    [DataMember(Name = "limit")]
    public int Limit { get; set; } = 25;

    [NotMapped]
    public int ZeroIndexPage {
      get
      {
        var newPage = this.Page - 1;
        if (newPage < 0)
        {
          newPage = 0;
        }

        return newPage;
      }
    }
  }
}
