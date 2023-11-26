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
            try
            {
                if (_context.Local == null)
                {
                    return NotFound();
                }

                List<Local> locals = await _context.Local.ToListAsync();
                return Ok(locals.ToReadDto());
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LocalReadDto>> GetLocal(int id)
        {
            try
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
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocal(int id, LocalUpdateDto local)
        {
            try
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
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Local>> PostLocal(LocalCreateDto local)
        {
            try
            {
                if (_context.Local == null)
                {
                    return Problem("Entity set 'RadarContext.Local'  is null.");
                }

                int newId = GetNextId();

                _context.Local.Add(local.ToModel(newId));
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetLocal", new { id = newId }, local);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocal(int id)
        {
            try
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
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        private bool LocalExists(int id)
        {
            return (_context.Local?.Any(e => e.LocalId == id)).GetValueOrDefault();
        }

        private int GetNextId()
        {
            if (!_context.Local.Any()) return 1;
            return _context.Local.Max(e => e.LocalId) + 1;
        }
    }
}
