﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FixitTicket.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FixitTicket.Models;

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

        // PUT: api/Tickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return BadRequest();
            }

            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
        {
            var resident = await _context.User.FindAsync(ticket.Id);
            if (resident == null)
            {
                // for testing purposes
                _context.User.Add(new User() { Id = ticket.Id, Name = "Name", Email = "g_bick@u.pacific.edu", UserRole = 1});
                //return BadRequest("User ID must belong to an existing user");
            }
            await ValidateTicket(ticket);
            //TODO serialize errors to JSON
            ticket.CreationDate = DateTime.Now;
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

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.Id == id);
        }

        private async Task<bool> IsValidResident(int residentId) 
        {
            return await _context.Resident.FindAsync(residentId) != null;
        }

        private bool IsValidCategory(RepairCategory repairCategory) 
        {
            return repairCategory != RepairCategory.None;
        }

        private bool IsValidStatus(RepairStatus status) 
        {
            return status != RepairStatus.None;
        }

        private bool IsValidCreationDate(DateTime? creationDate) 
        {
            return creationDate == null;
        }

        private async Task<List<string>> ValidateTicket(Ticket ticket) 
        {
            List<string> ticketErrors = new List<string>();

            if (!await IsValidResident(ticket.Id)) 
            {
                ticketErrors.Add(TicketValidationErrors.ResidentNotFoundError(ticket.Id));
            }

            if (!IsValidCategory(ticket.RepairCategory)) 
            {
                ticketErrors.Add(TicketValidationErrors.CategoryNotSetError());
            }

            if (!IsValidStatus(ticket.Status)) 
            {
                ticketErrors.Add(TicketValidationErrors.StatusNotSetError());
            }

            if (!IsValidCreationDate(ticket.CreationDate)) 
            {
                ticketErrors.Add(TicketValidationErrors.CreationDateSetError());
            }

            return ticketErrors;
        }
    }
}
