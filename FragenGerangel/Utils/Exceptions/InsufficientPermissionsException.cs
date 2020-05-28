using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Utils.Exceptions
{
    public class InsufficientPermissionsException : APIException
    {
        public const int CODE = 5;

        public InsufficientPermissionsException() : base("You do not have permission to perform this action.", CODE)
        {

        }
    }
}
