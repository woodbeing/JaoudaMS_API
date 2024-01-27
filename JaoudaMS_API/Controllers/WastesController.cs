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
    public class WastesController : ControllerBase
    {
        private readonly JaoudaSmContext _context;
        private readonly IMapper _mapper;

        public WastesController(JaoudaSmContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Wastes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WasteDto>>> GetWastes()
        {
            if (_context.Wastes == null)
                return NotFound();

            return Ok(await _context.Wastes
                .OrderBy(waste => waste.Product)
                .Select(waste => _mapper.Map<WasteDto>(waste))
                .ToListAsync());
        }

        // GET: api/Wastes/5
        [HttpGet("{product}/{type}")]
        public async Task<ActionResult<WasteDto>> GetWaste(string product, string type)
        {
            if (_context.Wastes == null)
                return NotFound();

            var waste = await _context.Wastes.FindAsync(product, type);

            if (waste == null)
                return NotFound();

            return Ok(_mapper.Map<WasteDto>(waste));
        }

        // POST: api/Wastes
        [HttpPost]
        public async Task<ActionResult<WasteDto>> PostWaste(WasteDto waste)
        {
            if (_context.Wastes == null)
                return NotFound();

            if (WasteExists(waste.Product, waste.Type))
                return Conflict(new { detail = "Ce Type de Faut deja Exist!" });

            try
            {
                _context.Wastes.Add(_mapper.Map<Waste>(waste));
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException) { throw; }

            return Ok(waste);
        }

        // PUT: api/Wastes/5
        [HttpPut("{product}/{type}")]
        public async Task<ActionResult<WasteDto>> PutWaste(string product, string type, WasteDto waste)
        {
            if (product != waste.Product)
                return NotFound();

            if (!WasteExists(product, type))
                return NotFound();

            try
            {
                _context.Entry(_mapper.Map<Waste>(waste)).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException){ throw; }

            return Ok(waste);
        }

        // DELETE: api/Wastes/5
        [HttpDelete("{product}/{type}/{quantity}")]
        public async Task<ActionResult<WasteDto>> DeleteWaste(string product, string type, int quantity)
        {
            if (_context.Wastes == null)
                return NotFound();

            var waste = await _context.Wastes.FindAsync(product, type);

            if (waste == null)
                return NotFound();

            waste.Qtt -= quantity;

            if (waste.Qtt < 0)
                return Conflict(new { detail = "Tu ne peux pas descendre en dessous de 0" });

            try
            {
                _context.Entry(waste).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException) { return Conflict(); }

            return Ok(_mapper.Map<WasteDto>(waste));
        }

        private bool WasteExists(string product, string type)
        {
            return (_context.Wastes?.Any(waste => waste.Product == product && waste.Type == type)).GetValueOrDefault();
        }
    }
}
