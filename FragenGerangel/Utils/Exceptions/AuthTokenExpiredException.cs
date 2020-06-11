using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Utils.Exceptions
{
    public class AuthTokenExpiredException : APIException
    {
        public const int CODE = 4;

        public AuthTokenExpiredException() : base("The used authentification token has expired. Please request a new one.", CODE)
        {

        }
    }
}
