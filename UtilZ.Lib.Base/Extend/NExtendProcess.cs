using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace UtilZ.Lib.Base.Extend
{
    /// <summary>
    /// 进程扩展方法
    /// </summary>
    public class NExtendProcess
    {
        /// <summary>
        /// 进程互斥对象
        /// </summary>
        private static Mutex _processMutex = null;

        /// <summary>
        /// 多线程锁
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// 单进程检测[创建进程互斥对象成功返回true;否则返回false]
        /// </summary>
        /// <param name="rangeFlag">单进程检测范围[true:所有用户;false:仅当前用户]</param>
        /// <returns>创建进程互斥对象成功返回true;否则返回false</returns>
        public static bool SingleProcessCheck(bool rangeFlag)
        {
            lock (_lock)
            {
                if (_processMutex != null)
                {
                    return true;
                }

                string mutexName = Assembly.GetEntryAssembly().GetCustomAttributes(true).Where(t => t is GuidAttribute).Cast<GuidAttribute>().Select(t => t.Value).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(mutexName))
                {
                    mutexName = AppDomain.CurrentDomain.SetupInformation.ApplicationName;
                }

                if (rangeFlag)
                {
                    mutexName = @"Global\" + mutexName;
                }

                bool  createNew;
                try
                {
                    _processMutex = new Mutex(true, mutexName, out createNew);
                }
                catch
                {
                    createNew = false;
                }

                return createNew;
            }
        }
    }
}
