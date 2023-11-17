using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Radar.Api.Data;
using Radar.Api.Models.Dto;

namespace Radar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CurtidaController : ControllerBase
    {
        private readonly RadarContext _context;
        private readonly IConfiguration _configuration;

        public CurtidaController(RadarContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Curtida>>> GetCurtida()
        {
            if (_context.Curtida == null)
            {
                return NotFound();
            }

            IEnumerable<Curtida> curtidas = await _context.Curtida.ToListAsync();
            return Ok(curtidas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Curtida>> GetCurtida(int id)
        {
            if (_context.Curtida == null)
            {
                return NotFound();
            }
            Curtida? curtida = await _context.Curtida.FindAsync(id);

            if (curtida == null)
            {
                return NotFound();
            }

            return Ok(curtida);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurtida(int id, Curtida curtida)
        {
            if (id != curtida.CurtidaId)
            {
                return BadRequest();
            }

            _context.Entry(curtida).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!CurtidaExists(id))
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
        public async Task<ActionResult<CurtidaCreateDto>> PostCurtida(CurtidaCreateDto curtida)
        {
            if (_context.Curtida == null)
            {
                return Problem("Entity set 'RadarContext.Curtida'  is null.");
            }

            int newId = GetNextId();
            _context.Curtida.Add(curtida.ToModel(newId));

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CurtidaExists(newId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCurtida", new { id = newId }, curtida);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurtida(int id)
        {
            if (_context.Curtida == null)
            {
                return NotFound();
            }
            Curtida? curtida = await _context.Curtida.FindAsync(id);

            if (curtida == null)
            {
                return NotFound();
            }

            _context.Curtida.Remove(curtida);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{pessoaId}/{postId}")]
        public async Task<IActionResult> DeleteCurtida(int pessoaId, int postId)
        {
            if (_context.Curtida == null)
            {
                return NotFound();
            }
            Curtida? curtida = await _context.Curtida.SingleAsync(curtida => curtida.PessoaIdCurtindo == pessoaId && curtida.PostIdCurtido == postId);

            if (curtida == null)
            {
                return NotFound();
            }

            _context.Curtida.Remove(curtida);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CurtidaExists(int id)
        {
            return (_context.Curtida?.Any(e => e.CurtidaId == id)).GetValueOrDefault();
        }

        private int GetNextId()
        {
            if (!_context.Curtida.Any()) return 1;
            return _context.Curtida.Max(e => e.CurtidaId) + 1;
        }
    }
}
