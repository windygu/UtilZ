using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SHPBase.Base;

namespace UtilZ.Dotnet.SHPBase.Exceptions
{
    public class SHPOperationException : Exception
    {
        public SHPCommandExcuteResult ExcuteResult { get; private set; }

        public SHPOperationException(SHPCommandExcuteResult excuteResult, string message)
            : base(message)
        {
            this.ExcuteResult = excuteResult;
        }

        public SHPOperationException(SHPResult shpResult)
            : this(shpResult.Result, shpResult.ToString())
        {

        }
    }
}
