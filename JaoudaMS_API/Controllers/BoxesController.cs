using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JaoudaMS_API.Models;

namespace JaoudaMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoxesController : ControllerBase
    {
        private readonly JaoudaSmContext _context;

        public BoxesController(JaoudaSmContext context)
        {
            _context = context;
        }

        // GET: api/Boxes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Box>>> GetBoxes()
        {
          if (_context.Boxes == null)
          {
              return NotFound();
          }
            return await _context.Boxes.ToListAsync();
        }

        // GET: api/Boxes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Box>> GetBox(string id)
        {
          if (_context.Boxes == null)
          {
              return NotFound();
          }
            var box = await _context.Boxes.FindAsync(id);

            if (box == null)
            {
                return NotFound();
            }

            return box;
        }

        // PUT: api/Boxes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBox(string id, Box box)
        {
            if (id != box.Type)
            {
                return BadRequest();
            }

            _context.Entry(box).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoxExists(id))
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

        // POST: api/Boxes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Box>> PostBox(Box box)
        {
          if (_context.Boxes == null)
          {
              return Problem("Entity set 'JaoudaSmContext.Boxes'  is null.");
          }
            _context.Boxes.Add(box);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BoxExists(box.Type))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBox", new { id = box.Type }, box);
        }

        // DELETE: api/Boxes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBox(string id)
        {
            if (_context.Boxes == null)
            {
                return NotFound();
            }
            var box = await _context.Boxes.FindAsync(id);
            if (box == null)
            {
                return NotFound();
            }

            _context.Boxes.Remove(box);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BoxExists(string id)
        {
            return (_context.Boxes?.Any(e => e.Type == id)).GetValueOrDefault();
        }
    }
}
