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
    public class PurchasesController : ControllerBase
    {
        private readonly JaoudaSmContext _context;
        private readonly IMapper _mapper;

        public PurchasesController(JaoudaSmContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Purchases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseDto>>> GetPurchases()
        {
          if (_context.Purchases == null)
              return NotFound();

            return Ok(await _context.Purchases
                .OrderByDescending(purchase => purchase.Date)
                .Select(purchase => _mapper.Map<PurchaseDto>(purchase))
                .ToListAsync());
        }

        // GET: api/Purchases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseDto>> GetPurchase(string id)
        {
            if (_context.Purchases == null)
                return NotFound();

            var purchase = await _context.Purchases
                .Include(purchase => purchase.PurchaseProducts)
                .Include(purchase => purchase.PurchaseBoxes)
                .Include(purchase => purchase.PurchaseWastes)
                .FirstAsync(purchase => purchase.Id == id);

            if (purchase == null)
                return NotFound();

            return Ok(_mapper.Map<PurchaseDto>(purchase));
        }

        // POST: api/Purchases
        [HttpPost]
        public async Task<ActionResult<PurchaseDto>> PostPurchase(PurchaseDto purchase)
        {
            if (_context.Purchases == null)
                return NotFound();

            if (PurchaseExists(purchase.Id))
                return Conflict(new { detail = "Cette Achat Deja Exist" });

            try
            {
                foreach(PurchaseBoxDto purchaseB in purchase.PurchaseBoxes) 
                {
                    var box = await _context.Boxes.FindAsync(purchaseB.Box);

                    #pragma warning disable
                    box.InStock += (short?)(purchaseB.QttIn);
                    box.Empty -= (short?)(purchaseB.QttSent);
                    #pragma warning restore

                    _context.Entry(box).State = EntityState.Modified;
                }

                foreach(PurchaseProductDto purchaseP in purchase.PurchaseProducts)
                {
                    var product = await _context.Products.FindAsync(purchaseP.Product);

                    #pragma warning disable
                    product.Stock += (int?)purchaseP.Qtt;
                    #pragma warning restore

                    _context.Entry(product).State = EntityState.Modified;
                }

                foreach (PurchaseWasteDto purchaseW in purchase.PurchaseWastes)
                {
                    var waste = await _context.Wastes.FindAsync(purchaseW.Product, purchaseW.Type);

                    #pragma warning disable
                    waste.Qtt -= (short?)(purchaseW.Qtt);
                    #pragma warning restore

                    _context.Entry(waste).State = EntityState.Modified;
                }

                _context.Purchases.Add(_mapper.Map<Purchase>(purchase));   
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException){ throw; }

            return Ok(purchase);
        }

        private bool PurchaseExists(string id)
        {
            return (_context.Purchases?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
