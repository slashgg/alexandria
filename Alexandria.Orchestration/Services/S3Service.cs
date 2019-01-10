using System;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.DTO.Asset;
using Alexandria.Interfaces.Services;
using Amazon.S3;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Svalbard.Services;

namespace Alexandria.Orchestration.Services
{
  public class S3Service : IFileService
  {
    private readonly IAmazonS3 client;
    private readonly IMemoryCache cache;
    private readonly ILogger<S3Service> logger;

    public S3Service(IAmazonS3 client, IMemoryCache cache, ILogger<S3Service> logger)
    {
      this.client = client;
      this.cache = cache;
      this.logger = logger;
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

      try
      {
        var result = this.client.GetPreSignedURL(request);
        var correlationId = Svalbard.Utils.MurmurHash2.ComputeHash(result);

        response.Succeed(new PresignedURLResponse(result, correlationId));
        cache.Set(correlationId, result, request.Expires);
      }
      catch (Exception e)
      {
        logger.LogError(e, "Failed to create a presigned url.");
        response.Error = Shared.ErrorKey.Asset.PresignedRequestFailure;

        return response;
      }

      return response;
    }

    public async Task<ServiceResult> DeleteByUrl(string url)
    {
      var result = new ServiceResult();

      Uri uri;
      if (!Uri.TryCreate(url, UriKind.Absolute, out uri) || !uri.Host.StartsWith("s3.amazonaws.com"))
      {
        result.Error = Shared.ErrorKey.Asset.InvalidUrl;
        return result;
      }

      try
      {
        var bucketName = uri.LocalPath.Substring(1, uri.LocalPath.IndexOf('/', 1) - 1);
        var objectKey = uri.LocalPath.Substring(uri.LocalPath.IndexOf('/', 1) + 1);

        await client.DeleteObjectAsync(bucketName, objectKey);
      }
      catch (ArgumentOutOfRangeException)
      {
        result.Error = Shared.ErrorKey.Asset.InvalidUrl;
        return result;
      }
      catch (Exception)
      {
        result.Error = Shared.ErrorKey.Asset.DeleteObjectFailure;
        return result;
      }

      result.Succeed();
      return result;
    }

    public ServiceResult<PresignedURLResponse> GetFromCorrelationId(uint correlationId)
    {
      var result = new ServiceResult<PresignedURLResponse>();
      string url = null;
      if (!cache.TryGetValue(correlationId, out url))
      {
        result.Error = Shared.ErrorKey.Asset.CorrelationIdInvalid;
        return result;
      }

      result.Succeed(new PresignedURLResponse(url, correlationId));
      return result;
    }

    private async Task<bool> ConfirmBucketExists(string bucketName)
    {
      var bucketList = await this.client.ListBucketsAsync();
      var exists = bucketList.Buckets.Any(b => b.BucketName == bucketName);
      return exists;
    }
  }
}
