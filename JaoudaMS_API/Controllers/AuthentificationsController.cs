using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JaoudaMS_API.Models;

namespace JaoudaMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificationsController : ControllerBase
    {
        private readonly JaoudaSmContext _context;

        public AuthentificationsController(JaoudaSmContext context)
        {
            _context = context;
        }

        // GET: api/Authentifications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Authentification>>> GetAuthentifications()
        {
            if (_context.Authentifications == null)
                return NotFound();

            return Ok(await _context.Authentifications.ToListAsync());
        }

        // GET: api/Authentifications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Authentification>> GetAuthentification(string id)
        {
            if (_context.Authentifications == null)
                return NotFound();

            var authentification = await _context.Authentifications.FindAsync(id);

            if (authentification == null)
                return NotFound();

            return authentification;
        }

        // PUT: api/Authentifications/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthentification(string id, Authentification authentification)
        {
            if (id != authentification.Login)
                return NotFound();

            if (!AuthentificationExists(id))
                return NotFound();

            _context.Entry(authentification).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException) { throw; }

            return Ok(authentification);
        }

        // POST: api/Authentifications
        [HttpPost]
        public async Task<ActionResult<Authentification>> PostAuthentification(Authentification authentification)
        {
            if (_context.Authentifications == null)
                return NotFound();

            if (AuthentificationExists(authentification.Login)) 
                return NotFound(); 

            _context.Authentifications.Add(authentification);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException) { throw; }

            return Ok(authentification);
        }

        // POST: api/Authentifications/5
        [HttpPost("{id}")]
        public async Task<ActionResult<Authentification>> PostAuthentification(string id, Authentification authentification)
        {
            if (_context.Authentifications == null)
                return NotFound();

            if (!AuthentificationExists(authentification.Login))
                return NotFound();

            if(id != authentification.Login)
                return NotFound();

            var account = await _context.Authentifications.FindAsync(id);

            if (account?.Password != authentification.Password)
                return Ok(false);

            return Ok(true);
        }

        // DELETE: api/Authentifications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthentification(string id)
        {
            if (_context.Authentifications == null)
                return NotFound();

            var authentification = await _context.Authentifications.FindAsync(id);

            if (authentification == null) 
                return NotFound(); 

            _context.Authentifications.Remove(authentification);
            await _context.SaveChangesAsync();

            return Ok(authentification);
        }

        private bool AuthentificationExists(string id)
        {
            return (_context.Authentifications?.Any(e => e.Login == id)).GetValueOrDefault();
        }
    }
}