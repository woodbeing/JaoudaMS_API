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
    public class InBoxesController : ControllerBase
    {
        private readonly JaoudaSmContext _context;
        private readonly IMapper _mapper;

        public InBoxesController(JaoudaSmContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region GET Methodes

        #region api/InBoxes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InBoxDto>>> GetInBoxes()
        {
            if (_context.InBoxes == null)
                return Problem("la base du donnes ou le table Contenue Caisse n'exite pas.");

            return await _context.InBoxes.Select(inbox => _mapper.Map<InBoxDto>(inbox)).ToListAsync();
        }
        #endregion
        #region api/InBoxes/{product}/{box}
        [HttpGet("{product}/{box}")]
        public async Task<ActionResult<InBoxDto>> GetInBox(string product, string box)
        {
            if (_context.InBoxes == null)
                return Problem("la base du donnes ou le table Contenue Caisse n'exite pas.");

            var inBox = _mapper.Map<InBoxDto>(await _context.InBoxes.FindAsync(product, box));

            if (inBox == null)
                return NotFound();

            return Ok(inBox);
        }
        #endregion

        #endregion

        #region POST Methodes

        #region api/InBoxes
        [HttpPost]
        public async Task<ActionResult<InBoxDto>> PostInBox(InBoxDto inBox)
        {
            if (_context.InBoxes == null)
                return Problem("la base du donnes ou le table Contenue Caisse n'exite pas.");

            if (InBoxExists(inBox.Product, inBox.Box))
                return Conflict(new { title = "Impossible d'Ajouter!", detail = "Cette information deja existe!" });

            _context.InBoxes.Add(_mapper.Map<InBox>(inBox));

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException) { throw; }

            return Ok(inBox);
        }
        #endregion

        #endregion

        #region PUT Methodes

        #region api/InBoxes/{product}/{box}
        [HttpPut("{product}/{box}")]
        public async Task<IActionResult> PutInBox(string product, string box, InBox inBox)
        {
            if (_context.InBoxes == null)
                return Problem("la base du donnes ou le table Contenue Caisse n'exite pas.");

            if (!InBoxExists(product, box))
                return NotFound();

            if ((inBox.Product, inBox.Box ) != (product, box ))
                return Conflict(new { title = "Impossible de mise a jour", detail = "ne peut pas traiter les informations fournies" });

            _context.Entry(inBox).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }

            return Ok(inBox);
        }
        #endregion

        #endregion

        #region DELETE Methodes

        #region api/InBoxes/{product}/{box}
        [HttpDelete("{product}/{box}")]
        public async Task<IActionResult> DeleteInBox(string product, string box)
        {
            if (_context.InBoxes == null)
                return Problem("la base du donnes ou le table Contenue Caisse n'exite pas.");

            var inBox = await _context.InBoxes.FindAsync(product, box);

            if (inBox == null)
                return NotFound();

            try
            {
                _context.InBoxes.Remove(inBox);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException) { return Conflict(new { title = "Impossible de Supprimer" , detail = "Assurez Vous supprimez une caisse ajouter par erreur" }); }

            return Ok(_mapper.Map<InBoxDto>(inBox));
        }
        #endregion

        #endregion

        private bool InBoxExists(string product, string box)
        {
            return (_context.InBoxes?.Any(e => e.Product == product && e.Box == box)).GetValueOrDefault();
        }
    }
}
