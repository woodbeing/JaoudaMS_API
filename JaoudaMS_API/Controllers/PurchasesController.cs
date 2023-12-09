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
    public class PurchasesController : ControllerBase
    {
        private readonly JaoudaSmContext _context;
        private readonly IMapper _mapper;

        public PurchasesController(JaoudaSmContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region GET Methodes

        #region api/Purchases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseDto>>> GetPurchases()
        {
            if (_context.Purchases == null)
                return Problem("la base du donnes ou le table Achat n'exite pas.");

            return await _context.Purchases.Select(purchase => _mapper.Map<PurchaseDto>(purchase)).ToListAsync();
        }
        #endregion
        #region api/Purchases/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseDto>> GetPurchase(string id)
        {
            if (_context.Purchases == null)
                return Problem("la base du donnes ou le table Achat n'exite pas.");

            var purchase = _mapper.Map<PurchaseDto>(await _context.Purchases
                .Include(p => p.PurchaseInfos)
                .FirstOrDefaultAsync(p => p.Id == id));

            if (purchase == null)
                return NotFound();

            return Ok(purchase);
        }
        #endregion

        #endregion

        #region POST Methodes

        #region api/Purchases
        [HttpPost]
        public async Task<ActionResult<PurchaseDto>> PostPurchase(PurchaseDto purchase)
        {
            
            if (_context.Purchases == null)
                return Problem("la base du donnes ou le table Achat n'exite pas.");

            if (PurchaseExists(purchase.Id))
                return Conflict(new { title = "Impossible de Creer!", detail = "Cette Achat deja Ajouter" });

            _context.Purchases.Add(_mapper.Map<Purchase>(purchase));

            foreach (var prod in purchase.PurchaseInfos) 
            {
                var product = await _context.InBoxes.FindAsync(prod.Product, prod.Box);
                #pragma warning disable 
                product.InStock += prod.Qtt;
                _context.Entry(product).State = EntityState.Modified;
                #pragma warning restore
            }

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException) { throw; }

            return Ok(purchase);
        }
        #endregion

        #endregion

        private bool PurchaseExists(string id)
        {
            return (_context.Purchases?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
