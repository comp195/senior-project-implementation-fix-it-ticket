using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Principal;

namespace FixitTicket.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public Building? Building { get; set; }
        public int? RoomNumber { get; set; }
        public UserRole UserRole { get; set; }
        public string PasswordHash { get; set; }
    }
}
