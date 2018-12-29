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
  }
}
