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
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Boxes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoxDto>>> GetBoxes()
        {
            if (_context.Boxes == null)
                return NotFound();

            return Ok(await _context.Boxes.Select(box => _mapper.Map<BoxDto>(box)).ToListAsync());
        }

        // GET: api/Boxes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BoxDto>> GetBox(string id)
        {
            if (_context.Boxes == null)
                return NotFound();

            var box = _mapper.Map<BoxDto>(await _context.Boxes.FindAsync(id));

            if (box == null)
                return NotFound();

            return Ok(box);
        }

        // POST: api/Boxes
        [HttpPost]
        public async Task<ActionResult<BoxDto>> PostBox(BoxDto box)
        {
            if (_context.Boxes == null)
                return NotFound();

            if (BoxExists(box.Id))
                return Conflict(new { detail = "cette Caisse deja existe" });
            
            _context.Boxes.Add(_mapper.Map<Box>(box));

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException){ throw; }

            return Ok(_mapper.Map<BoxDto>(box));
        }

        // PUT: api/Boxes/5
        [HttpPut("{id}")]
        public async Task<ActionResult<BoxDto>> PutBox(string id, BoxDto box)
        {
            if (_context.Boxes == null)
                return NotFound();

            if (!BoxExists(id))
                return NotFound();

            if (id != box.Id)
                return BadRequest();

            try
            {
                _context.Entry(box).State = EntityState.Modified;
                await _context.SaveChangesAsync(); 
            }
            catch (DbUpdateException) { throw; }

            return Ok(box);
        }

        // DELETE: api/Boxes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BoxDto>> DeleteBox(string id)
        {
            if (_context.Boxes == null)
                return NotFound();

            var box = await _context.Boxes.FindAsync(id);

            if (box == null)
                return NotFound();
            try
            {
                _context.Boxes.Remove(box);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException) { Conflict("Assurez Vous supprimez une caisse ajouter par erreur"); }
            

            return Ok(_mapper.Map<BoxDto>(box));
        }

        private bool BoxExists(string id)
        {
            return (_context.Boxes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
