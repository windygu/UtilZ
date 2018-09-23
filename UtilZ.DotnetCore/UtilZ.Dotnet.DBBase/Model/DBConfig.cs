using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using UtilZ.Dotnet.DBBase.Common;
using UtilZ.Dotnet.DBBase.Model;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.DBBase.Model
{
    /// <summary>
    /// 日志基础配置类
    /// </summary>
    public class DBConfig
    {
        /// <summary>
        /// 数据库编号,int.MinValue无效编号
        /// </summary>
        public int DBID { get; set; }

        /// <summary>
        /// 数据库连接名称
        /// </summary>
        public string Name { get; set; } = null;

        /// <summary>
        /// 数据库连接信息类型[0:字符串;1:ip端口号等分散信息]
        /// </summary>
        public int ConnectionType { get; set; } = DBConstant.ConnectionTypeStr;

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; } = null;

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string Host { get; set; } = null;

        /// <summary>
        /// 数据库服务器端口号
        /// </summary>
        public int Port { get; set; } = 0;

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName { get; set; } = null;

        /// <summary>
        /// 帐号
        /// </summary>
        public string Account { get; set; } = null;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = null;

        /// <summary>
        /// SQL语句执行超时时间,DBConstant.CommandTimeout为默认值
        /// </summary>
        public int CommandTimeout { get; set; } = DBConstant.CommandTimeout;

        /// <summary>
        /// 数据库连接信息解密接口程序集名
        /// </summary>
        public string Decryption { get; set; } = null;

        /// <summary>
        /// 数据库访问对象创建工厂类型
        /// </summary>
        public string DBFactory { get; set; } = null;

        /// <summary>
        /// sql语句最大长度,DBConstant.SqlMaxLength为数制库默认值
        /// </summary>
        public long SqlMaxLength { get; set; } = DBConstant.SqlMaxLength;

        /// <summary>
        /// 数据库写连接数,小于1为不限制
        /// </summary>
        public int WriteConnectionCount { get; set; } = DBConstant.WriteConnectionCount;

        /// <summary>
        /// 数据库读连接数,小于1为不限制
        /// </summary>
        public int ReadConnectionCount { get; set; } = DBConstant.ReadConnectionCount;

        /// <summary>
        /// 获取连接超时时长,单位/毫秒
        /// </summary>
        public int GetConnectionObjectTimeout { get; set; } = DBConstant.GetConnectionObjectTimeout;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DBConfig()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ele">配置元素</param>
        public DBConfig(XElement ele)
        {
            if (ele == null)
            {
                throw new ArgumentNullException(nameof(ele));
            }

            string tmpStr;
            tmpStr = XmlEx.GetXElementAttributeValue(ele, nameof(this.DBID).ToUpper());
            if (string.IsNullOrWhiteSpace(tmpStr))
            {
                throw new ArgumentNullException(nameof(this.DBID));
            }

            this.DBID = int.Parse(tmpStr);
            this.Name = XmlEx.GetXElementAttributeValue(ele, nameof(this.Name));

            tmpStr = XmlEx.GetXElementAttributeValue(ele, nameof(this.ConnectionType));
            if (!string.IsNullOrWhiteSpace(tmpStr))
            {
                this.ConnectionType = int.Parse(tmpStr);
            }

            this.ConnectionString = XmlEx.GetXElementAttributeValue(ele, nameof(this.ConnectionString));
            this.Host = XmlEx.GetXElementAttributeValue(ele, nameof(this.Host));

            tmpStr = XmlEx.GetXElementAttributeValue(ele, nameof(this.Port));
            if (!string.IsNullOrWhiteSpace(tmpStr))
            {
                this.Port = int.Parse(tmpStr);
            }

            tmpStr = XmlEx.GetXElementAttributeValue(ele, nameof(this.DatabaseName));
            if (this.ConnectionType != DBConstant.ConnectionTypeStr && string.IsNullOrWhiteSpace(tmpStr))
            {
                throw new ArgumentNullException(nameof(this.DatabaseName));
            }

            this.DatabaseName = tmpStr;

            this.Account = XmlEx.GetXElementAttributeValue(ele, nameof(this.Account));
            this.Password = XmlEx.GetXElementAttributeValue(ele, nameof(this.Password));

            string str = XmlEx.GetXElementAttributeValue(ele, nameof(this.CommandTimeout));
            int commandTimeout;
            if (!string.IsNullOrWhiteSpace(str) && int.TryParse(str, out commandTimeout))
            {
                this.CommandTimeout = commandTimeout;
            }

            this.Decryption = XmlEx.GetXElementAttributeValue(ele, nameof(this.Decryption));

            tmpStr = XmlEx.GetXElementAttributeValue(ele, nameof(this.DBFactory));
            if (string.IsNullOrWhiteSpace(tmpStr))
            {
                throw new ArgumentNullException(nameof(this.DBFactory));
            }

            this.DBFactory = tmpStr;

            str = XmlEx.GetXElementAttributeValue(ele, nameof(this.CommandTimeout));
            long sqlMaxLength;
            if (!string.IsNullOrWhiteSpace(str) && long.TryParse(str, out sqlMaxLength))
            {
                this.SqlMaxLength = sqlMaxLength;
            }

            str = XmlEx.GetXElementAttributeValue(ele, nameof(this.WriteConnectionCount));
            if (!string.IsNullOrWhiteSpace(str))
            {
                this.WriteConnectionCount = int.Parse(str);
            }

            str = XmlEx.GetXElementAttributeValue(ele, nameof(this.ReadConnectionCount));
            if (!string.IsNullOrWhiteSpace(str))
            {
                this.ReadConnectionCount = int.Parse(str);
            }

            str = XmlEx.GetXElementAttributeValue(ele, nameof(this.GetConnectionObjectTimeout));
            int getConTimeout;
            if (!string.IsNullOrWhiteSpace(str) && int.TryParse(str, out getConTimeout))
            {
                this.GetConnectionObjectTimeout = getConTimeout;
            }
        }

        /// <summary>
        /// 返回表示当前对象的String
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = this.Name;
            if (string.IsNullOrWhiteSpace(str))
            {
                str = this.DatabaseName;
            }

            return str;
        }
    }
}
