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
    public class ProductsController : ControllerBase
    {
        private readonly JaoudaSmContext _context;
        private readonly IMapper _mapper;

        public ProductsController(JaoudaSmContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region GET Methodes

        #region api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            if (_context.Products == null)
                return Problem("la base du donnes ou le table Produit n'exite pas.");

            return await _context.Products.Select(prod => _mapper.Map<ProductDto>(prod)).ToListAsync();
        }
        #endregion
        #region api/Products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(string id)
        {
            if (_context.Products == null)
                return Problem("La base du donnees ou La Table Produit n'existe pas.");

            var product = _mapper.Map<ProductDto>(await _context.Products.FindAsync(id));

            if (product == null)
                return NotFound();

            return Ok(product);
        }
        #endregion

        #endregion

        #region POST Methodes

        #region api/Products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> PostProduct(ProductDto product)
        {
            if (_context.Products == null)
                return Problem("Entity set 'JaoudaSmContext.Products'  is null.");

            if (ProductExists(product.Id))
                return Conflict(new { title = "Impossible d'Ajouter!", detail = "Cette Produit deja Existe" });

            _context.Products.Add(_mapper.Map<Product>(product));

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException) { throw; }

            return Ok(product);
        }
        #endregion

        #endregion

        #region PUT Methodes

        #region api/Products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(string id, ProductDto product)
        {
            if (_context.Products == null)
                return Problem("la base du donnes ou le table Produit n'exite pas.");

            if (id != product.Id)
                return Conflict(new { title = "Impossible de Modifier!", detail = "Impossible Changer ID d'un produit" });

            if (!ProductExists(id))
                return NotFound();

            _context.Entry(_mapper.Map<Product>(product)).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }

            return Ok(product);
        }
        #endregion

        #endregion

        #region DELETE Methodes

        #region api/Products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            if (_context.Products == null)
                return Problem("la base du donnes ou le table Produit n'exite pas.");

            var product = _mapper.Map<ProductDto>(await _context.Products.FindAsync(id));

            if (product == null)
                return NotFound();

            try
            {
                _context.Products.Remove(_mapper.Map<Product>(product));
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) { Conflict(new { title = "Impossible de Supprimer!", detail = "assurez-vous de supprimer un produit ajouté par accident." }); }

            return Ok(product);
        }
        #endregion

        #endregion

        private bool ProductExists(string id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
