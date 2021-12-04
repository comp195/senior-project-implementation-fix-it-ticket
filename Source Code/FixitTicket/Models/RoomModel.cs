using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FixitTicket.Models
{
    public class RoomModel
    {
        public int? RoomNumber { get; set; }
        public Building Building { get; set; }
    }
}
