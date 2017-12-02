using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.DataStruct.Threading
{
    /// <summary>
    /// 线程执行完成事件参数
    /// </summary>
    public class ThreadExCompletedArgs : EventArgs
    {
        /// <summary>
        /// 线程执行完成类型
        /// </summary>
        public ThreadExCompletedType Type { get; private set; }

        /// <summary>
        /// 当执行异常可取消时可能的异常信息
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">线程执行完成类型</param>
        /// <param name="ex">当执行异常可取消时可能的异常信息</param>
        public ThreadExCompletedArgs(ThreadExCompletedType type, Exception ex)
        {
            this.Type = type;
            this.Exception = ex;
        }
    }
}
