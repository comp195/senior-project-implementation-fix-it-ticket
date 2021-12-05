using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FixitTicket.Models
{
    public class TicketUpdateChangeMessages
    {
        public static string CategoryChange(RepairCategory oldCategory, RepairCategory newCategory)
        {
            return $"Repair category changed from {oldCategory} to {newCategory}.";
        }

        public static string StatusChange(RepairStatus? oldStatus, RepairStatus? newStatus)
        {
            return $"Repair status changed from {oldStatus} to {newStatus}.";
        }

        public static string AssignedIdChange(int? oldAssignedId, int? newAssignedId) // TODO change to name strings
        {
            return $"Assigned Id changed from {oldAssignedId} to {newAssignedId}.";
        }

        public static string DescriptionChange(string oldDescription, string newDescription)
        {
            return $"Ticket description changed from {oldDescription} to {newDescription}.";
        }
    }
}
