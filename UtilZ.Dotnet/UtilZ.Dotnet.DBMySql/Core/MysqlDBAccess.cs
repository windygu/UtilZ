using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBIBase.DBBase.Core;
using UtilZ.Dotnet.DBIBase.DBModel.Constant;

namespace UtilZ.Dotnet.DBMySql.Core
{
    /// <summary>
    /// Mysql数据库访问类
    /// </summary>
    public partial class MysqlDBAccess : DBAccessBase
    {
        /// <summary>
        /// 数据库程序集名称
        /// </summary>
        private readonly string _databaseName;

        /// <summary>
        /// 数据库类型名称
        /// </summary>
        public override string DatabaseTypeName
        {
            get { return _databaseName; }
        }

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
        /// sql语句最大长度
        /// </summary>
        public override long SqlMaxLength { get; protected set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        public MysqlDBAccess(int dbid)
            : base(dbid)
        {
            _databaseName = typeof(MySqlConnection).Assembly.FullName;
            if (this.Config.SqlMaxLength == DBConstant.SqlMaxLength)
            {
                this.SqlMaxLength = 1048576;
            }
            else
            {
                this.SqlMaxLength = this.Config.SqlMaxLength;
            }
        }
    }
}
