using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.NLog.Config.Interface;
using UtilZ.Lib.Base.NLog.Model;

namespace UtilZ.Lib.Base.NLog.LogRecorderInterface
{
    /// <summary>
    /// 记录接口
    /// </summary>
    public interface IRecorder : ILoger
    {
        /// <summary>
        /// 基础配置
        /// </summary>
        IConfig BaseConfig { get; set; }

        /// <summary>
        /// 内部日志添加到ILoger记录事件
        /// </summary>
        event EventHandler<InnerLogAddedIlogerArgs> InnerLogAddedIloger;

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="item">日志项</param>
        void WriteLog(LogItem item);

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="items">日志项集合</param>
        void WriteLog(List<LogItem> items);
    }
}
