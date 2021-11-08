using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FixitTicket.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FixitTicket.Controllers
{
    [Route("api/Tickets")]
    [ApiController]
    public class OrganizeTicketsController : ControllerBase
    {
        private readonly TicketContext _context;

        public OrganizeTicketsController(TicketContext context)
        {
            _context = context;
        }

        // GET: api/Tickets/assigned/989271487
        // gets tickets assigned to an employee id

        [HttpGet("assigned/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsAssigned(int id)
        {
            return await _context.Ticket.Where(ticket => ticket.ResidentId == id).ToListAsync();
        }

        // GET: api/Tickets/assigned
        // sorts tickets by assigned employee

        [HttpGet("assigned")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsByAssigned()
        {
            return await _context.Ticket.OrderBy(ticket => ticket.AssignedId).ToListAsync();
        }


        // GET: api/Tickets/location/2
        // gets tickets for a particular building

        [HttpGet("location/{location}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsLocation(Building location)
        {
            var tickets = await _context.Ticket.ToListAsync();
            var ticketIds = tickets.Select(t => t.ResidentId);
            var users = await _context.User.Where(u => ticketIds.Contains(u.Id))
                                            .Where(u => u.Building == location)
                                            .ToListAsync();

            return new ActionResult<IEnumerable<Ticket>>(tickets.Where(t => users.Select(u => u.Id).Contains(t.ResidentId)));
        }

        // GET: api/Tickets/location
        // orders tickets by location
        [HttpGet("location")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Ticket>> GetTicketsByLocation()
        {
            IEnumerable<Ticket> tickets = from ticket in _context.Ticket
                                          join user in _context.User
                                          on ticket.ResidentId
                                          equals user.Id
                                          orderby user.Building
                                          select ticket;

            return new ActionResult<IEnumerable<Ticket>>(tickets);
        }


        // GET: api/Tickets/status
        // get tickets with a certain status

        [HttpGet("status/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsWithStatus(RepairStatus status)
        {
            return await _context.Ticket.Where(ticket => ticket.Status == status).ToListAsync();
        }

        // GET: api/Tickets/status
        // orders tickets by status

        [HttpGet("status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsStatus()
        {
            return await _context.Ticket.OrderBy(ticket => ticket.Status).ToListAsync();
        }

        // GET: api/Tickets/category
        // get tickets with a certain category

        [HttpGet("category/{category}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsWithCategory(RepairCategory category)
        {
            return await _context.Ticket.Where(ticket => ticket.RepairCategory == category).ToListAsync();
        }

        // GET: api/Tickets/category
        // orders tickets by category

        [HttpGet("category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsCategory()
        {
            return await _context.Ticket.OrderBy(ticket => ticket.RepairCategory).ToListAsync();
        }

        // GET: api/Tickets/date
        // orders tickets by creation date

        [HttpGet("date")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsCreationDate()
        {
            return await _context.Ticket.OrderBy(ticket => ticket.CreationDate).ToListAsync();
        }
    }
}
