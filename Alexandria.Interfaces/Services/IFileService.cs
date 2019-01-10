using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexandria.DTO.Asset;
using Svalbard.Services;

namespace Alexandria.Interfaces.Services
{
  public interface IFileService
  {
    Task<ServiceResult<PresignedURLResponse>> CreatePresignedUrl(string bucketName, string filePath, string contentType);
    ServiceResult<PresignedURLResponse> GetFromCorrelationId(uint correlationId);
    Task<ServiceResult> DeleteByUrl(string url);
  }
}
