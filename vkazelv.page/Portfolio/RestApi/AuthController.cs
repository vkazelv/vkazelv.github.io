using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.RestApi
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet("google")]
        [HttpPost("google")]
        public IActionResult Google()
        {
            return Ok();
        }
    }
}
