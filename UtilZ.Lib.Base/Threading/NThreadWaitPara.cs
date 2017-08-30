using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UtilZ.Lib.Base.Threading
{
    /// <summary>
    /// 线程等待参数
    /// </summary>
    internal class NThreadWaitPara
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="token">线程取消通知</param>
        public NThreadWaitPara(CancellationToken token)
        {
            this.Token = token;
        }

        /// <summary>
        /// 线程取消通知
        /// </summary>
        public CancellationToken Token { get; private set; }
    }
}
