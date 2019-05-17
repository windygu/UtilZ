using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.Exceptions
{
    public class SHPException : Exception
    {
        public SHPException(string message)
            : base(message)
        {

        }

        public SHPException()
           : base()
        {

        }
    }
}
