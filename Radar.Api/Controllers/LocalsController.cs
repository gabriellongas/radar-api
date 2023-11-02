using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Radar.Api.Data;
using Radar.Api.Models;
using Radar.Api.Models.Dto;

namespace Radar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalsController : ControllerBase
    {
        private readonly RadarContext _context;
        private readonly IConfiguration _configuration;

        public LocalsController(RadarContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpGet]
        [Route("GetConnectionString")]
        public String GetConnectionString()
        {
            return _configuration.GetValue<string>("ConnectionStrings:SqlConnection");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocalReadDto>>> GetLocal()
        {
            if (_context.Locals == null)
            {
                return NotFound();
            }

            List<Local> locals = await _context.Locals.ToListAsync();
            return Ok(locals.ToReadDto());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LocalReadDto>> GetLocal(int id)
        {
            if (_context.Locals == null)
            {
                return NotFound();
            }

            var local = await _context.Locals.FindAsync(id);

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
            catch (DbUpdateConcurrencyException)
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
            if (_context.Locals == null)
            {
                return Problem("Entity set 'RadarContext.Local'  is null.");
            }

            _context.Locals.Add(local.ToModel());
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocal", new { id = local.LocalId }, local);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocal(int id)
        {
            if (_context.Locals == null)
            {
                return NotFound();
            }
            var local = await _context.Locals.FindAsync(id);
            if (local == null)
            {
                return NotFound();
            }

            _context.Locals.Remove(local);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LocalExists(int id)
        {
            return (_context.Locals?.Any(e => e.LocalId == id)).GetValueOrDefault();
        }
    }
}
