using Microsoft.AspNetCore.Mvc;

namespace Radar.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocalController : ControllerBase
    {
        private readonly ILogger<LocalController> _logger;

        public LocalController(ILogger<LocalController> logger)
        {
            _logger = logger;
        }

    }
}