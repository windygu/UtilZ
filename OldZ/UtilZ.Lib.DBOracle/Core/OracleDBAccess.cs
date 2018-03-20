using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Lib.DBBase.Core;
using UtilZ.Lib.DBModel.Constant;

namespace UtilZ.Lib.DBOracle.Core
{
    /// <summary>
    /// SQLServer数据库访问类
    /// </summary>
    public partial class OracleDBAccess : DBAccessBase
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
        private const string PARASIGN = ":";

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
        public OracleDBAccess(int dbid)
            : base(dbid)
        {
            this._databaseName = typeof(OracleConnection).Assembly.FullName;
            if (this.Config.SqlMaxLength == DBConstant.SqlMaxLength)
            {
                //Oracle文档说是64K
                this.SqlMaxLength = 65536;
            }
            else
            {
                this.SqlMaxLength = this.Config.SqlMaxLength;
            }
        }
    }
}
