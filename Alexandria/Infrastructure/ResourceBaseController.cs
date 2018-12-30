using System;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.Infrastructure
{
  [ApiController]
  public class ResourceBaseController : ControllerBase
  {
    public Guid resourceId;
    public string Slug;
  }
}
