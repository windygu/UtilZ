using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.Log.Config.Core;
using UtilZ.Lib.Base.Log.Config.Interface;

namespace UtilZ.Lib.Base.Log.Config.Framework
{
    /// <summary>
    /// 邮件配置节点元素
    /// </summary>
    public class MailLogConfigElement : BaseLogConfig, IEmailLogConfig
    {
        /// <summary>
        /// 邮件服务器主机地址属性名称
        /// </summary>
        private const string SmtpHostPropertyName = "SmtpHost";

        /// <summary>
        /// 用户名属性名称
        /// </summary>
        private const string UserNamePropertyName = "UserName";

        /// <summary>
        /// 密码属性名称
        /// </summary>
        private const string PasswordPropertyName = "Password";

        /// <summary>
        /// 发件人属性名称
        /// </summary>
        private const string FromPropertyName = "From";

        /// <summary>
        /// 收件人集合属性名称
        /// </summary>
        private const string ToPropertyName = "To";

        /// <summary>
        /// 抄送收件人集合属性名称
        /// </summary>
        private const string CCPropertyName = "CC";

        /// <summary>
        /// 密件抄送收件人集合属性名称
        /// </summary>
        private const string BccPropertyName = "Bcc";

        /// <summary>
        /// 邮件服务器主机地址
        /// </summary>
        [ConfigurationProperty(MailLogConfigElement.SmtpHostPropertyName, DefaultValue = null, IsRequired = true)]
        public string SmtpHost
        {
            get
            {
                return Convert.ToString(this[MailLogConfigElement.SmtpHostPropertyName]);
            }
            set
            {
                this[MailLogConfigElement.SmtpHostPropertyName] = value;
            }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        [ConfigurationProperty(MailLogConfigElement.UserNamePropertyName, DefaultValue = null, IsRequired = true)]
        public string UserName
        {
            get
            {
                return Convert.ToString(this[MailLogConfigElement.UserNamePropertyName]);
            }
            set
            {
                this[MailLogConfigElement.UserNamePropertyName] = value;
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        [ConfigurationProperty(MailLogConfigElement.PasswordPropertyName, DefaultValue = null, IsRequired = true)]
        public string Password
        {
            get
            {
                return Convert.ToString(this[MailLogConfigElement.PasswordPropertyName]);
            }
            set
            {
                this[MailLogConfigElement.PasswordPropertyName] = value;
            }
        }

        /// <summary>
        /// 发件人
        /// </summary>
        [ConfigurationProperty(MailLogConfigElement.FromPropertyName, IsRequired = true)]
        public MailAddrConfigElement From
        {
            get
            {
                return this[MailLogConfigElement.FromPropertyName] as MailAddrConfigElement;
            }
            set
            {
                this[MailLogConfigElement.FromPropertyName] = value;
            }
        }

        /// <summary>
        /// 收件人集合
        /// </summary>
        [ConfigurationProperty(MailLogConfigElement.ToPropertyName, IsRequired = true)]
        public MailAddrConfigCollection To
        {
            get
            {
                return this[MailLogConfigElement.ToPropertyName] as MailAddrConfigCollection;
            }
            set
            {
                this[MailLogConfigElement.ToPropertyName] = value;
            }
        }

        /// <summary>
        /// 抄送收件人集合
        /// </summary>
        [ConfigurationProperty(MailLogConfigElement.CCPropertyName, IsRequired = false)]
        public MailAddrConfigCollection CC
        {
            get
            {
                return this[MailLogConfigElement.CCPropertyName] as MailAddrConfigCollection;
            }
            set
            {
                this[MailLogConfigElement.CCPropertyName] = value;
            }
        }

        /// <summary>
        /// 密件抄送收件人集合
        /// </summary>
        [ConfigurationProperty(MailLogConfigElement.BccPropertyName, IsRequired = false)]
        public MailAddrConfigCollection Bcc
        {
            get
            {
                return this[MailLogConfigElement.BccPropertyName] as MailAddrConfigCollection;
            }
            set
            {
                this[MailLogConfigElement.BccPropertyName] = value;
            }
        }
    }
}
