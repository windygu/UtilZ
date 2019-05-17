using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.Monitor
{
    public interface IAppMonitor
    { 
        bool IsMonitor { get; set; }

        string AppName { get; set; }

        string Arguments { get; set; }

        string AppProcessFilePath { get; set; }

        string AppExeFilePath { get; set; }
    }
}
