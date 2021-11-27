using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FixitTicket.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FixitTicket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly TicketContext _context;

        public TicketsController(TicketContext context)
        {
            _context = context;
        }

        // GET: api/Tickets
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {

            return await _context.Ticket.ToListAsync();
        }


        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        }


        // POST: api/Tickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
        {
            var resident = await _context.User.FindAsync(ticket.ResidentId);
            //if (resident == null)
            //{
            //    // for testing purposes
            //    _context.User.Add(new User() { Id = ticket.ResidentId, Name = "Name", Email = "g_bick@u.pacific.edu", UserRole = UserRole.Resident});
            //    //return BadRequest("User ID must belong to an existing user");
            //}
            var errors = await ValidateTicket(ticket);
            if (errors.Count != 0) 
            {
                return BadRequest(new { title = "One or more validation errors occurred.", status = 400, errors = errors });
            }
            ticket.CreationDate = DateTime.Now;
            ticket.Status = RepairStatus.Open;
            _context.Ticket.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
        }

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> IsValidUser(int residentId) 
        {
            return await _context.User.FindAsync(residentId) != null;
        }

        private bool IsValidCategory(RepairCategory repairCategory) 
        {
            return repairCategory != RepairCategory.None;
        }

        private bool IsValidStatus(RepairStatus? status) 
        {
            return status == null;
        }

        private bool IsValidCreationDate(DateTime? creationDate) 
        {
            return creationDate == null;
        }

        private async Task<List<string>> ValidateTicket(Ticket ticket) 
        {
            List<string> ticketErrors = new List<string>();

            if (!await IsValidUser(ticket.ResidentId)) 
            {
                ticketErrors.Add(TicketValidationErrors.ResidentNotFoundError(ticket.ResidentId));
            }

            if (ticket.AssignedId != null) 
            {
                ticketErrors.Add(TicketValidationErrors.EmployeeSetError());
            }

            if (!IsValidCategory(ticket.RepairCategory)) 
            {
                ticketErrors.Add(TicketValidationErrors.CategoryNotSetError());
            }

            if (!IsValidStatus(ticket.Status)) 
            {
                ticketErrors.Add(TicketValidationErrors.StatusSetError());
            }

            if (!IsValidCreationDate(ticket.CreationDate)) 
            {
                ticketErrors.Add(TicketValidationErrors.CreationDateSetError());
            }

            return ticketErrors;
        }
    }
}
