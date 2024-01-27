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
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
                return NotFound();

            return Ok(await _context.Employees
                    .OrderBy(employee => employee.Type)
                    .Select(employee => _mapper.Map<EmployeeDto>(employee))
                    .ToListAsync());
        }

        // GET: api/Employees/5
        [HttpGet("{cin}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(string cin)
        {
            if (_context.Employees == null)
                return NotFound();

            var employee = await _context.Employees
                .Include(employee => employee.Payments
                    .OrderBy(payment => payment.Year)
                    .ThenBy(payment => payment.Month))
                .Where(employee => employee.Cin == cin)
                .Select(employee => _mapper.Map<EmployeeDto>(employee))
                .SingleAsync();

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> PostEmployee(EmployeeDto employee)
        {
            if (_context.Employees == null)
                return NotFound();

            if (EmployeeExists(employee.Cin))
                return Conflict( new { detail = $"l'employee avec CIN : ({employee.Cin}) est deja ajouter" } );

            try
            {
                _context.Employees.Add(_mapper.Map<Employee>(employee));
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException){ throw; }

            return Ok(employee);
        }

        [HttpPost("Pay/{cin}/{month}/{year}")]
        public async Task<ActionResult<PaymentDto>> PostPayment(string cin, byte month, short year)
        {
            if (_context.Payments == null)
                return NotFound();

            if (PaymentExist(cin, month, year))
                return Conflict(new { detail = $"l'employee avec CIN : ({cin}) est deja Payer" });
              
            var employee = await _context.Employees.FindAsync(cin);

            if (employee == null)
                return NotFound();

            PaymentDto payment = new PaymentDto()
            {
                Employee = cin,
                Month = month,
                Year = year,
                Date = DateTime.Now,
                Salary = employee.Salary,
                Commission = employee.Commission,
            };

            employee.Commission = 0;

            try
            {
                _context.Payments.Add(_mapper.Map<Payment>(payment));
                _context.Entry(employee).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException) { throw; }

            return Ok(payment);
        }

        // PUT: api/Employees/5
        [HttpPut("{cin}")]
        public async Task<IActionResult> PutEmployee(string cin, EmployeeDto employee)
        {
            if (cin != employee.Cin)
                return NotFound();

            if (!EmployeeExists(cin))
                return NotFound();

            try
            {
                _context.Entry(_mapper.Map<Employee>(employee)).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException){ throw; }

            return Ok(employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{cin}")]
        public async Task<ActionResult<EmployeeDto>> DeleteEmployee(string cin)
        {
            if (_context.Employees == null)
                return NotFound();

            var employee = await _context.Employees.FindAsync(cin);

            if (employee == null)
                return NotFound();

            employee.Salary = 0;
            employee.Commission = 0;

            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<EmployeeDto>(employee));
        }

        private bool EmployeeExists(string cin)
        {
            return (_context.Employees?
                .Any(e => e.Cin == cin))
                .GetValueOrDefault();
        }

        private bool PaymentExist(string cin, byte month, short year)
        {
            return (_context.Payments?
                .Any(e => e.Employee == cin && e.Month == month && e.Year == year))
                .GetValueOrDefault();
        }
    }
}
