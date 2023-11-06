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

            return await _context.Employees.Select(employee => _mapper.Map<EmployeeDto>(employee)).ToListAsync();
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

            if (Payments.Count() == 0)
                return Problem("Cet employé n'a jamais été payé");

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
            {
                return Conflict(new
                {
                    statusCode = Conflict().StatusCode,
                    Message = $"l'employee avec CIN : ({employee.Cin}) est deja ajouter"
                });
            }

            _context.Employees.Add(_mapper.Map<Employee>(employee));

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction("PostEmployee", new { id = employee.Cin }, employee);
        }
        #endregion
        #region api/Employees/{cin}/Payments/Make
        [HttpPost("{cin}/Payments/Make")]
        public async Task<ActionResult<PaymentDto>> PostPayment(string cin)
        {
            if (_context.Payments == null)
                return Problem("la base du donnes ou le table Payment n'exite pas");

            var employee = await _context.Employees.FindAsync(cin);

            if (employee == null)
                return NotFound();

            var payment = new PaymentDto();

            payment.Employee = employee.Cin;
            payment.Salary = employee.Salary;
            payment.Commission = employee.Commission;
            payment.Date = DateTime.Now;
            payment.Month = byte.Parse(DateTime.Now.Month.ToString());

            employee.Commission = 0;

            _context.Entry(_mapper.Map<Payment>(payment)).State = EntityState.Added;
            _context.Entry(_mapper.Map<Employee>(employee)).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction("PostPayment", new { id = payment.Employee }, payment);
        }
        #endregion

        #endregion

        #region PUT Methodes

        // PUT: api/Employees/5
        [HttpPut("{cin}")]
        public async Task<IActionResult> PutEmployee(string cin, EmployeeDto employee)
        {
            if (!EmployeeExists(cin))
                return NotFound();

            _context.Entry(_mapper.Map<Employee>(employee)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(new { statusCode = Ok().StatusCode, message = "les données de cet employé ont été mises à jour avec succès" });
        }

        #endregion
        
        #region DELETE Methodes

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

        #endregion

        private bool EmployeeExists(string id)
        {
            return (_context.Employees?.Any(e => e.Cin == id)).GetValueOrDefault();
        }
    }
}
