using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FixitTicket.Models
{
    public class TicketValidationErrors
    {
        public static string ResidentNotFoundError(int id) 
        {
            return $"Resident {id} does not exist. Enter the id of an existing resident.";
        }

        public static string CategoryNotSetError() 
        {
            return "The ticket repair category was not set to a valid value. Enter a valid repair category.";
        }

        public static string StatusNotSetError() 
        {
            return "The ticket status was not set to a valid value. Enter a valid ticket status.";
        }

        public static string CreationDateSetError() 
        {
            return "The ticket creation date may not be set in the body of the request. Remove the ticket creation date.";
        }


    }
}
