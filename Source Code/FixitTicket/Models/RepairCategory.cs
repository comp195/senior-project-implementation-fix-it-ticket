using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace FixitTicket.Models
{
    public enum RepairCategory
    {
        None,
        Plumbing,
        Electrical,
        Flooring,
        Ceiling,
        Paint,
        Carpentry,
        Replacement,
        Other
    }
}
