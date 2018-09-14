using System;
using System.Collections.Generic;
using System.Text;
using UtilZ.Dotnet.SEx.Log.Config;

namespace UtilZ.Dotnet.SEx.Log.Appender
{
    internal class FileAppenderFixFileNameBuilder : IFileAppenderPathBuilder
    {
        private readonly char[] _pathSplitChars;
        private readonly FileAppenderConfig _config;
        private readonly string _rootDir;
        private readonly FileAppenderPathItem[] _pathItems;
        private bool _isFirstGetFilePath = true;

        public FileAppenderFixFileNameBuilder(FileAppenderConfig config, string[] paths, char[] pathSplitChars)
        {
            this._config = config;
            this._pathSplitChars = pathSplitChars;
            throw new NotImplementedException();
        }

        public string CreateLogFilePath()
        {
            throw new NotImplementedException();
        }
    }
}
