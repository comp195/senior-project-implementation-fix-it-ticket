using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FixitTicket.Models
{
    public class RegisterModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public Building? Building { get; set; }
        public int? RoomNumber { get; set; }
        public UserRole UserRole { get; set; }
        [MinLength(6)]
        public string Password { get; set; }
    }
}