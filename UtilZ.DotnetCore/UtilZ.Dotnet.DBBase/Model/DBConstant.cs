using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.Dotnet.DBBase.Model
{
    /// <summary>
    /// 常量定义类
    /// </summary>
    public class DBConstant
    {
        //数据库连接信息类型[0:字符串;1:ip端口号等分散信息]
        /// <summary>
        /// 数据库连接信息类型-字符串
        /// </summary>
        public const int ConnectionTypeStr = 0;

        /// <summary>
        /// 数据库连接信息类型-ip端口号等分散信息
        /// </summary>
        public const int ConnectionTypePart = 1;

        /// <summary>
        /// 配置移除订阅组ID
        /// </summary>
        public const string ConfigRemoveSubject = "ConfigRemove";

        /// <summary>
        /// 添加ConcurrentDictionary集合失败时重试次数
        /// </summary>
        public const int AddConcurrentDictionaryRepatCount = 3;

        /// <summary>
        /// SQL语句执行默认超时时间,单位/秒
        /// </summary>
        public const int CommandTimeout = -1;

        /// <summary>
        /// sql语句最大长度默认值
        /// </summary>
        public const long SqlMaxLength = -1;

        /// <summary>
        /// 数据库写连接数默认值
        /// </summary>
        public const int WriteConnectionCount = 1;

        /// <summary>
        /// 数据库读连接数默认值
        /// </summary>
        public const int ReadConnectionCount = 1;

        /// <summary>
        /// 获取连接超时时长默认值
        /// </summary>
        public const int GetConnectionObjectTimeout = 10000;
    }
}
