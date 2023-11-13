using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JaoudaMS_API.Models;
using JaoudaMS_API.DTOs;
using AutoMapper;

namespace JaoudaMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly JaoudaSmContext _context;
        private readonly IMapper _mapper;

        public TripsController(JaoudaSmContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region GET Methodes

        #region api/Trips
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripDto>>> GetTrips()
        {
            if (_context.Trips == null)
                return Problem("la base du donnes ou le table Voyage n'exite pas.");

            return Ok(await _context.Trips.Select(trip => _mapper.Map<TripDto>(trip)).ToListAsync());
        }
        #endregion
        #region api/Trips/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TripDto>> GetTrip(string id)
        {
            if (_context.Trips == null)
                return Problem("la base du donnes ou le table Voyage n'exite pas.");

            var trip = _mapper.Map<TripDto>(await _context.Trips.FindAsync(id));

            if (trip == null)
                return NotFound();

            return Ok(trip);
        }
        #endregion

        #endregion

        #region POST Methodes

        #region api/Trips
        [HttpPost]
        public async Task<ActionResult<TripDto>> PostTrip(TripDto trip)
        {
            if (_context.Trips == null)
                return Problem("la base du donnes ou le table Voyage n'exite pas.");

            if (TripExists(trip.Id)) 
                return Problem("Cette Voyage Deja Exist");

            _context.Trips.Add(_mapper.Map<Trip>(trip));

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException) { throw; }

            return Ok(trip);
        }
        #endregion

        #endregion

        #region PUT Methodes

        #region api/Trips/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrip(string id, TripDto trip)
        {
            if (_context.Trips == null)
                return Problem("la base du donnes ou le table Voyage n'exite pas.");

            if (id != trip.Id)
                return Problem("Impossible de mise A jour cette Voyage");

            if (!TripExists(id))
                return NotFound();

            try
            {
                _context.Entry(trip).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) { Problem("Impossible de mise A jour cette Voyage"); }

            return Ok(trip);
        }
        #endregion

        #endregion

        private bool TripExists(string id)
        {
            return (_context.Trips?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
