using static BCrypt.Net.BCrypt;
using FixitTicket.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Microsoft.AspNetCore.Authorization;

namespace FixitTicket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : Controller
    {
        private readonly TicketContext _context;
        private readonly Regex idRegex = new (@"^989[0-9]{6}$");
        private readonly Regex emailRegex = new(@"^.+@(u.)?pacific.edu$");
        private readonly Regex roomRegex = new(@"^[1-4][0-9]{2}$");

        public RegisterController(TicketContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterModel model) 
        {
            if (_context.User.Any(u => u.Email == model.Email)) 
            {
                return BadRequest();
            }

            if (_context.User.Any(u => u.Id == model.Id))
            {
                return BadRequest();
            }

            if (!idRegex.IsMatch(model.Id.ToString())) 
            {
                return BadRequest();
            }

            if (!emailRegex.IsMatch(model.Email)) 
            {
                return BadRequest();
            }

            if (!roomRegex.IsMatch(model.RoomNumber?.ToString())) // TODO make sure this works with null
            {
                return BadRequest();
            }

            var hash = HashPassword(model.Password);

            var user = new User 
            {   Id = model.Id,
                Name = model.FirstName + " " + model.LastName,
                Email = model.Email,
                Building = model.Building,
                RoomNumber = model.RoomNumber,
                UserRole = model.UserRole,
                PasswordHash = hash 
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, model);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status401Unauthorized)]
        [ProducesResponseType(Status403Forbidden)]
        public async Task<ActionResult<User>> GetUser(int id) 
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }


    }
}
