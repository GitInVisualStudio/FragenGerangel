using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Utils.Exceptions
{
    public class InvalidHigherIDException : APIException
    {
        public const int CODE = 6;

        public InvalidHigherIDException() : base("The given higher online-ID of object was invalid. Please try uploading the semester/course first.", CODE)
        {
        }
    }
}
