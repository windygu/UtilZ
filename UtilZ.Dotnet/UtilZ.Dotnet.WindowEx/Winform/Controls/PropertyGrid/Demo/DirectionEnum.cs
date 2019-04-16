
using UtilZ.Dotnet.Ex.Attributes;

namespace UtilZ.Dotnet.WindowEx.Winform.Controls.PropertyGrid.Demo
{
    /// <summary>
    /// PropertyGrid枚举
    /// </summary>
    public enum DirectionEnum
    {
        /// <summary>
        /// 左
        /// </summary>
        [DisplayNameExAttribute("左")]
        Left,

        /// <summary>
        /// 右
        /// </summary>
        [DisplayNameExAttribute("右")]
        Right,

        /// <summary>
        /// 上
        /// </summary>
        [DisplayNameExAttribute("上")]
        Up,

        /// <summary>
        /// 下
        /// </summary>
        [DisplayNameExAttribute("下")]
        Down
    }
}
