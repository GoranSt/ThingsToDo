using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThingsToDo.BL.ServiceHelpers
{
    public enum ServiceActionStatus
    {
        Ok,
        Created,
        Updated,
        Finished,
        NotFound,
        Deleted,
        NothingModified,
        Error,
        ReturnedMessage,
        
    }
}
