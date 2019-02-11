using System;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.Shared.Infrastructure
{
  [ApiController]
  public class ResourceBaseController : ControllerBase
  {
    public Guid resourceId;
    public string Slug;
  }
}
