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
    public class ProductsController : ControllerBase
    {
        private readonly JaoudaSmContext _context;
        private readonly IMapper _mapper;

        public ProductsController(JaoudaSmContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            if (_context.Products == null)
                return NotFound();

            return Ok(await _context.Products
                .OrderBy(product => product.Genre)
                .Select(product => _mapper.Map<ProductDto>(product))
                .ToListAsync());
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(string id)
        {
            if (_context.Products == null)
                return NotFound();

            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            return Ok(_mapper.Map<ProductDto>(product));
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> PostProduct(ProductDto product)
        {
            if (_context.Products == null)
                return NotFound();

            if (ProductExists(product.Id))
                return Conflict(new { detail = "Cette Produit deja Existe" } );

            try
            {
                _context.Products.Add(_mapper.Map<Product>(product));
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException) { throw; }

            return Ok(product);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> PutProduct(string id, ProductDto product)
        {
            if (_context.Products == null)
                return NotFound();

            if (id != product.Id)
                return NotFound();

            if (!ProductExists(id))
                return NotFound();

            try
            {
                _context.Entry(_mapper.Map<Product>(product)).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException) { throw; }

            return Ok(product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductDto>> DeleteProduct(string id)
        {
            if (_context.Products == null)
                return NotFound();

            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            try
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException) { Conflict(new { detail = "Assurez-vous de supprimer un produit ajouté par accident." }); }

            return Ok(_mapper.Map<ProductDto>(product));
        }

        private bool ProductExists(string id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
