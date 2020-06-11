using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Utils.Exceptions
{
    public class IllegalOperationException : APIException
    {
        public const int CODE = 8;

        public IllegalOperationException() : base("The requested operation was semantically illegal", 8)
        {

        }
    }
}
