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
            try
            {
                if (_context.Pessoa == null)
                {
                    return NotFound();
                }

                List<Pessoa> pessoas = await _context.Pessoa.ToListAsync();
                return Ok(pessoas.ToReadDto());
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaReadDto>> GetPessoa(int id)
        {
            try
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
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPessoa(int id, PessoaUpdateDto pessoaDto)
        {
            try
            {
                if (id != pessoaDto.PessoaId)
                {
                    return BadRequest();
                }

                Pessoa? pessoa = await _context.Pessoa.FindAsync(id);

                if (pessoa == null)
                {
                    return NotFound();
                }

                pessoa.Nome = pessoaDto.Nome;
                pessoa.Email = pessoaDto.Email;
                pessoa.Login = pessoaDto.Login;
                pessoa.Descricao = pessoaDto.Descricao;

                _context.Entry(pessoa).State = EntityState.Modified;

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
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Pessoa>> PostPessoa(PessoaCreateDto pessoa)
        {
            try
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
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            try
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
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("SignIn")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(PessoaLoginDto login)
        {
            try
            {
                if (_context.Pessoa == null)
                {
                    return NotFound();
                }

                if (!await _context.Pessoa.AnyAsync(pessoa => pessoa.Login == login.Login || pessoa.Email == login.Email))
                {
                    return NotFound();
                }

                Pessoa pessoa = (await _context.Pessoa.FirstOrDefaultAsync(pessoa => pessoa.Login == login.Login || pessoa.Email == login.Email))!;
                if (!IsPasswordValid(login.Senha, pessoa.SenhaHash, pessoa.SenhaKey))
                {
                    return Unauthorized();
                }

                string token = CreateToken(pessoa);

                return Ok(token);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("ValidatePassword")]
        public async Task<IActionResult> ValidatePassword(PessoaLoginDto login)
        {
            try
            {
                if (_context.Pessoa == null)
                {
                    return NotFound();
                }

                if (!await _context.Pessoa.AnyAsync(pessoa => pessoa.Login == login.Login || pessoa.Email == login.Email))
                {
                    return BadRequest();
                }

                Pessoa pessoa = (await _context.Pessoa.FirstOrDefaultAsync(pessoa => pessoa.Login == login.Login || pessoa.Email == login.Email))!;

                bool isValid = IsPasswordValid(login.Senha, pessoa.SenhaHash, pessoa.SenhaKey);

                return Ok(isValid);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDto updatePasswordDto)
        {
            try
            {
                if (_context.Pessoa == null)
                {
                    return NotFound();
                }

                if (!await _context.Pessoa.AnyAsync(pessoa => pessoa.PessoaId == updatePasswordDto.PessoaId))
                {
                    return BadRequest();
                }

                Pessoa pessoa = (await _context.Pessoa.FirstOrDefaultAsync(pessoa => pessoa.PessoaId == updatePasswordDto.PessoaId))!;

                Dictionary<string, string> hmac = GetHmacFromPassword(updatePasswordDto.NewPassword);
                pessoa.SenhaHash = hmac["Hash"];
                pessoa.SenhaKey = hmac["Key"];

                _context.Entry(pessoa).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();

                    return Ok();
                }
                catch (DbUpdateException)
                {
                    if (!PessoaExists(updatePasswordDto.PessoaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        private bool PessoaExists(int id)
        {
            return (_context.Pessoa?.Any(e => e.PessoaId == id)).GetValueOrDefault();
        }

        private int GetNextId()
        {
            if (!_context.Pessoa.Any()) return 1;
            return _context.Pessoa.Max(e => e.PessoaId) + 1;
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
            byte[] keyBytes = Convert.FromBase64String(hmac512key);
            using HMACSHA512 hmac = new(keyBytes);

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] passwordHashBytes = hmac.ComputeHash(passwordBytes);
            string passwordHash = Convert.ToBase64String(passwordHashBytes);
            
            return hmac512hash == passwordHash;
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