using System;
using System.Collections.Generic;
using System.Text;

namespace FFPT_Project.Service.Exceptions
{
    public class RoutingServiceException : Exception
    {
        public RoutingServiceException()
        {
        }

        public RoutingServiceException(string message) : base(message)
        {
        }

        public RoutingServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
