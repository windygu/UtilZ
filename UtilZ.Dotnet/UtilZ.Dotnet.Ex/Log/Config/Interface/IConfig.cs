using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Log.Model;

namespace UtilZ.Dotnet.Ex.Log.Config.Interface
{
    /// <summary>
    /// 日志配置接口
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// 日志记录器名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 日志记录器是否启用
        /// </summary>
        bool Enable { get; set; }

        /// <summary>
        /// 日志布局
        /// </summary>
        string Layout { get; set; }

        /// <summary>
        /// 时间格式
        /// </summary>
        string DateFormat { get; set; }

        /// <summary>
        /// 是否记录异常堆栈信息,true:记录,false不记录[默认为true]
        /// </summary>
        bool IsRecordExceptionStackInfo { get; set; }

        /// <summary>
        /// 是否记录线程信息,true:记录,false不记录[默认为true]
        /// </summary>
        bool IsRecordThreadInfo { get; set; }

        /// <summary>
        /// 过滤日志级别起始值
        /// </summary>
        LogLevel FilterFrom { get; set; }

        /// <summary>
        /// 过滤日志级别结束值
        /// </summary>
        LogLevel FilterTo { get; set; }

        /// <summary>
        /// 过滤日志名称
        /// </summary>
        string FilterName { get; set; }

        /// <summary>
        /// 扩展日志记录器类型
        /// </summary>
        string ExtendLogRecorderType { get; set; }

        /// <summary>
        /// 日志追加器器类型
        /// </summary>
        string LogAppenderType { get; set; }

        /// <summary>
        /// 分隔线长度
        /// </summary>
        int SeparatorCount { get; set; }

        /// <summary>
        /// 获取分隔线
        /// </summary>
        string SeparatorLine { get; }

        /// <summary>
        /// 判断两个配置项值是否相同[相同返回true;否则返回false]
        /// </summary>
        /// <param name="config">目标项</param>
        /// <returns>相同返回true;否则返回false</returns>
        bool Equals(IConfig config);
    }
}
