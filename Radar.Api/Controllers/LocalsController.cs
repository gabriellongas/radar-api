using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Radar.Api.Data;
using Radar.Api.Models;

namespace Radar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalsController : ControllerBase
    {
        private readonly RadarContext _context;

        public LocalsController(RadarContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Local>>> GetLocal()
        {
            if (_context.Locals == null)
            {
                return NotFound();
            }

            return await _context.Locals.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Local>> GetLocal(int id)
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

            return local;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocal(int id, Local local)
        {
            if (id != local.LocalId)
            {
                return BadRequest();
            }

            _context.Entry(local).State = EntityState.Modified;

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
        public async Task<ActionResult<Local>> PostLocal(Local local)
        {
            if (_context.Locals == null)
            {
                return Problem("Entity set 'RadarContext.Local'  is null.");
            }

            _context.Locals.Add(local);
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
