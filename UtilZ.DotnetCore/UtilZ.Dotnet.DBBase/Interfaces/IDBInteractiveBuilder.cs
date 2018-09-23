using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using UtilZ.Dotnet.DBBase.Model;

namespace UtilZ.Dotnet.DBBase.Interfaces
{
    /// <summary>
    /// 数据库交互构建接口
    /// </summary>
    public interface IDBInteractiveBuilder
    {
        /// <summary>
        /// 创建数据库读连接对象
        /// </summary>
        /// /// <param name="config">数据库配置</param>
        /// <param name="visitType">访问类型</param>
        /// <returns>数据库连接对象</returns>
        DbConnection CreateConnection(DBConfig config, DBVisitType visitType);

        /// <summary>
        /// 创建命令
        /// </summary>
        /// <returns>命令</returns>
        DbCommand CreateCommand();

        /// <summary>
        /// 创建DbDataAdapter
        /// </summary>
        /// <returns>创建好的DbDataAdapter</returns>
        IDbDataAdapter CreateDbDataAdapter();
    }
}
