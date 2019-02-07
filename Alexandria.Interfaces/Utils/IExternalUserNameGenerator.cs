using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alexandria.Interfaces.Utils
{
  public interface IExternalUserNameGenerator
  {
    Task<DTO.UserProfile.ExternalUserName> Create(Guid userId);
  }
}
