using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialApis.Models;

namespace SocialApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class likesController : ControllerBase
    {
        private readonly SocialDbContext _context;

        public likesController(SocialDbContext context)
        {
            _context = context;
        }

        // GET: api/likes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<like>>> Getlikes()
        {
          if (_context.likes == null)
          {
              return NotFound();
          }
            return await _context.likes.ToListAsync();
        }

        // GET: api/likes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<like>> Getlike(int id)
        {
          if (_context.likes == null)
          {
              return NotFound();
          }
            var like = await _context.likes.FindAsync(id);

            if (like == null)
            {
                return NotFound();
            }

            return like;
        }

        // PUT: api/likes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putlike(int id, like like)
        {
            if (id != like.like_id)
            {
                return BadRequest();
            }

            _context.Entry(like).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!likeExists(id))
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

        // POST: api/likes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<like>> Postlike(like like)
        {
          if (_context.likes == null)
          {
              return Problem("Entity set 'SocialDbContext.likes'  is null.");
          }
            _context.likes.Add(like);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (likeExists(like.like_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("Getlike", new { id = like.like_id }, like);
        }

        // DELETE: api/likes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletelike(int id)
        {
            if (_context.likes == null)
            {
                return NotFound();
            }
            var like = await _context.likes.FindAsync(id);
            if (like == null)
            {
                return NotFound();
            }

            _context.likes.Remove(like);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool likeExists(int id)
        {
            return (_context.likes?.Any(e => e.like_id == id)).GetValueOrDefault();
        }
    }
}
