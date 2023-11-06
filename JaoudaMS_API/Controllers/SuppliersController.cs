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
using NuGet.Protocol;

namespace JaoudaMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly JaoudaSmContext _context;
        private readonly IMapper _mapper;

        public SuppliersController(JaoudaSmContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Suppliers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetSuppliers()
        {
            if (_context.Suppliers == null)
                return Problem("la base du donnes ou le table caisse n'exite pas.");

            return await _context.Suppliers.Select(sup => _mapper.Map<SupplierDto>(sup)).ToListAsync();
        }

        // GET: api/Suppliers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierDto>> GetSupplier(string id)
        {
            if (_context.Suppliers == null)
                return Problem("la base du donnes ou le table caisse n'exite pas.");

            var supplier = _mapper.Map<SupplierDto>(await _context.Suppliers.FindAsync(id));

            if (supplier == null)
                return NotFound();

            return Ok(supplier);
        }

        // PUT: api/Suppliers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplier(string id, SupplierDto supplier)
        {
            if (id != supplier.Id)
                return NotFound();

            if (!SupplierExists(id))
                return NotFound();

            _context.Entry(supplier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/Suppliers
        [HttpPost]
        public async Task<ActionResult<SupplierDto>> PostSupplier(SupplierDto supplier)
        {
            if (_context.Suppliers == null)
                return Problem("la base du donnes ou le table caisse n'exite pas.");

            if (SupplierExists(supplier.Id))
                return Conflict(new { statusCode = Conflict().StatusCode, massage = "ce Fournisseur deja existe" });

            _context.Suppliers.Add(_mapper.Map<Supplier>(supplier));

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction("PostSupplier", new { id = supplier.Id }, supplier);
        }

        // DELETE: api/Suppliers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<PurchaseDto>>> DeleteSupplier(string id)
        {
            if (_context.Suppliers == null)
                return Problem("la base du donnes ou le table caisse n'exite pas.");

            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
                return NotFound();

            try
            {
                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException) 
            {
                return Ok(_mapper.Map<IEnumerable<PurchaseDto>>(await _context.Purchases.Where(purchase => purchase.Supplier == id).ToListAsync()));
            }

            return NoContent();
        }

        private bool SupplierExists(string id)
        {
            return (_context.Suppliers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
