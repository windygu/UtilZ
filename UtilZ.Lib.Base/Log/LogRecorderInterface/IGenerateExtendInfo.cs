using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.Log.LogRecorderInterface
{
    /// <summary>
    /// 自动生成日志扩展信息
    /// </summary>
    public interface IGenerateExtendInfo
    {
        /// <summary>
        /// 获取日志扩展信息
        /// </summary>
        /// <returns>日志扩展信息</returns>
        object GetExtendInfo();
    }
}
