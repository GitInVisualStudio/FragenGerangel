using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Utils.Exceptions
{
    public class MissingParametersException : APIException
    {
        public const int CODE = 1;
        public MissingParametersException() : base("There were one or more parameters missing from your call.", CODE)
        {

        }
    }
}
