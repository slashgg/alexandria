using System;
using System.Collections.Generic;
using System.Text;
using Svalbard;

namespace Alexandria.Shared.ErrorKey
{
  public static class Casting
  {
    public static ServiceError ClaimNotFound = new ServiceError("CASTING.CLAIM_NOT_FOUND", 404);
    public static ServiceError CastNotFound = new ServiceError("CASTING.NOT_FOUND", 404);
    public static ServiceError ClaimAlreadyExists = new ServiceError("CASTING.CLAIM_ALREADY_EXISTS", 409);
    public static ServiceError AlreadyClaimed = new ServiceError("CASTING.ALREADY_CLAIMED", 409);
  }
}
