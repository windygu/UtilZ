using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.DataBaseAccess.DBModel.Constant
{
    /// <summary>
    /// 常量定义类
    /// </summary>
    public class DBConstant
    {
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
        public const int WriteConCount = 1;

        /// <summary>
        /// 数据库读连接数默认值
        /// </summary>
        public const int ReadConCount = 1;

        /// <summary>
        /// 获取连接超时时长默认值
        /// </summary>
        public const int GetConTimeout = 10000;
    }
}
