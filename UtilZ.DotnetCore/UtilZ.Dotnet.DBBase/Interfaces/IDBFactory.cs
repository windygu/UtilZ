using System;
using System.Collections.Generic;
using System.Text;
using UtilZ.Dotnet.DBBase.Model;

namespace UtilZ.Dotnet.DBBase.Interfaces
{
    /// <summary>
    /// 数据库访问实例创建工厂
    /// </summary>
    public interface IDBFactory
    {
        /// <summary>
        /// 获取数据库访问实例
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <returns>数据库访问实例</returns>
        IDBAccess GetDBAccess(int dbid);
    }
}
