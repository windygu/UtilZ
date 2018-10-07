using System;
using System.Collections.Generic;
using System.Text;
using UtilZ.Dotnet.DBBase.Core;
using UtilZ.Dotnet.DBBase.Interfaces;

namespace UtilZ.Dotnet.DBMySql.Core
{
    public class MySqlDBFactory : DBFactoryBase
    {
        /// <summary>
        /// 数据库交互构建对象
        /// </summary>
        private readonly IDBInteractiveBuilder _interactiveBuilder = new MySqlInteractiveBuilder();

        /// <summary>
        /// 构造函数
        /// </summary>
        public MySqlDBFactory() : base()
        {

        }

        // <summary>
        /// 创建数据库访问实例
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <returns>数据库访问实例</returns>
        protected override IDBAccess CreateDBAccess(int dbid)
        {
            return new MySqlDBAccess(dbid, this._interactiveBuilder);
        }
    }
}
