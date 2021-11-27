using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace FixitTicket.Models
{
    public enum RepairStatus
    {
        None,
        Open,
        [EnumMember(Value = "In Progress")]
        InProgress,
        New,
        Resolved,
        Closed
    }
}
