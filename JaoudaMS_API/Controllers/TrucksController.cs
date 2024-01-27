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
    public class TrucksController : ControllerBase
    {
        private readonly JaoudaSmContext _context;
        private readonly IMapper _mapper;

        public TrucksController(JaoudaSmContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Trucks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TruckDto>>> GetTrucks()
        {
            if (_context.Trucks == null)
                return NotFound();

            return Ok(await _context.Trucks
                .OrderBy(truck => truck.Type)
                .Select(truck => _mapper.Map<TruckDto>(truck))
                .ToListAsync());
        }

        // GET: api/Trucks/5
        [HttpGet("{matricula}")]
        public async Task<ActionResult<TruckDto>> GetTruck(string matricula)
        {
            if (_context.Trucks == null)
                return NotFound();

            var truck = await _context.Trucks.FindAsync(matricula);

            if (truck == null)
                return NotFound();

            return Ok(_mapper.Map<TruckDto>(truck));
        }

        // POST: api/Trucks
        [HttpPost]
        public async Task<ActionResult<TruckDto>> PostTruck(TruckDto truck)
        {
            if (_context.Trucks == null)
                return NotFound();

            if (TruckExists(truck.Matricula))
                return Conflict();

            try
            {
                _context.Trucks.Add(_mapper.Map<Truck>(truck));
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException){ throw; }

            return Ok(truck);
        }

        // PUT: api/Trucks/5
        [HttpPut("{matricula}")]
        public async Task<ActionResult<TruckDto>> PutTruck(string matricula, TruckDto truck)
        {
            if (_context.Trucks == null)
                return NotFound();

            if (matricula != truck.Matricula)
                return BadRequest();
            
            if (!TruckExists(matricula))
                return NotFound();

            try
            {
                _context.Entry(truck).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException){ throw; }

            return Ok(truck);
        }

        // DELETE: api/Trucks/5
        [HttpDelete("{matricula}")]
        public async Task<ActionResult<TruckDto>> DeleteTruck(string matricula)
        {
            if (_context.Trucks == null)
                return NotFound();

            var truck = await _context.Trucks.FindAsync(matricula);
            
            if (truck == null)
                return NotFound();
            try
            {
                _context.Trucks.Remove(truck);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException) { Conflict(new { detail = "assurez-vous de supprimer un produit ajouté par accident." }); }

            return Ok(_mapper.Map<TruckDto>(truck));
        }

        private bool TruckExists(string matricula)
        {
            return (_context.Trucks?.Any(e => e.Matricula == matricula)).GetValueOrDefault();
        }
    }
}
