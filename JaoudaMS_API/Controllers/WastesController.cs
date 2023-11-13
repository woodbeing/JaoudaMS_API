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
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JaoudaMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WastesController : ControllerBase
    {
        private readonly JaoudaSmContext _context;
        private readonly IMapper _mapper;

        public WastesController(JaoudaSmContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region GET Methodes

        #region api/Wastes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WasteDto>>> GetWastes()
        {
            if (_context.Wastes == null)
                return Problem("la base du donnes ou le table Faut n'exite pas.");

            return await _context.Wastes.Select(waste => _mapper.Map<WasteDto>(waste)).ToListAsync();
        }
        #endregion
        #region api/Wastes/{product}
        [HttpGet("{product}")]
        public async Task<ActionResult<IEnumerable<WasteDto>>> GetWaste(string product)
        {
            if (_context.Wastes == null)
                return Problem("la base du donnes ou le table Faut n'exite pas.");

            var waste = _mapper.Map<IEnumerable<WasteDto>>(await _context.Wastes.Where(wst => wst.Product == product).ToListAsync());

            if (waste.Count() == 0)
                return NotFound();

            return Ok(waste);
        }
        #endregion
        #region api/Wastes/{product}/{type}
        [HttpGet("{product}/{type}")]
        public async Task<ActionResult<WasteDto>> GetWaste(string product, string type )
        {
            if (_context.Wastes == null)
                return Problem("la base du donnes ou le table Faut n'exite pas.");

            var waste = _mapper.Map<WasteDto>(await _context.Wastes.FindAsync(product, type));

            if (waste == null)
                return NotFound();

            return Ok(waste);
        }
        #endregion

        #endregion

        #region POST Methodes

        #region api/Wastes
        [HttpPost]
        public async Task<ActionResult<WasteDto>> PostWaste(WasteDto waste)
        {
            if (_context.Wastes == null)
                return Problem("la base du donnes ou le table Faut n'exite pas.");

            if (WasteExists(waste.Product, waste.Type))
                Problem("Ce Type de Faut deja Exist!");

            _context.Wastes.Add(_mapper.Map<Waste>(waste));

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException) { throw; }

            return Ok(waste);
        }
        #endregion

        #endregion

        #region PUT Methodes

        #region api/Wastes/{product}/{type}/{qtt}
        [HttpPut("{product}/{type}/{qtt}")]
        public async Task<IActionResult> PutWaste(string product, string type, int qtt)
        {
            if (_context.Wastes == null)
                return Problem("la base du donnes ou le table Faut n'exite pas.");

            var waste = await _context.Wastes.FindAsync(product, type);

            if (waste == null)
                return NotFound();
            
            waste.Qtt += qtt;

            _context.Entry(waste).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }

            return Ok(_mapper.Map<WasteDto>(waste));
        }
        #endregion

        #endregion

        #region DELETE Methodes

        #region api/Wastes/{product}/{type}/{qtt}
        [HttpDelete("{product}/{type}/{qtt}")]
        public async Task<IActionResult> DeleteWaste(string product, string type, int qtt)
        {
            if (_context.Wastes == null)
                return Problem("la base du donnes ou le table Faut n'exite pas.");

            var waste = await _context.Wastes.FindAsync(product, type);
            
            if (waste == null)
                return NotFound();

            waste.Qtt -= qtt;

            if (waste.Qtt < 0) return Problem("tu ne peux pas descendre en dessous de 0");

            _context.Wastes.Entry(waste).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException) { throw; }

            return Ok(_mapper.Map<WasteDto>(waste));
        }
        #endregion

        #endregion

        private bool WasteExists(string product, string type)
        {
            return (_context.Wastes?.Any(e => e.Product == product && e.Type == type)).GetValueOrDefault();
        }
    }
}
