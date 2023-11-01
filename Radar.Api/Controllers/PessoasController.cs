using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Radar.Api.Data;
using Radar.Api.Models.Dto;

namespace Radar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoasController : ControllerBase
    {
        private readonly RadarContext _context;

        public PessoasController(RadarContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PessoaReadDto>>> GetPessoa()
        {
            if (_context.Pessoas == null)
            {
                return NotFound();
            }

            List<Pessoa> pessoas = await _context.Pessoas.ToListAsync();
            return Ok(pessoas.ToReadDto());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaReadDto>> GetPessoa(int id)
        {
            if (_context.Pessoas == null)
            {
                return NotFound();
            }

            var pessoa = await _context.Pessoas.FindAsync(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            return Ok(pessoa.ToReadDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPessoa(int id, PessoaCreateDto pessoa)
        {
            if (id != pessoa.PessoaId)
            {
                return BadRequest();
            }

            _context.Entry(pessoa.ToModel()).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
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
        public async Task<ActionResult<Pessoa>> PostPessoa(PessoaCreateDto pessoa)
        {
            if (_context.Pessoas == null)
            {
                return Problem("Entity set 'RadarContext.Pessoa'  is null.");
            }

            _context.Pessoas.Add(pessoa.ToModel());
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPessoa", new { id = pessoa.PessoaId }, pessoa);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            if (_context.Pessoas == null)
            {
                return NotFound();
            }

            var pessoa = await _context.Pessoas.FindAsync(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PessoaExists(int id)
        {
            return (_context.Pessoas?.Any(e => e.PessoaId == id)).GetValueOrDefault();
        }
    }
}
