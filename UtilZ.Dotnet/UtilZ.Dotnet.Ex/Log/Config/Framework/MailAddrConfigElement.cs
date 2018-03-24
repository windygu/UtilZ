using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Log.Config.Core;

namespace UtilZ.Dotnet.Ex.Log.Config.Framework
{
    /// <summary>
    /// 邮件地址配置元素
    /// </summary>
    public class MailAddrConfigElement : BaseLogConfig
    {
        /// <summary>
        /// 地址属性名称
        /// </summary>
        private const string AddrPropertyName = "Addr";

        /// <summary>
        /// 显示名称属性名称
        /// </summary>
        private const string DisplayNamePropertyName = "DisplayName";

        /// <summary>
        /// 地址
        /// </summary>
        [ConfigurationProperty(MailAddrConfigElement.AddrPropertyName, DefaultValue = null, IsRequired = true)]
        public string Addr
        {
            get
            {
                return Convert.ToString(this[MailAddrConfigElement.AddrPropertyName]);
            }
            set
            {
                this[MailAddrConfigElement.AddrPropertyName] = value;
            }
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        [ConfigurationProperty(MailAddrConfigElement.DisplayNamePropertyName, DefaultValue = null, IsRequired = false)]
        public string DisplayName
        {
            get
            {
                return Convert.ToString(this[MailAddrConfigElement.DisplayNamePropertyName]);
            }
            set
            {
                this[MailAddrConfigElement.DisplayNamePropertyName] = value;
            }
        }
    }
}
