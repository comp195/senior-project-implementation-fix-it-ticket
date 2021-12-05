using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FixitTicket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace FixitTicket.Controllers
{
    [Route("api/Tickets")]
    [ApiController]
    [Authorize]
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
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<IActionResult> PutTicket(int id, Ticket ticket)
        {
            var currentUser = HttpContext.User;
            var userId = TicketsController.GetId(currentUser);

            var oldTicket = await _context.Ticket.FindAsync(id);
            Console.WriteLine(ticket.CreationDate);
            if (ticket.CreationDate == null)
            {
                ticket.CreationDate = oldTicket.CreationDate;
            }
            
            if (oldTicket == null)
            {
                return NotFound();
            }

            if (id != ticket.Id)
            {
                ModelState.AddModelError(id.ToString(), "Ticket id does not match id");
                return BadRequest(new { title = "Ticket id does not match id"});
            }

            if (TicketsController.IsResident(currentUser) && userId != ticket.ResidentId)
            {
                return Forbid();
            }

            if (!await ValidUpdate(oldTicket, ticket, TicketsController.IsResident(currentUser))) 
            {
                return BadRequest(new { title = "update not vlid"});
            }

            var description = AddDescription(oldTicket, ticket);

            _context.Entry(oldTicket).CurrentValues.SetValues(ticket);

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

            _context.TicketUpdate.Add(new TicketUpdate { TicketId = id, UpdaterId = userId, Description = description, CreationDate = DateTime.Now });

            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.Id == id);
        }

        private static string AddDescription(Ticket oldTicket, Ticket newTicket) 
        {
            List<string> changes = new();
            if (oldTicket.RepairCategory != newTicket.RepairCategory) 
            {
                changes.Add(TicketUpdateChangeMessages.CategoryChange(oldTicket.RepairCategory, newTicket.RepairCategory));
            }

            if (oldTicket.Status != newTicket.Status)
            {
                changes.Add(TicketUpdateChangeMessages.StatusChange(oldTicket.Status, newTicket.Status));
            }

            if (oldTicket.AssignedId != newTicket.AssignedId)
            {
                changes.Add(TicketUpdateChangeMessages.AssignedIdChange(oldTicket.AssignedId, newTicket.AssignedId));
            }

            if (oldTicket.Description != newTicket.Description)
            {
                changes.Add(TicketUpdateChangeMessages.DescriptionChange(oldTicket.Description, newTicket.Description));
            }

            string changeString = "Changes made to ticket: \n";
            foreach (var change in changes) 
            {
                changeString += change + "\n";
            }
            return changeString;
        }

        private async Task<bool> ValidUpdate(Ticket oldTicket, Ticket newTicket, bool isResident) 
        {
            if (newTicket.Id != oldTicket.Id) 
            {
                return false;
            }

            if (newTicket.ResidentId != oldTicket.ResidentId) 
            {
                return false;
            }

            if (newTicket.CreationDate != oldTicket.CreationDate) 
            {
                return false;
            }

            if (newTicket.Status == RepairStatus.None) 
            {
                return false;
            }

            if (newTicket.Status != oldTicket.Status && isResident)
            {
                return false;
            }

            if (newTicket.RepairCategory == RepairCategory.None) 
            {
                return false;
            }
            
            if (newTicket.AssignedId != null && await _context.User.FindAsync(newTicket.AssignedId) == null) 
            {
                return false;
            }

            if (oldTicket.AssignedId != newTicket.AssignedId && isResident)
            {
                return false;
            }


            return true;
        }
    }
}
