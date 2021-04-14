using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CustomExceptions
{
    public class InsufficientRightsException : Exception
    {
        public InsufficientRightsException()
        {
        }

        public InsufficientRightsException(string message) : base(message)
        {
        }

        public InsufficientRightsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
