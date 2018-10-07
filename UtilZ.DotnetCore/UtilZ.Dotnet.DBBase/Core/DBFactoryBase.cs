using System;
using System.Collections.Generic;
using System.Text;
using UtilZ.Dotnet.DBBase.Interfaces;

namespace UtilZ.Dotnet.DBBase.Core
{
    public abstract class DBFactoryBase
    {
        // <summary>
        /// 创建数据库访问实例
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <returns>数据库访问实例</returns>
        protected abstract IDBAccess CreateDBAccess(int dbid);
    }
}
