using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Utils.Exceptions
{
    public class ServerException : APIException
    {
        public const int CODE = 0;
        public ServerException() : base("Something went wrong server-side. Please try again.", CODE)
        {
        }
    }
}
