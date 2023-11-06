using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Radar.Api.Data;
using Radar.Api.Models.Dto;

namespace Radar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PessoaController : ControllerBase
    {
        private readonly RadarContext _context;

        public PessoaController(RadarContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PessoaReadDto>>> GetPessoa()
        {
            if (_context.Pessoa == null)
            {
                return NotFound();
            }

            List<Pessoa> pessoas = await _context.Pessoa.ToListAsync();
            return Ok(pessoas.ToReadDto());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaReadDto>> GetPessoa(int id)
        {
            if (_context.Pessoa == null)
            {
                return NotFound();
            }

            var pessoa = await _context.Pessoa.FindAsync(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            return Ok(pessoa.ToReadDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPessoa(int id, PessoaUpdateDto pessoa)
        {
            if (id != pessoa.PessoaId)
            {
                return BadRequest();
            }

            Dictionary<string, string> hmac = GetHmacFromPassword(pessoa.Senha);

            _context.Entry(pessoa.ToModel(hmac["Hash"], hmac["Key"])).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!PessoaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Pessoa>> PostPessoa(PessoaCreateDto pessoa)
        {
            if (_context.Pessoa == null)
            {
                return Problem("Entity set 'RadarContext.Pessoa'  is null.");
            }

            int newId = GetNextId();

            Dictionary<string, string> hmac = GetHmacFromPassword(pessoa.Senha);
            _context.Pessoa.Add(pessoa.ToModel(newId, hmac["Hash"], hmac["Key"]));

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PessoaExists(newId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPessoa", new { id = newId }, pessoa);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            if (_context.Pessoa == null)
            {
                return NotFound();
            }

            Pessoa? pessoa = await _context.Pessoa.FindAsync(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            _context.Pessoa.Remove(pessoa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("SignIn")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(PessoaLoginDto login)
        {
            if (_context.Pessoa == null)
            {
                return NotFound();
            }

            if (!await _context.Pessoa.AnyAsync(pessoa => pessoa.Login == login.Login || pessoa.Email == login.Email))
            {
                return base.NotFound();
            }

            Pessoa pessoa = (await _context.Pessoa.FirstOrDefaultAsync(pessoa => pessoa.Login == login.Login || pessoa.Email == login.Email))!;
            if (!IsPasswordValid(login.Senha, pessoa.SenhaHash, pessoa.SenhaKey))
            {
                return Unauthorized();
            }

            string token = CreateToken(pessoa);

            return Ok(token);
        }

        private bool PessoaExists(int id)
        {
            return (_context.Pessoa?.Any(e => e.PessoaId == id)).GetValueOrDefault();
        }

        private int GetNextId()
        {
            return (_context.Pessoa?.Max(e => e.PessoaId) ?? 0) + 1;
        }

        private static Dictionary<string, string> GetHmacFromPassword(string password)
        {
            using HMACSHA512 hmac = new();

            return new Dictionary<string, string> {
                { "Hash", Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password))) },
                { "Key", Convert.ToBase64String(hmac.Key) }
            };
        }

        private static bool IsPasswordValid(string password, string hmac512hash, string hmac512key)
        {
            using HMACSHA512 hmac = new(Convert.FromBase64String(hmac512key));

            return hmac512hash == Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        private string CreateToken(Pessoa login)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login.Nome),
                new Claim(ClaimTypes.Email, login.Email),
                new Claim(ClaimTypes.NameIdentifier, login.PessoaId.ToString())
            };

            byte[] rawKey = Encoding.UTF8.GetBytes(_context.Configuration["Jwt:Key"]!);
            byte[] key = new byte[16];
            Array.Copy(rawKey, key, Math.Min(rawKey.Length, key.Length));

            SymmetricSecurityKey symetricKey = new(key);
            SigningCredentials credentials = new(symetricKey, SecurityAlgorithms.HmacSha512Signature);
            JwtSecurityToken jwt = new(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return token;
        }
    }
}