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
    public class PostController : ControllerBase
    {
        private readonly RadarContext _context;

        public PostController(RadarContext context)
        {
            _context = context;
        }

        [HttpGet("{currentUserId}")]
        public async Task<ActionResult<IEnumerable<PostReadDto>>> GetPost(int currentUserId)
        {
            try
            {
                if (currentUserId <= 0)
                {
                    return BadRequest();
                }

                if (_context.Post == null)
                {
                    return NotFound();
                }

                List<Post> posts = await _context.Post.ToListAsync();
                return Ok(posts.ToReadDto(currentUserId, _context));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{currentUserId}/{id}")]
        public async Task<ActionResult<PostReadDto>> GetPost(int currentUserId, int id)
        {
            try
            {
                if (currentUserId <= 0)
                {
                    return BadRequest();
                }

                if (_context.Post == null)
                {
                    return NotFound();
                }

                var post = await _context.Post.FindAsync(id);

                if (post == null)
                {
                    return NotFound();
                }

                return Ok(post.ToReadDto(currentUserId, _context));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, Post post)
        {
            try
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
                catch (DbUpdateException)
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
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(PostCreateDto post)
        {
            try
            {
                if (_context.Post == null)
                {
                    return Problem("Entity set 'RadarContext.Post'  is null.");
                }

                if (!_context.Pessoa.AnyAsync(pessoa => pessoa.PessoaId == post.PessoaId).Result)
                {
                    return Problem("Entity set 'RadarContext.Pessoa' does not contain the specified key.");
                }

                if (!_context.Local.AnyAsync(local => local.LocalId == post.LocalId).Result)
                {
                    return Problem("Entity set 'RadarContext.Local' does not contain the specified key.");
                }

                int newId = GetNextId();
                _context.Post.Add(post.ToModel(newId, _context));
                await _context.SaveChangesAsync();

                return CreatedAtAction("PostPost", new { id = newId });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                if (_context.Post == null)
                {
                    return NotFound();
                }
                var post = await _context.Post.FindAsync(id);
                if (post == null)
                {
                    return NotFound();
                }

                _context.Post.Remove(post);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("FromPessoa/{currentUserId}/{pessoaId}")]
        public async Task<ActionResult<IEnumerable<PostReadDto>>> GetPostFromPessoa(int currentUserId, int pessoaId)
        {
            try
            {
                if (currentUserId <= 0)
                {
                    return BadRequest();
                }

                if (_context.Post == null)
                {
                    return NotFound();
                }

                List<Post> posts = await _context.Post.Where(post => post.PessoaId == pessoaId).ToListAsync();

                return Ok(posts.ToReadDto(currentUserId, _context));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("FromLocal/{currentUserId}/{localId}")]
        public async Task<ActionResult<IEnumerable<PostReadDto>>> GetPostFromLocal(int currentUserId, int localId)
        {
            try
            {
                if (currentUserId <= 0)
                {
                    return BadRequest();
                }

                if (_context.Post == null)
                {
                    return NotFound();
                }

                List<Post> posts = await _context.Post.Where(post => post.LocalId == localId).ToListAsync();

                return Ok(posts.ToReadDto(currentUserId, _context));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        private bool PostExists(int id)
        {
            return (_context.Post?.Any(e => e.PostId == id)).GetValueOrDefault();
        }

        private int GetNextId()
        {
            if (!_context.Post.Any()) return 1;
            return _context.Post.Max(e => e.PostId) + 1;
        }
    }
}