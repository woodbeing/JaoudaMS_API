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

        #region GET Methodes

        #region api/Trucks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TruckDto>>> GetTrucks()
        {
          if (_context.Trucks == null)
                return Problem("la base du donnes ou le table Camion n'exite pas.");

            return Ok(await _context.Trucks.Select(truck => _mapper.Map<TruckDto>(truck)).ToListAsync());
        }
        #endregion
        #region api/Trucks/{matricula}
        [HttpGet("{matricula}")]
        public async Task<ActionResult<TruckDto>> GetTruck(string matricula)
        {
            if (_context.Trucks == null)
                return Problem("la base du donnes ou le table Camion n'exite pas.");

            var truck = _mapper.Map<TruckDto>(await _context.Trucks.FindAsync(matricula));

            if (truck == null)
                return NotFound();

            return Ok(truck);
        }
        #endregion

        #endregion

        #region POST Methodes

        #region api/Trucks
        [HttpPost]
        public async Task<ActionResult<TruckDto>> PostTruck(TruckDto truck)
        {
            if (_context.Trucks == null)
                return Problem("la base du donnes ou le table Camion n'exite pas.");

            if (TruckExists(truck.Matricula))
                return Problem("Ce Camion Existe Déjà");

            _context.Trucks.Add(_mapper.Map<Truck>(truck));
            
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException) { throw; }

            return Ok(truck);
        }
        #endregion

        #endregion

        #region PUT Methodes

        #region api/Trucks/{matricula}
        [HttpPut("{matricula}")]
        public async Task<IActionResult> PutTruck(string matricula, TruckDto truck)
        {
            if (_context.Trucks == null)
                return Problem("la base du donnes ou le table Camion n'exite pas.");

            if (matricula != truck.Matricula)
                return Problem("Impossible de mise a jour matricula d'une camion");

            if (!TruckExists(matricula))
                return NotFound();

            _context.Entry(_mapper.Map<Truck>(truck)).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }

            return Ok(truck);
        }
        #endregion

        #endregion

        #region DELETE Methodes

        #region api/Trucks/{matricula}
        [HttpDelete("{matricula}")]
        public async Task<IActionResult> DeleteTruck(string matricula)
        {
            if (_context.Trucks == null)
                return Problem("la base du donnes ou le table Camion n'exite pas.");

            var truck = _mapper.Map<Truck>(await _context.Trucks.FindAsync(matricula));

            if (truck == null)
                return NotFound();

            try
            {
                _context.Trucks.Remove(truck);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException) { return Problem("Impossible de Effacer une Camion avec des voyages"); }

            return Ok(truck);
        }
        #endregion

        #endregion

        private bool TruckExists(string matricula)
        {
            return (_context.Trucks?.Any(e => e.Matricula == matricula)).GetValueOrDefault();
        }
    }
}
