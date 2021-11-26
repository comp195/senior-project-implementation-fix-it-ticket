using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Principal;

namespace FixitTicket.Models
{
    public class User : IIdentity
    {
        public User(string authenticationType, bool isAuthenticated, string name) 
        {
            AuthenticationType = authenticationType;
            IsAuthenticated = isAuthenticated;
            Name = name;
        }
        public string AuthenticationType { get;  }

        public bool IsAuthenticated { get;  }
        public int Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public Building Building { get; set; }
        public int? RoomNumber { get; set; }
        public UserRole UserRole { get; set; }
    }
}
