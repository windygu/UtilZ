using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.Dotnet.SEx.Log.Appender
{
    internal interface IFileAppenderPathBuilder
    {
        string CreateLogFilePath();
    }
}
