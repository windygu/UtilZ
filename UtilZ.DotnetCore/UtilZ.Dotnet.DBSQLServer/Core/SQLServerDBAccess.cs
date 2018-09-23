using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using UtilZ.Dotnet.DBBase.Core;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.Dotnet.DBBase.Model;

namespace UtilZ.Dotnet.DBSQLServer.Core
{
    /// <summary>
    /// SQLite数据库访问类
    /// </summary>
    public partial class SQLServerDBAccess : DBAccessBase
    {
        #region 重写父类方法属性
        /// <summary>
        /// 数据库参数字符
        /// </summary>
        private const string PARASIGN = "@";

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
        private readonly string _databaseName = typeof(SqlConnection).Assembly.FullName;

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
        public SQLServerDBAccess(int dbid, IDBInteractiveBuilder interactiveBuilder) : base(dbid, interactiveBuilder)
        {
            if (base._config.SqlMaxLength == DBConstant.SqlMaxLength)
            {
                //65,536 * 网络数据包大小,网络数据包大小指的是用于在应用程序和关系 数据库引擎 之间进行通信的表格格式数据流(TDS) 数据包的大小。 默认的数据包大小为 4 KB，由“网络数据包大小”配置选项控制。
                this.SqlMaxLength = 268435456;
            }
            else
            {
                this.SqlMaxLength = base._config.SqlMaxLength;
            }
        }
    }
}
