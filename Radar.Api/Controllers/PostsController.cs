using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Radar.Api.Data;
using Radar.Api.Models.Dto;

namespace Radar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly RadarContext _context;

        public PostsController(RadarContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostReadDto>>> GetPost()
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }

            List<Post> posts = await _context.Posts.ToListAsync();
            return Ok(posts.ToReadDto(_context));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostReadDto>> GetPost(int id)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post.ToReadDto(_context));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, Post post)
        {
            if (id != post.PostId)
            {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
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
        public async Task<ActionResult<Post>> PostPost(PostCreateDto post)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'RadarContext.Post'  is null.");
            }

            if (!_context.Pessoas.AnyAsync(pessoa => pessoa.PessoaId == post.PessoaId).Result)
            {
                return Problem("Entity set 'RadarContext.Pessoa' does not contain the specified key.");
            }

            if (!_context.Locals.AnyAsync(local => local.LocalId == post.LocalId).Result)
            {
                return Problem("Entity set 'RadarContext.Local' does not contain the specified key.");
            }

            post.PostId = GetNextId();

            _context.Posts.Add(post.ToModel(_context));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost", new { id = post.PostId }, post);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("FromPessoa/{pessoaId}")]
        public async Task<ActionResult<IEnumerable<PostReadDto>>> GetPostFromPessoa(int pessoaId)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }

            List<Post> posts = await _context.Posts.Where(post => post.PessoaId == pessoaId).ToListAsync();

            if (posts.Count == 0)
            {
                return NotFound();
            }

            return Ok(posts.ToReadDto(_context));
        }

        private bool PostExists(int id)
        {
            return (_context.Posts?.Any(e => e.PostId == id)).GetValueOrDefault();
        }

        private int GetNextId()
        {
            return (_context.Posts?.Max(e => e.PostId) ?? 0) + 1;
        }
    }
}