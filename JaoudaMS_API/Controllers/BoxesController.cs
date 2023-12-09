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

        #region GET Methodes

        #region api/Boxes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoxDto>>> GetBoxes()
        {
            if (_context.Boxes == null)
                return Problem("la base du donnes ou le table caisse n'exite pas.");

            return Ok(await _context.Boxes.Select(box => _mapper.Map<BoxDto>(box)).ToListAsync());
        }
        #endregion
        #region api/Boxes/{type}
        [HttpGet("{type}")]
        public async Task<ActionResult<BoxDto>> GetBox(string type)
        {
            if (_context.Boxes == null) 
                return Problem("la base du donnes ou le table caisse n'exite pas.");

            var box = _mapper.Map<BoxDto>(await _context.Boxes.FindAsync(type));

            if (box == null)
                return NotFound();

            return Ok(box);
        }
        #endregion

        #endregion

        #region POST Methodes

        #region api/Boxes
        [HttpPost]
        public async Task<ActionResult<BoxDto>> PostBox(BoxDto box)
        {
            if (_context.Boxes == null)
                return Problem("la base du donnes ou le table caisse n'exite pas.");

            if (BoxExists(box.Type))
                return Conflict(new { title = "Impossible d'Ajouter!" ,detail = "cette Caisse deja existe" });

            _context.Boxes.Add(_mapper.Map<Box>(box));

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException) { throw; }

            return Ok(box);
        }
        #endregion

        #endregion

        #region DELETE Methodes

        #region api/Boxes/{Delete}
        [HttpDelete("{type}")]
        public async Task<IActionResult> DeleteBox(string type)
        {
            if (_context.Boxes == null)
                return Problem("la base du donnes ou le table caisse n'exite pas.");

            var box = await _context.Boxes.FindAsync(type);

            if (box == null)
                return NotFound();

            _context.Boxes.Remove(box);

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException) {  return Conflict(new { title = "Impossible de Supprimer!", detail = "Assurez Vous supprimez une caisse ajouter par erreur" }); }
            
            return Ok(_mapper.Map<BoxDto>(box));
        }
        #endregion

        #endregion

        private bool BoxExists(string id)
        {
            return (_context.Boxes?.Any(e => e.Type == id)).GetValueOrDefault();
        }
    }
}
