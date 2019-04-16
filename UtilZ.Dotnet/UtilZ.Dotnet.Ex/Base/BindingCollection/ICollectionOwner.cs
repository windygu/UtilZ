using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.Base
{
    public interface ICollectionOwner
    {
        object Invoke(Delegate method, params object[] args);

        bool InvokeRequired { get; }

        bool IsDisposed { get; }
    }
}
