using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.Admin.Controllers
{
  [Authorize]
  [ApiController]
  public class AdminController : ControllerBase
  {

  }
}
