using FragenGerangel.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Utils.API
{
    public class APIExceptionManager
    {
        private static Type[] ExceptionTypes = new Type[]
        {
            typeof(ServerException),
            typeof(MissingParametersException),
            typeof(WrongCredentialsException),
            typeof(WrongFormatException),
            typeof(AuthTokenExpiredException),
            typeof(InsufficientPermissionsException),
            typeof(InvalidHigherIDException),
            typeof(ObjectDoesntExistException),
            typeof(IllegalOperationException)
        };

        /// <summary>
        /// Gibt von einem Error-Code eine Exception zurück.
        /// </summary>
        public static APIException FromID(int id)
        {
            foreach (Type t in ExceptionTypes)
                if ((int)t.GetField("CODE").GetRawConstantValue() == id)
                    return (APIException)t.GetConstructor(new Type[] { }).Invoke(new object[] { });

            return null;
        }
    }
}
