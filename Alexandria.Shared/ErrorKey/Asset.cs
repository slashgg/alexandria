using System;
using System.Collections.Generic;
using System.Text;
using Svalbard;

namespace Alexandria.Shared.ErrorKey
{
  public static class Asset
  {
    public static readonly ServiceError InvalidBucket = new ServiceError("ASSET.INVALID_BUCKET", 400);
    public static readonly ServiceError PresignedUrlInvalid = new ServiceError("ASSET.PRESIGNED_INVALID", 400);
    public static readonly ServiceError CorrelationIdInvalid = new ServiceError("ASSET.CORRELATION_ID_INVALID", 400);
    public static readonly ServiceError PresignedRequestFailure = new ServiceError("ASSET.PRESIGNED_FAILURE", 500);
    public static readonly ServiceError InvalidUrl = new ServiceError("ASSET.INVALID_URL", 400);
    public static readonly ServiceError DeleteObjectFailure = new ServiceError("ASSET.DELETE_OBJECT_FAILURE", 500);
  }
}
