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
                return NotFound();

            return Ok(await _context.Suppliers
                .Select(supplier => _mapper.Map<SupplierDto>(supplier))
                .ToListAsync());
        }

        // GET: api/Suppliers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierDto>> GetSupplier(string id)
        {
            if (_context.Suppliers == null)
                return NotFound();

            var supplier = await _context.Suppliers
                .Include(supplier => supplier.Purchases
                    .OrderByDescending(purchase => purchase.Date))
                .FirstAsync(supplier => supplier.Id == id);

            if (supplier == null)
                return NotFound();

            return Ok(_mapper.Map<SupplierDto>(supplier));
        }

        // POST: api/Suppliers
        [HttpPost]
        public async Task<ActionResult<SupplierDto>> PostSupplier(SupplierDto supplier)
        {
            if (_context.Suppliers == null)
                return NotFound();

            if (SupplierExists(supplier.Id))
                return Conflict(new { deteil = "Ce Fournisseur deja existe" });

            try
            {
                _context.Suppliers.Add(_mapper.Map<Supplier>(supplier));
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException){ throw; }

            return Ok(supplier);
        }

        // PUT: api/Suppliers/5
        [HttpPut("{id}")]
        public async Task<ActionResult<SupplierDto>> PutSupplier(string id, SupplierDto supplier)
        {
            if (_context.Suppliers == null)
                return NotFound();

            if (id != supplier.Id)
                return NotFound();

            if (!SupplierExists(id))
                return NotFound();

            try
            {
                _context.Entry(_mapper.Map<Supplier>(supplier)).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException) { throw; }

            return Ok(supplier);
        }

        // DELETE: api/Suppliers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SupplierDto>> DeleteSupplier(string id)
        {
            if (_context.Suppliers == null)
                return NotFound();

            var supplier = await _context.Suppliers.FindAsync(id);
            
            if (supplier == null)
                return NotFound();

            try
            {
                _context.Suppliers.Remove(_mapper.Map<Supplier>(supplier));
                await _context.SaveChangesAsync();
            } 
            catch (DbUpdateException) 
            {
                Conflict( new { detail = "Le Fournisseur a des achats" } );
            }

            return Ok(supplier);
        }

        private bool SupplierExists(string id)
        {
            return (_context.Suppliers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
