using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UtilZ.Lib.Base
{
    /// <summary>
    /// 基础辅助类
    /// </summary>
    public class NBaeHepler
    {
        /// <summary>
        /// 获取永久时间片
        /// </summary>
        /// <returns>永久时间片</returns>
        public static TimeSpan GetForeverTimeSpan()
        {
            //将1000年当作为永久时间
            return new TimeSpan(365 * 1000, 0, 0, 0, 0);
        }

        /// <summary>
        /// 设置线程是否为后台线程
        /// </summary>
        /// <param name="thread">要设置的线程</param>
        /// <param name="isBackground">true:后台线程;false:前台线程</param>
        public static void SetThreadIsBackground(Thread thread, bool isBackground)
        {
            try
            {
                if (thread == null)
                {
                    return;
                }

                thread.IsBackground = isBackground;
            }
            catch
            { }
        }
    }
}
