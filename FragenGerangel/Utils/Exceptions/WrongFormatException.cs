using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Utils.Exceptions
{
    public class WrongFormatException : APIException
    {
        public const int CODE = 3;

        public WrongFormatException() : base("A parameter you passed was in the wrong format. Please refer to the API documentation.", CODE)
        {

        }
    }
}
