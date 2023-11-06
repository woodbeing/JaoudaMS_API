using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JaoudaMS_API.Models;
using AutoMapper;
using JaoudaMS_API.DTOs;

namespace JaoudaMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoxesController : ControllerBase
    {
        private readonly JaoudaSmContext _context;
        private readonly IMapper _mapper;

        public BoxesController(JaoudaSmContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        // GET: api/Boxes
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<BoxDto>>> GetBoxes()
        {
            if (_context.Boxes == null)
                return Problem("la base du donnes ou le table caisse n'exite pas.");

            return await _context.Boxes.Select(box => _mapper.Map<BoxDto>(box)).ToListAsync();
        }

        // GET: api/Boxes/5
        [HttpGet("{type}")]
        public async Task<ActionResult<BoxDto>> GetBox(string type)
        {
            if (_context.Boxes == null) 
                return Problem("la base du donnes ou le table caisse n'exite pas.");

            var box = _mapper.Map<BoxDto>(await _context.Boxes.FindAsync(type));

            if (box == null)
                return NotFound();

            return box;
        }

        // PUT: api/Boxes/5
        //[HttpPut("{type}")]
        //public async Task<IActionResult> PutBox(string type, BoxDto box)
        //{
        //    if (type != box.Type)
        //        return BadRequest();

        //    if (!BoxExists(type))
        //        return NotFound();

        //    _context.Entry(_mapper.Map<Box>(box)).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        throw;
        //    }

        //    return NoContent();
        //}

        // POST: api/Boxes
        [HttpPost]
        public async Task<ActionResult<BoxDto>> PostBox(BoxDto box)
        {
            if (_context.Boxes == null)
                return Problem("la base du donnes ou le table caisse n'exite pas.");

            if (BoxExists(box.Type))
                return Conflict(new { statusCode = Conflict().StatusCode, massage = "cette Caisse deja existe"});

            _context.Boxes.Add(_mapper.Map<Box>(box));

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction("PostBox", new { id = box.Type }, box);
        }

        // DELETE: api/Boxes/5
        [HttpDelete("{type}")]
        public async Task<IActionResult> DeleteBox(string type)
        {
            if (_context.Boxes == null)
                return Problem("la base du donnes ou le table caisse n'exite pas.");

            var box = await _context.Boxes.FindAsync(type);

            if (box == null)
                return NotFound();

            if (_mapper.Map<BoxDto>(await _context.Boxes.FirstOrDefaultAsync(box => box.SubBox == type)) != null) 
                return Conflict(
                    new
                    {
                        statusCode = Conflict().StatusCode,
                        message = $"Assurez-vous de supprimer une boîte créée par accident.\n(Il est impossible de supprimer une caisse déjà utilisée dans d'autres opérations.)"
                    });

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
