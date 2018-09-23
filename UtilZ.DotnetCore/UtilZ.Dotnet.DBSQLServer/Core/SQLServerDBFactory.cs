using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UtilZ.Dotnet.DBBase.Core;
using UtilZ.Dotnet.DBBase.Interfaces;

namespace UtilZ.Dotnet.DBSQLServer.Core
{
    public class SQLServerDBFactory : DBFactoryBase
    {
        /// <summary>
        /// 数据库交互构建对象
        /// </summary>
        private readonly IDBInteractiveBuilder _interactiveBuilder = new SQLServerInteractiveBuilder();

        /// <summary>
        /// 构造函数
        /// </summary>
        public SQLServerDBFactory() : base()
        {

        }

        // <summary>
        /// 创建数据库访问实例
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <returns>数据库访问实例</returns>
        protected override IDBAccess CreateDBAccess(int dbid)
        {
            return new SQLServerDBAccess(dbid, this._interactiveBuilder);
        }
    }
}
