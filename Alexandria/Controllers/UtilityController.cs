using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Alexandria.Controllers
{
  [ApiController]
  public class UtilityController : ControllerBase
  {
    [SwaggerIgnore]
    [HttpGet("status")]
    public IActionResult HealthCheck()
    {
      return Ok();
    }

    [SwaggerIgnore]
    [HttpGet("test-error")]
    public IActionResult TestError()
    {
      throw new System.Exception("Test Error");
    }
  }
}
