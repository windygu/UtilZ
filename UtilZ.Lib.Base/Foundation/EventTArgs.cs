using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.Foundation
{
    /// <summary>
    /// 控件可见性事件参数
    /// </summary>
    public class EventTArgs<T> : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="para">参数</param>
        public EventTArgs(T para)
        {
            this.Para = para;
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        public T Para { get; private set; }
    }
}
