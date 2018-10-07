using System;
using System.Collections.Generic;
using System.Text;
using UtilZ.Dotnet.DBBase.Common;
using UtilZ.Dotnet.DBBase.Core;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.Dotnet.DBBase.Model;

namespace UtilZ.Dotnet.DBMySql.Core
{
    /// <summary>
    /// MySql数据库访问类
    /// </summary>
    public partial class MySqlDBAccess : DBAccessBase
    {
        #region 重写父类方法属性
        /// <summary>
        /// 数据库参数字符
        /// </summary>
        private const string PARASIGN = "?";

        /// <summary>
        /// 数据库参数字符
        /// </summary>
        public override string ParaSign
        {
            get { return PARASIGN; }
        }

        /// <summary>
        /// 数据库程序集名称
        /// </summary>
        private readonly string _databaseName = typeof(MySql.Data.MySqlClient.MySqlConnection).Assembly.FullName;

        /// <summary>
        /// 数据库类型名称
        /// </summary>
        public override string DatabaseTypeName
        {
            get { return _databaseName; }
        }

        /// <summary>
        /// sql语句最大长度
        /// </summary>
        public override long SqlMaxLength { get; protected set; }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <param name="interactiveBuilder">数据库交互构建对象</param>
        public MySqlDBAccess(int dbid, IDBInteractiveBuilder interactiveBuilder) : base(dbid, interactiveBuilder)
        {
            if (base._config.SqlMaxLength == DBConstant.SqlMaxLength)
            {
                this.SqlMaxLength = 1048576;
            }
            else
            {
                this.SqlMaxLength = base._config.SqlMaxLength;
            }
        }
    }
}
