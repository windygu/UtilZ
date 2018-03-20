using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Winform.Base
{
    /// <summary>
    /// UI动作行为接口
    /// </summary>
    public interface IUIAction
    {
        /// <summary>
        /// 鼠标行为动作事件
        /// </summary>
        event EventHandler<MouseActionArgs> MouseAction;
    }
}
