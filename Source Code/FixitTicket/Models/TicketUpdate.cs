using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FixitTicket.Models
{
    public class TicketUpdate
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int UpdaterId { get; set; }
        public string Description { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
