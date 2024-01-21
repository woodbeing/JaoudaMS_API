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

        #region GET Methodes

        #region api/Suppliers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetSuppliers()
        {
            if (_context.Suppliers == null)
                return Problem("la base du donnes ou le table Fournisseur n'exite pas.");

            return Ok(await _context.Suppliers.Select(sup => _mapper.Map<SupplierDto>(sup)).ToListAsync());
        }
        #endregion
        #region api/Suppliers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierDto>> GetSupplier(string id)
        {
            if (_context.Suppliers == null)
                return Problem("la base du donnes ou le table Fournisseur n'exite pas.");

            var supplier = _mapper.Map<SupplierDto>(await _context.Suppliers.FindAsync(id));

            if (supplier == null)
                return NotFound();

            return Ok(supplier);
        }
        #endregion

        #endregion

        #region POST Methodes

        #region api/Suppliers
        [HttpPost]
        public async Task<ActionResult<SupplierDto>> PostSupplier(SupplierDto supplier)
        {
            if (_context.Suppliers == null)
                return Problem("la base du donnes ou le table Fournisseur n'exite pas.");

            if (SupplierExists(supplier.Id))
                return Conflict(new { title = "Impossible d'Ajouter!", deteil = "Ce Fournisseur deja existe" });

            _context.Suppliers.Add(_mapper.Map<Supplier>(supplier));

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException) { throw; }

            return Ok(supplier);
        }
        #endregion

        #endregion

        #region PUT Methodes

        #region api/Suppliers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplier(string id, SupplierDto supplier)
        {
            if (_context.Suppliers == null)
                return Problem("la base du donnes ou le table Fournisseur n'exite pas.");

            if (!SupplierExists(id))
                return NotFound();

            if (id != supplier.Id)
                return Conflict(new {title = "Impossible de Modifier!", detail = "Impossible de changer l'ID d'un Fournisseur" });

            _context.Entry(_mapper.Map<Supplier>(supplier)).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }

            return Ok(supplier);
        }
        #endregion

        #endregion

        #region DELETE Methodes

        #region api/Suppliers/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSupplier(string id)
        {
            if (_context.Suppliers == null)
                return Problem("la base du donnes ou le table Fournisseur n'exite pas.");

            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
                return NotFound();

            try
            {
                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException) { return Conflict(new { title = "Impossible de Effacer!", detail = "Le Fournisseur a des achats" }); }

            return Ok(_mapper.Map<SupplierDto>(supplier));
        }
        #endregion

        #endregion

        private bool SupplierExists(string id)
        {
            return (_context.Suppliers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
