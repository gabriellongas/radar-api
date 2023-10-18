using Microsoft.AspNetCore.Mvc;

namespace Radar.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger, IConfiguration configuration)
        {
            _logger = logger;

        }

        [HttpGet(Name = "Login")]
        public async Task<IActionResult> Login(string user, string senha)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}