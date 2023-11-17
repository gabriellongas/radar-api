using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Radar.Api.Data;

namespace Radar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SeguidorController : ControllerBase
    {
        private readonly RadarContext _context;

        public SeguidorController(RadarContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seguidor>>> GetSeguidor()
        {
            if (_context.Seguidor == null)
            {
                return NotFound();
            }

            List<Seguidor> seguidor = await _context.Seguidor.ToListAsync();
            return Ok(seguidor);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Seguidor>> GetSeguidor(int id)
        {
            if (_context.Seguidor == null)
            {
                return NotFound();
            }
            Seguidor? seguidor = await _context.Seguidor.FindAsync(id);

            if (seguidor == null)
            {
                return NotFound();
            }

            return Ok(seguidor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeguidor(int id, Seguidor seguidor)
        {
            if (id != seguidor.SeguidorId)
            {
                return BadRequest();
            }

            _context.Entry(seguidor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!SeguidorExists(id))
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
        public async Task<ActionResult<Seguidor>> PostSeguidor(Seguidor seguidor)
        {
            if (_context.Seguidor == null)
            {
                return Problem("Entity set 'RadarContext.Seguidores'  is null.");
            }

            seguidor.SeguidorId = GetNextId();
            _context.Seguidor.Add(seguidor);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SeguidorExists(seguidor.SeguidorId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSeguidor", new { id = seguidor.SeguidorId }, seguidor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeguidor(int id)
        {
            if (_context.Seguidor == null)
            {
                return NotFound();
            }
            Seguidor? seguidor = await _context.Seguidor.FindAsync(id);
            if (seguidor == null)
            {
                return NotFound();
            }

            _context.Seguidor.Remove(seguidor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SeguidorExists(int id)
        {
            return (_context.Seguidor?.Any(e => e.SeguidorId == id)).GetValueOrDefault();
        }

        private int GetNextId()
        {
            if (!_context.Seguidor.Any()) return 1;
            return _context.Seguidor.Max(e => e.SeguidorId) + 1;
        }
    }
}
