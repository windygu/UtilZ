using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Winform.OriginalDataShow
{
    /// <summary>
    /// 原始数据展示接口
    /// </summary>
    public interface IOriginalDataShow
    {
        /// <summary>
        /// 默认原始数据显示类型
        /// </summary>
        OriginalDataShowType DefaultOriginalDataShowType { get; set; }

        /// <summary>
        /// 获取或设置排除或忽略要展示的原始数据项集合
        /// </summary>
        OriginalDataShowTypeCollection IgnoreDataShowTypes { get; set; }

        /// <summary>
        /// 获取当前数据加载类型[true:二进制;false:文件]
        /// </summary>
        bool DataLoadType { get; }

        /// <summary>
        /// 获取或设置数据文件路径
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// 获取或设置数据
        /// </summary>
        byte[] Data { get; set; }

        /// <summary>
        /// 清除当前显示
        /// </summary>
        void Clear();
    }
}
