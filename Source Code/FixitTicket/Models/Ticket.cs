using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FixitTicket.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int ResidentId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public RepairCategory RepairCategory { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public RepairStatus? Status { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? AssignedId { get; set; }
        public string Description { get; set; }

    }
}
