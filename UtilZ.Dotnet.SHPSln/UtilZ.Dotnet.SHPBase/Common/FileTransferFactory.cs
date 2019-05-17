using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UtilZ.Dotnet.Ex.FileTransfer;

namespace UtilZ.Dotnet.SHPBase.Common
{
    public class FileTransferFactory
    {
        public static IFileTransfer Create(string baseUrl, string userName = null, string password = null)
        {
            //, IWebProxy proxy = null
            return new FtpFileTransfer(baseUrl, userName, password, null);
        }
    }
}
