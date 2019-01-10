using System;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.DTO.Asset;
using Alexandria.Interfaces.Services;
using Amazon.S3;
using Svalbard.Services;

namespace Alexandria.Orchestration.Services
{
  public class S3Service : IFileService
  {
    private readonly IAmazonS3 client;
    public S3Service(IAmazonS3 client)
    {
      this.client = client;
    }

    public async Task<ServiceResult<PresignedURLResponse>> CreatePresignedUrl(string bucketName, string filePath, string contentType)
    {
      var response = new ServiceResult<PresignedURLResponse>();
      if (!(await ConfirmBucketExists(bucketName)))
      {
        response.Error = Shared.ErrorKey.Asset.InvalidBucket;
        return response;
      }

      var request = new Amazon.S3.Model.GetPreSignedUrlRequest
      {
        BucketName = bucketName,
        Verb = HttpVerb.PUT,
        Expires = DateTime.UtcNow.AddMinutes(5),
        Key = filePath,
        ContentType = contentType,
      };

      var result = this.client.GetPreSignedURL(request);
      response.Success = true;
      response.Data = new PresignedURLResponse(result);

      return response;
    }

    private async Task<bool> ConfirmBucketExists(string bucketName)
    {
      var bucketList = await this.client.ListBucketsAsync();
      var exists = bucketList.Buckets.Any(b => b.BucketName == bucketName);
      return exists;
    }
  }
}
