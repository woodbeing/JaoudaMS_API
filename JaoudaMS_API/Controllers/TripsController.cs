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
    public class TripsController : ControllerBase
    {
        private readonly JaoudaSmContext _context;
        private readonly IMapper _mapper;

        public TripsController(JaoudaSmContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Trips
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripDto>>> GetTrips()
        {
            if (_context.Trips == null)
                return NotFound();

            return await _context.Trips
                .OrderByDescending(trip => trip.Date)
                .Select(trip => _mapper.Map<TripDto>(trip))
                .ToListAsync();
        }

        // GET: api/Trips/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TripDto>> GetTrip(string id)
        {
            if (_context.Trips == null)
                return NotFound();

            var trip = await _context.Trips
                .Include(trip => trip.TripProducts)
                .Include(trip => trip.TripBoxes)
                .Include(trip => trip.TripCharges)
                .Include(trip => trip.TripWastes)
                .FirstAsync(trip => trip.Id == id);

            if (trip == null)
                return NotFound();

            return Ok(_mapper.Map<TripDto>(trip));
        }

        // GET: api/Trips/last
        [HttpGet("last/{truck}")]
        public async Task<ActionResult<TripDto>> GetLastTrip(string truck)
        {
            if (_context.Trips == null)
                return NotFound();

            var trip = await _context.Trips
                .OrderByDescending(trip => trip.Date)
                .Include(trip => trip.TripProducts)
                .Include(trip => trip.TripBoxes)
                .FirstAsync(trip => trip.Truck == truck);

            foreach(TripProduct tripP in  trip.TripProducts)
            {
                tripP.QttOut = (short?)(tripP.QttOut - tripP.QttSold);
            }

            if (trip == null)
                return NotFound();

            return Ok(_mapper.Map<TripDto>(trip));
        }

        // POST: api/Trips
        [HttpPost]
        public async Task<ActionResult<TripDto>> PostTrip(TripDto trip)
        {
            if (_context.Trips == null)
                return NotFound();

            if (TripExists(trip.Id))
                return Conflict(new { detail = "Cette Voyage Deja Exist" });

            try
            {
                foreach(TripProductDto tripP in trip.TripProducts) 
                {
                    var product = await _context.Products.FindAsync(tripP.Product);
                    
                    if (product == null) return NotFound();
                    
                    product.Stock -= tripP.QttOut;

                    _context.Entry(product).State = EntityState.Modified;
                }

                foreach(TripBoxDto tripB in trip.TripBoxes) 
                {
                    var box = await _context.Boxes.FindAsync(tripB);

                    if (box == null) return NotFound();

                    #pragma warning disable
                    box.InStock -= (short)tripB.QttOut;
                    #pragma warning restore

                    _context.Entry(box).State = EntityState.Modified;
                }

                _context.Trips.Add(_mapper.Map<Trip>(trip));
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException){ throw; }

            return Ok(trip);
        }

        // PUT: api/Trips/5
        [HttpPut("{id}")]
        public async Task<ActionResult<TripDto>> PutTrip(string id, TripDto trip)
        {
            if (_context.Trips == null)
                return NotFound();

            if (id != trip.Id)
                return NotFound();

            if (!TripExists(id))
                return NotFound();

            try
            {
                _context.Entry(_mapper.Map<Trip>(trip)).State = EntityState.Modified;
 
                foreach (TripBoxDto tripB in trip.TripBoxes)
                {
                    var tripBox = await _context.TripBoxes.FindAsync(tripB.Trip, tripB.Box);
                    var box = await _context.Boxes.FindAsync(tripB.Box);

                    if (tripBox == null) return NotFound();

                    #pragma warning disable
                    box.Empty += tripBox.QttIn;
                    #pragma warning restore

                    _context.Entry(box).State = EntityState.Modified;
                    _context.Entry(_mapper.Map<TripBox>(tripB)).State = EntityState.Modified;
                }

                foreach (TripProductDto tripP in trip.TripProducts)
                {
                    var tripProduct = await _context.TripProducts.FindAsync(tripP.Trip, tripP.Product);

                    if (tripProduct == null) NotFound();
                    
                    _context.Entry(_mapper.Map<TripProduct>(tripP)).State = EntityState.Modified;
                    _context.TripProducts.Add(_mapper.Map<TripProduct>(tripProduct));
                }
                
                foreach (TripWasteDto tripW in trip.TripWastes) 
                {
                    var waste = await _context.Wastes.FindAsync(tripW.Product, tripW.Type);

                    if (waste == null) _context.Wastes.Add(new Waste()
                        {
                            Product = tripW.Product,
                            Type = tripW.Type,
                            Qtt = tripW.Qtt,
                        });
                    else
                    {
                        waste.Qtt += tripW.Qtt;
                        _context.Entry(waste).State = EntityState.Modified;
                    }
                }

                foreach (TripChargeDto tripC  in trip.TripCharges) 
                    _context.TripCharges.Add(_mapper.Map<TripCharge>(tripC));

                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException) { throw; }

            return Ok(trip);
        }

        private bool TripExists(string id)
        {
            return (_context.Trips?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
