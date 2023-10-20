using Microsoft.AspNetCore.Mvc;
using Radar.Api.Data;
using System.Text.Json;

namespace Radar.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IPessoaRepository _pessoaRepository;

        public LoginController(ILogger<LoginController> logger, IConfiguration configuration, IPessoaRepository pessoaRepository)
        {
            _logger = logger;
            _pessoaRepository = pessoaRepository
;        }

        [HttpGet(Name = "Login")]
        public async Task<IActionResult> Login()
        {
            try
            {
                var lista = _pessoaRepository.ListarPessoas();
                return Ok(JsonSerializer.Serialize(lista));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}