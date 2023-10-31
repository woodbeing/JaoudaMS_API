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
using Microsoft.AspNetCore.Http.HttpResults;

namespace JaoudaMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly JaoudaSmContext _context;
        private readonly IMapper _mapper;

        public EmployeesController(JaoudaSmContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            return await _context.Employees.Select(employee => _mapper.Map<EmployeeDto>(employee)).ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{cin}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(string cin)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }

            var employeepure = await _context.Employees.FindAsync(cin);

            if (employeepure == null)
            {
                return NotFound();
            }

            var employee = _mapper.Map<EmployeeDto>(employeepure);

            return Ok(employee);
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{cin}")]
        public async Task<IActionResult> PutEmployee(string cin, EmployeeDto employee)
        {
            if (!EmployeeExists(cin)) return NotFound();
                
            _context.Entry(_mapper.Map<Employee>(employee)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> PostEmployee(EmployeeDto employee)
        {
            if (_context.Employees == null)
            {
                return Problem("la base du donnes ou le table empolyee n'exite pas");
            }

            _context.Employees.Add(_mapper.Map<Employee>(employee));

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EmployeeExists(employee.Cin))
                {
                    return Conflict(new {
                        statusCode = Conflict().StatusCode, Message = $"l'employee avec CIN : ({employee.Cin}) est deja ajouter"
                    });
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("PostEmployee", new { id = employee.Cin }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{Cin}")]
        public async Task<IActionResult> DeleteEmployee(string Cin)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(Cin);

            if (employee == null)
            {
                return NotFound();
            }

            employee.Salary = 0;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(string id)
        {
            return (_context.Employees?.Any(e => e.Cin == id)).GetValueOrDefault();
        }
    }
}
