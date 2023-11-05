using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Radar.Api.Data;
using Radar.Api.Models;
using Radar.Api.Models.Dto;

namespace Radar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalController : ControllerBase
    {
        private readonly RadarContext _context;
        private readonly IConfiguration _configuration;

        public LocalController(RadarContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocalReadDto>>> GetLocal()
        {
            if (_context.Local == null)
            {
                return NotFound();
            }

            List<Local> locals = await _context.Local.ToListAsync();
            return Ok(locals.ToReadDto());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LocalReadDto>> GetLocal(int id)
        {
            if (_context.Local == null)
            {
                return NotFound();
            }

            var local = await _context.Local.FindAsync(id);

            if (local == null)
            {
                return NotFound();
            }

            return Ok(local.ToReadDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocal(int id, LocalCreateDto local)
        {
            if (id != local.LocalId)
            {
                return BadRequest();
            }

            _context.Entry(local.ToModel()).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!LocalExists(id))
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
        public async Task<ActionResult<Local>> PostLocal(LocalCreateDto local)
        {
            if (_context.Local == null)
            {
                return Problem("Entity set 'RadarContext.Local'  is null.");
            }

            local.LocalId = GetNextId();

            _context.Local.Add(local.ToModel());
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocal", new { id = local.LocalId }, local);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocal(int id)
        {
            if (_context.Local == null)
            {
                return NotFound();
            }
            var local = await _context.Local.FindAsync(id);
            if (local == null)
            {
                return NotFound();
            }

            _context.Local.Remove(local);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LocalExists(int id)
        {
            return (_context.Local?.Any(e => e.LocalId == id)).GetValueOrDefault();
        }

        private int GetNextId()
        {
            return (_context.Local?.Max(e => e.LocalId) ?? 0) + 1;
        }
    }
}
