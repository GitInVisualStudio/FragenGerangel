using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Utils.Exceptions
{
    public class WrongCredentialsException : APIException
    {
        public const int CODE = 2;

        public WrongCredentialsException() : base("The entered credentials were wrong.", CODE)
        {

        }
    }
}
