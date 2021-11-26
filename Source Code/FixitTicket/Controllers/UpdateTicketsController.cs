using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FixitTicket.Models;
using Microsoft.EntityFrameworkCore;

namespace FixitTicket.Controllers
{
    [Route("api/Tickets")]
    [ApiController]
    public class UpdateTicketsController : ControllerBase
    {
        private readonly TicketContext _context;

        public UpdateTicketsController(TicketContext context)
        {
            _context = context;
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

            _context.TicketUpdate.Add(new TicketUpdate { TicketId = id, UpdaterId = ticket.ResidentId, CreationDate = DateTime.Now });

            return NoContent();
        }


        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.Id == id);
        }
    }
}
