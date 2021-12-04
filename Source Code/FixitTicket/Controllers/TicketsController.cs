using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FixitTicket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace FixitTicket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly TicketContext _context;

        public TicketsController(TicketContext context)
        {
            _context = context;
        }

        // GET: api/Tickets
        [HttpGet]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status401Unauthorized)]
        [ProducesResponseType(Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            var currentUser = HttpContext.User;
            var userId = GetId(currentUser);
            if (IsResident(currentUser)) 
            {
                return await _context.Ticket.Where(t => t.ResidentId == userId).ToListAsync();
            }

            return await _context.Ticket.ToListAsync();
        }


        // GET: api/Tickets/5
        [HttpGet("{id}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status401Unauthorized)]
        [ProducesResponseType(Status403Forbidden)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var currentUser = HttpContext.User;
            var userId = GetId(currentUser);

            var ticket = await _context.Ticket.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            if (IsResident(currentUser) && userId != ticket.ResidentId) 
            {
                return Forbid();
            }

            return ticket;
        }


        // POST: api/Tickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status401Unauthorized)]
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
        {
            var currentUser = HttpContext.User;
            if (ticket.ResidentId == 0)
            {
                var id = GetId(currentUser);
                ticket.ResidentId = id;
            }
            var errors = await ValidateTicket(ticket);
            if (errors.Count != 0) 
            {
                return BadRequest(new { title = "One or more validation errors occurred.", status = 400, errors });
            }
            ticket.CreationDate = DateTime.Now;
            ticket.Status = RepairStatus.Open;
            _context.Ticket.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
        }

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status401Unauthorized)]
        [ProducesResponseType(Status403Forbidden)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var currentUser = HttpContext.User;
            if (IsResident(currentUser)) 
            {
                return Forbid();
            }
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Tickets/5/location
        [HttpGet("{id}/location")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status401Unauthorized)]
        [ProducesResponseType(Status403Forbidden)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult<RoomModel>> GetTicketLocation(int id) 
        {
            var response = await GetTicket(id);
            var currentUser = HttpContext.User;
            var userId = GetId(currentUser);

            if (!(response.Value is Ticket)) 
            {
                return response.Result;
            }
            var ticket = response.Value;
            var residentId = ticket.ResidentId;

            var user = await _context.User.FindAsync(residentId);

            if (user == null)
            {
                return NotFound();
            }

            if (IsResident(currentUser) && userId != user.Id)
            {
                return Forbid();
            }

            return new RoomModel { RoomNumber = user.RoomNumber, Building = user.Building };

        }


        private async Task<bool> IsValidUser(int residentId) 
        {
            return await _context.User.FindAsync(residentId) != null;
        }

        private static bool IsValidCategory(RepairCategory repairCategory) 
        {
            return repairCategory != RepairCategory.None;
        }

        private static bool IsValidStatus(RepairStatus? status) 
        {
            return status == null;
        }

        private static bool IsValidCreationDate(DateTime? creationDate) 
        {
            return creationDate == null;
        }

        private async Task<List<string>> ValidateTicket(Ticket ticket) 
        {
            List<string> ticketErrors = new();

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

        private static bool IsResident(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value == "Resident";
        }

        private static int GetId(ClaimsPrincipal user) 
        {
            return int.Parse(user.Claims.FirstOrDefault(c => c.Type == "Id").Value);
        }
    }


}
