using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alexandria.Interfaces.Services
{
  public interface IInputValidationService
  {
    Task<IList<Svalbard.FieldError>> Validate(object data);
    Task<IList<Svalbard.FieldError>> Validate(object data, Svalbard.Services.ServiceResult serviceResult);
  }
}
