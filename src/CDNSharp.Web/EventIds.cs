using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDNSharp.Web
{
    public class EventIds
    {
        public static readonly EventId NoAvailableDestinations = new EventId(1, "NoAvailableDestinations");
    }
}
