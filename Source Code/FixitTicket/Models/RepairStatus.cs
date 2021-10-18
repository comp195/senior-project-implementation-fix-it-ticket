using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FixitTicket.Models
{
    public enum RepairStatus
    {
        None,
        Open,
        InProgress,
        New,
        Resolved,
        Closed
    }
}
