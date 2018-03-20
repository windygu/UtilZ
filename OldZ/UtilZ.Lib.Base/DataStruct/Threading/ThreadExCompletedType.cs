using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.DataStruct.Threading
{
    /// <summary>
    /// 线程执行完成类型
    /// </summary>
    public enum ThreadExCompletedType
    {
        /// <summary>
        /// 完成
        /// </summary>
        Completed,

        /// <summary>
        /// 异常
        /// </summary>
        Exception,

        /// <summary>
        /// 取消
        /// </summary>
        Cancel
    }
}
