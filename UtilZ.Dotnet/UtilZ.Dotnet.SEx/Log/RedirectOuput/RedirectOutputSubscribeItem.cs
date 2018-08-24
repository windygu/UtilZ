using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UtilZ.Dotnet.SEx.Log.Model;

namespace UtilZ.Dotnet.SEx.Log.RedirectOuput
{
    /// <summary>
    /// 输出日志订阅项
    /// </summary>
    [Serializable]
    public class RedirectOutputSubscribeItem : IRedirectOuputSubscribeItem
    {
        /// <summary>
        /// 日志输出事件
        /// </summary>
        public event EventHandler<RedirectOuputArgs> LogOutput;

        /// <summary>
        /// 日志系统日志输出过滤
        /// </summary>
        public RedirectOutputSubscribeItem()
        {

        }

        /// <summary>
        /// 日志输出
        /// </summary>
        /// <param name="logItem">要输出的日志项</param>
        public void Logoutput(RedirectOuputArgs logItem)
        {
            var handler = this.LogOutput;
            if (handler != null)
            {
                handler(this, logItem);
            }
        }
    }
}
