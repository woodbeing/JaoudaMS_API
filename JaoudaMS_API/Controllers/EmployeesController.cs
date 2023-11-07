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

        #region GET Methodes

        #region api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            if (_context.Employees == null)
                return Problem("la base du donnes ou le table Employee n'exite pas.");

            return Ok(await _context.Employees.Select(employee => _mapper.Map<EmployeeDto>(employee)).ToListAsync());
        }
        #endregion
        #region api/Employees/{cin}
        [HttpGet("{cin}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(string cin)
        {
            if (_context.Employees == null)
                return Problem("la base du donnes ou le table Employee n'exite pas.");

            var employee = _mapper.Map<EmployeeDto>(await _context.Employees.FindAsync(cin));

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }
        #endregion
        #region api/Employees/{cin}/Payments
        [HttpGet("{cin}/Payments")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPayments(string cin)
        {
            if (_context.Payments == null)
                return Problem("la base du donnes ou le table Payments n'exite pas.");

            var Payments = _mapper.Map<ICollection<PaymentDto>>(await _context.Payments.Where(pay => pay.Employee == cin).ToListAsync());

            if (Payments == null)
                return NotFound();

            return Ok(Payments);
        }
        #endregion

        #endregion

        #region POST Methodes

        #region api/Employees
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> PostEmployee(EmployeeDto employee)
        {
            if (_context.Employees == null)
                return Problem("la base du donnes ou le table Empolyee n'exite pas");

            if (EmployeeExists(employee.Cin))
                return Problem($"l'employee avec CIN : ({employee.Cin}) est deja ajouter");

            _context.Employees.Add(_mapper.Map<Employee>(employee));

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException) { throw; }

            return Ok(employee);
        }
        #endregion
        #region api/Employees/{cin}/Payments/Make/{month}/{year}
        [HttpPost("{cin}/Payments/Make/{month}/{year}")]
        public async Task<ActionResult<PaymentDto>> PostPayment(string cin, byte month, short year)
        {
            if (_context.Payments == null)
                return Problem("la base du donnes ou le table Payment n'exite pas");

            var employee = await _context.Employees.FindAsync(cin);

            if (employee == null)
                return NotFound();

            var payment = new PaymentDto()
            {
                Employee = employee.Cin,
                Date = DateTime.Now,
                Month = month,
                Year = year,
                Salary = employee.Salary,
                Commission = employee.Commission,
            };

            employee.Commission = 0;

            _context.Entry(_mapper.Map<Payment>(payment)).State = EntityState.Added;
            _context.Entry(_mapper.Map<Employee>(employee)).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException) { throw; }

            return Ok(payment);
        }
        #endregion

        #endregion

        #region PUT Methodes

        #region api/Employees/{cin}
        [HttpPut("{cin}")]
        public async Task<IActionResult> PutEmployee(string cin, EmployeeDto employee)
        {
            if (_context.Employees == null)
                return Problem("la base du donnes ou le table Empolyee n'exite pas");

            if (!EmployeeExists(cin))
                return NotFound();

            if (cin != employee.Cin)
                return Problem($"Impossible de modifier le CIN des employés");

            _context.Entry(_mapper.Map<Employee>(employee)).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }

            return Ok(employee);
        }
        #endregion

        #endregion

        #region DELETE Methodes

        #region api/Employees/{cin}
        [HttpDelete("{Cin}")]
        public async Task<IActionResult> DeleteEmployee(string Cin)
        {
            if (_context.Employees == null)
                return Problem("la base du donnes ou le table Empolyee n'exite pas");

            var employee = await _context.Employees.FindAsync(Cin);

            if (employee == null)
                return NotFound();

            employee.Salary = 0;
            await _context.SaveChangesAsync();

            return Ok(employee);
        }
        #endregion

        #endregion

        private bool EmployeeExists(string id)
        {
            return (_context.Employees?.Any(e => e.Cin == id)).GetValueOrDefault();
        }
    }
}
