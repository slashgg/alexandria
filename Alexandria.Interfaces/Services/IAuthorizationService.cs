using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alexandria.Interfaces.Services
{
  public interface IAuthorizationService
  {
    Task<bool> Can(Guid userId, string permission);
    Task AddPermission(Guid userId, string permission);
    Task RemovePermission(Guid userId, string permission);
    Task AddPermission(Guid userId, IList<string> permissions);
    Task RemovePermission(Guid userId, IList<string> permissions);
    Task<List<Guid>> GetAvailableResources<T>(Guid userId, string permission);
  }
}
