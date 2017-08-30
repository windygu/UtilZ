using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.NLog.Config.Framework;

namespace UtilZ.Lib.Base.NLog.Config.Interface
{
    /// <summary>
    /// 邮件日志配置接口
    /// </summary>
    public interface IEmailLogConfig : IConfig
    {
        /// <summary>
        /// 邮件服务器主机地址
        /// </summary>
        string SmtpHost { get; set; }

        /// <summary>
        /// 用户名
        /// </summary> 
        string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// 发件人
        /// </summary>
        MailAddrConfigElement From { get; set; }

        /// <summary>
        /// 收件人集合
        /// </summary>
        MailAddrConfigCollection To { get; set; }

        /// <summary>
        /// 抄送收件人集合
        /// </summary>
        MailAddrConfigCollection CC { get; set; }

        /// <summary>
        /// 密件抄送收件人集合
        /// </summary>
        MailAddrConfigCollection Bcc { get; set; }
    }
}
