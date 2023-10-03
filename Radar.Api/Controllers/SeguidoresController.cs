using Microsoft.AspNetCore.Mvc;

namespace Radar.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeguidoresController : ControllerBase
    {
        private readonly ILogger<SeguidoresController> _logger;

        public SeguidoresController(ILogger<SeguidoresController> logger)
        {
            _logger = logger;
        }

    }
}