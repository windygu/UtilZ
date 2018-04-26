using UtilZ.Dotnet.Ex.Model;
using UtilZ.Dotnet.Ex.Model.Attributes;

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
        [DisplayNameExAttribute(DisplayName = "左")]
        Left,

        /// <summary>
        /// 右
        /// </summary>
        [DisplayNameExAttribute(DisplayName = "右")]
        Right,

        /// <summary>
        /// 上
        /// </summary>
        [DisplayNameExAttribute(DisplayName = "上")]
        Up,

        /// <summary>
        /// 下
        /// </summary>
        [DisplayNameExAttribute(DisplayName = "下")]
        Down
    }
}
