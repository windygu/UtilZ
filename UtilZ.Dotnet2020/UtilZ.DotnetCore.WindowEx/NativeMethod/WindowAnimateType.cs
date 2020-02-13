using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.DotnetCore.WindowEx.NativeMethod
{
    /// <summary>
    /// 动画类型定义类
    /// </summary>
    public class WindowAnimateType
    {
        /// <summary>
        /// 从左到右打开窗口
        /// </summary>
        public const int AW_HOR_POSITIVE = 0x00000001;

        /// <summary>
        /// 从右到左打开窗口
        /// </summary>
        public const int AW_HOR_NEGATIVE = 0x00000002;

        /// <summary>
        /// 从上到下打开窗口
        /// </summary>
        public const int AW_VER_POSITIVE = 0x00000004;

        /// <summary>
        /// 从下到上打开窗口
        /// </summary>
        public const int AW_VER_NEGATIVE = 0x00000008;

        /// <summary>
        /// 若使用了AW_HIDE标志，则使窗口向内重叠；若未使用AW_HIDE标志，则使窗口向外扩展
        /// </summary>
        public const int AW_CENTER = 0x00000010;

        /// <summary>
        /// 隐藏窗口，缺省则显示窗口
        /// </summary>
        public const int AW_HIDE = 0x00010000;

        /// <summary>
        /// 激活窗口。在使用了AW_HIDE标志后不要使用这个标志
        /// </summary>
        public const int AW_ACTIVATE = 0x00020000;

        /// <summary>
        /// 使用滑动类型。缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略
        /// </summary>
        public const int AW_SLIDE = 0x00040000;

        /// <summary>
        /// 使用淡出效果。只有当hWnd为顶层窗口的时候才可以使用此标志
        /// </summary>
        public const int AW_BLEND = 0x00080000;
    }
}
