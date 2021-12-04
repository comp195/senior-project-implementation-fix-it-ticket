using System;

namespace FixitTicket.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int ResidentId { get; set; }
        public RepairCategory RepairCategory { get; set; }
        public RepairStatus Status { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? AssignedId { get; set; }
        public string Description { get; set; }

    }
}
