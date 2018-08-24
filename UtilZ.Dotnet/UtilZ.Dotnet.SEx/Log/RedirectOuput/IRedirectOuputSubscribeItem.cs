using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.Dotnet.SEx.Log.RedirectOuput
{
    /// <summary>
    /// 日志重定向输出订阅项接口
    /// </summary>
    public interface IRedirectOuputSubscribeItem
    {
        /// <summary>
        /// 日志输出事件
        /// </summary>
        event EventHandler<RedirectOuputArgs> LogOutput;
    }
}
