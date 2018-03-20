using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Winform.Base
{
    /// <summary>
    /// 鼠标操作命令事参数
    /// </summary>
    public class MouseActionArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">获取鼠标行为类型</param>
        /// <param name="mouseArgs">鼠标操作参数</param>
        public MouseActionArgs(MouseActionType type, System.Windows.Forms.MouseEventArgs mouseArgs)
        {
            this.Type = type;
            this.Args = null;
            this.MouseArgs = mouseArgs;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">获取鼠标行为类型</param>
        /// <param name="args">事件参数</param>
        public MouseActionArgs(MouseActionType type, EventArgs args)
        {
            this.Type = type;
            this.Args = args;
            this.MouseArgs = null;
        }

        /// <summary>
        /// 获取鼠标行为类型
        /// </summary>
        public MouseActionType Type { get; private set; }

        /// <summary>
        /// 事件参数
        /// </summary>
        public EventArgs Args { get; private set; }

        /// <summary>
        /// 鼠标事件参数
        /// </summary>
        public System.Windows.Forms.MouseEventArgs MouseArgs { get; private set; }
    }
}
