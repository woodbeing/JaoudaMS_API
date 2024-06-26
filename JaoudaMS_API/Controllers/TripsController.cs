﻿using System;
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

            return Ok(await _context.Trips
                .OrderByDescending(trip => trip.Date)
                .Select(trip => _mapper.Map<TripDto>(trip))
                .ToListAsync());
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

            if (trip == null)
                return NotFound();

            foreach(TripProduct tripP in  trip.TripProducts)
            {
                tripP.QttOut = (short?)(tripP.QttOut - tripP.QttSold);
                tripP.QttSold = 0;
            }

            foreach(TripBox tripB in trip.TripBoxes)
            {
                tripB.QttOut = (short?)(tripB.QttOut - tripB.QttIn);
                tripB.QttIn = 0;
            }

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

            var employeeTyp = await _context.Employees.FindAsync(trip.Driver);

            try
            {
                foreach(TripProductDto tripP in trip.TripProducts) 
                {
                    var product = await _context.Products.FindAsync(tripP.Product);
                    
                    if (product == null) return NotFound();
                    
                    product.Stock -= tripP.QttOut;

                    tripP.Price = product.Price;

                    #pragma warning disable
                    if (employeeTyp.Type == "Professional")
                        tripP.Price = product.PriceProfessional;
                    #pragma warning restore

                    _context.Entry(product).State = EntityState.Modified;
                }

                foreach(TripBoxDto tripB in trip.TripBoxes) 
                {
                    var box = await _context.Boxes.FindAsync(tripB.Box);

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
                var driver = await _context.Employees.FindAsync(trip.Driver);
                var seller = await _context.Employees.FindAsync(trip.Seller);


                foreach (TripBoxDto tripB in trip.TripBoxes)
                {
                    var box = await _context.Boxes.FindAsync(tripB.Box);

                    #pragma warning disable
                    box.Empty += tripB.QttIn;
                    #pragma warning restore

                    _context.Entry(box).State = EntityState.Modified;
                    _context.Entry(_mapper.Map<TripBox>(tripB)).State = EntityState.Modified;
                }

                foreach (TripProductDto tripP in trip.TripProducts)
                {
                    var product = await _context.Products.FindAsync(tripP.Product);

                    #pragma warning disable
                    if (driver.Type != "Professional")
                    {
                        driver.Commission += (decimal?)(tripP.QttSold * product.CommissionDriver);
                        seller.Commission += (decimal?)(tripP.QttSold * product.CommissionSeller);
                    }
                    #pragma warning restore

                    _context.Entry(_mapper.Map<TripProduct>(tripP)).State = EntityState.Modified;
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

                    _context.TripWastes.Add(_mapper.Map<TripWaste>(tripW));
                }

                foreach (TripChargeDto tripC  in trip.TripCharges) 
                    _context.TripCharges.Add(_mapper.Map<TripCharge>(tripC));

                #pragma warning disable
                _context.Entry(driver).State = EntityState.Modified;
                _context.Entry(seller).State = EntityState.Modified;
                _context.Entry(_mapper.Map<Trip>(trip)).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                #pragma warning restore
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
