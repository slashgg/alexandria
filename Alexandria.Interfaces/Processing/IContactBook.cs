using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alexandria.Interfaces.Processing
{
  public interface IContactBook
  {
    Task CreateContacts(IList<DTO.Marketing.Contact> contacts);
    Task UpdateContacts(IList<DTO.Marketing.Contact> contacts);
    Task DeleteContacts(IList<DTO.Marketing.Contact> contacts);
  }
}
