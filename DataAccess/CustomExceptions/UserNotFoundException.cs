using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CustomExceptions
{
    /// <summary>
    /// Custom exceptions must end with "exception" and 
    /// implement the 3 constructors
    /// </summary>
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
        {

        }
        public UserNotFoundException(string message) : base(message)
        {

        }
        public UserNotFoundException(string message, Exception inner) : base(message, inner)
        {

        }

    }
}
