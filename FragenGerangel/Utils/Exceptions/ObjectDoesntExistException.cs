using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Utils.Exceptions
{
    public class ObjectDoesntExistException : APIException
    {
        public const int CODE = 7;

        public ObjectDoesntExistException() : base("The specified object couldn't be found.", CODE)
        {

        }
    }
}
