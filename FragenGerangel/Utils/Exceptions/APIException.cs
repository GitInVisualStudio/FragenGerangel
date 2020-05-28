using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Utils.Exceptions
{
    public abstract class APIException : Exception
    {
        private int id;

        public int Id { get => id; set => id = value; }

        public APIException(string message, int id) : base(message)
        {
            this.id = id;
        }
    }
}
