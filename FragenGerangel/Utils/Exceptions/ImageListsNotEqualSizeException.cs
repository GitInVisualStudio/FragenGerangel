using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Utils.Exceptions
{
    public class ImageListsNotEqualSizeException : Exception
    {
        public ImageListsNotEqualSizeException() : base("The two Lists of Images were not of the same size.")
        {
        }
    }
}
