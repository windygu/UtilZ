using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Winform.Base
{
    /// <summary>
    /// 鼠标行为类形
    /// </summary>
    public enum MouseActionType
    {
        /// <summary>
        /// 鼠标按下
        /// </summary>
        MouseDown,

        /// <summary>
        /// 鼠标移动
        /// </summary>
        MouseMove,

        /// <summary>
        /// 鼠标弹起
        /// </summary>
        MouseUp,

        /// <summary>
        /// 移动鼠标滚轮
        /// </summary>
        MouseWheel,

        /// <summary>
        /// 鼠标指针进入控件
        /// </summary>
        MouseEnter,

        /// <summary>
        /// 鼠标指针离开控件
        /// </summary>
        MouseLeave,

        /// <summary>
        /// 鼠标指针停放在控件上
        /// </summary>
        MouseHover
    }
}
