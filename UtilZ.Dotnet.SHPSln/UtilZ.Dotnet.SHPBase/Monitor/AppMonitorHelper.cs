using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.Monitor
{
    public class AppMonitorHelper
    {
        /// <summary>
        /// 将item1中值设置到item2中
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        public static void SetValueTo(IAppMonitor item1, IAppMonitor item2)
        {
            item2.IsMonitor = item1.IsMonitor;
            item2.AppName = item1.AppName;
            item2.Arguments = item1.Arguments;
            item2.AppProcessFilePath = item1.AppProcessFilePath;
            item2.AppExeFilePath = item1.AppExeFilePath;
        }

        public static bool Equals(IAppMonitor item1, IAppMonitor item2)
        {
            if (item1 == item2)
            {
                return true;
            }

            if (item1 == null || item2 == null)
            {
                return false;
            }

            return string.Equals(item1.AppExeFilePath, item2.AppExeFilePath, StringComparison.OrdinalIgnoreCase);
        }
    }
}
