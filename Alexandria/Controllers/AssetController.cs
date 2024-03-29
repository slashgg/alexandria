﻿using System;
using System.Threading.Tasks;
using Alexandria.DTO.Asset;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
using Alexandria.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers
{
  [Route("assets")]
  [ApiController]
  public class AssetController : ControllerBase
  {
    public static readonly string ASSET_BUCKET = "slashgg.assets";
    private readonly IFileService files;
    private readonly IMimeMappingService mimeMapping;

    public AssetController(IFileService files, IMimeMappingService mimeMapping)
    {
      this.files = files;
      this.mimeMapping = mimeMapping;
    }

    /// <summary>
    /// Gets an Amazon S3 pre-signed URL that lasts 5 minutes.
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(PresignedURLResponse), 200)]
    [ProducesResponseType(typeof(void), 400)]
    [ProducesResponseType(typeof(Svalbard.Error), 400)]
    [ProducesResponseType(typeof(void), 401)]
    public async Task<OperationResult<PresignedURLResponse>> GetPresignedUrl([FromQuery] string type)
    {
      if (string.IsNullOrEmpty(type))
      {
        return new OperationResult<PresignedURLResponse>(400);
      }

      // We expect type to be a mime type
      string extension = string.Empty;
      try
      {
        extension = mimeMapping.GetExtension(type);
      }
      catch
      {
        return new OperationResult<PresignedURLResponse>(400);
      }

      var userId = HttpContext.GetUserId();
      var result = await files.CreatePresignedUrl(ASSET_BUCKET, $"{userId}/{Guid.NewGuid().ToString("N")}{extension}", type);
      if (result.Success)
      {
        return new OperationResult<PresignedURLResponse>(result.Data);
      }

      return new OperationResult<PresignedURLResponse>(result.Error);
    }
  }
}
