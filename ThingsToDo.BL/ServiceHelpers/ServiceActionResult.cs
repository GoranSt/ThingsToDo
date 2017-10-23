using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThingsToDo.BL.ServiceHelpers
{
    public class ServiceActionResult<T> where T : class
    {
        public T Entity { get; private set; }
        public ServiceActionStatus Status { get; private set; }

        public Exception Exception { get; private set; }

        public string Message { get; set; }

        public ServiceActionResult(T entity, ServiceActionStatus status)
        {
            Entity = entity;
            Status = status;
        }

        public ServiceActionResult(T entity, ServiceActionStatus status, Exception exception)
            : this(entity, status)
        {
            Exception = exception;
        }

        public ServiceActionResult(T entity, ServiceActionStatus status, string message)
            : this(entity, status)
        {
            Message = message;
        }
    }

}
