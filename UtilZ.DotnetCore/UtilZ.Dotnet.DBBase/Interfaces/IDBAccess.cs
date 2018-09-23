using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using UtilZ.Dotnet.DBBase.Core;
using UtilZ.Dotnet.DBBase.Model;

namespace UtilZ.Dotnet.DBBase.Interfaces
{
    /// <summary>
    /// 数据库访问接口
    /// </summary>
    public partial interface IDBAccess : IDisposable
    {
        #region 属性
        /// <summary>
        /// 数据库编号ID
        /// </summary>
        int DBID { get; }

        /// <summary>
        /// 数据库参数字符
        /// </summary>
        string ParaSign { get; }

        /// <summary>
        /// 数据库类型名称
        /// </summary>
        string DatabaseTypeName { get; }

        /// <summary>
        /// sql语句最大长度
        /// </summary>
        long SqlMaxLength { get; }
        #endregion

        #region ADO.NET执行原子操作方法
        /// <summary>
        /// 创建数据库接连对象
        /// </summary>
        /// <param name="visitType">数据库访问类型</param>
        /// <returns>数据库接连对象</returns>
        DbConnectionInfo CreateConnection(DBVisitType visitType);

        /// <summary>
        /// 检查数据库连接[连接正常返回true;否则返回false]
        /// </summary>
        /// <param name="visitType">数据库访问类型</param>
        /// <returns>连接正常返回true;否则返回false</returns>
        bool CheckDbConnection(DBVisitType visitType);

        /// <summary>
        /// 创建命令
        /// </summary>
        /// <returns>命令</returns>
        IDbCommand CreateCommand();

        /// <summary>
        /// 创建DbDataAdapter
        /// </summary>
        /// <returns>DbDataAdapter</returns>
        IDbDataAdapter CreateDbDataAdapter();

        /// <summary>
        /// ExecuteScalar执行SQL语句,返回执行结果的第一行第一列；
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="visitType">数据库访问类型</param>
        /// <param name="parameters">命令的参数集合</param>
        /// <returns>返回执行结果</returns>
        object ExecuteScalar(string sqlStr, DBVisitType visitType, DbParameterCollection parameters = null);

        /// <summary>
        /// ExecuteNonQuery执行SQL语句,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="visitType">数据库访问类型</param>
        /// <param name="parameters">命令的参数集合</param>
        /// <returns>返回执行结果</returns>
        int ExecuteNonQuery(string sqlStr, DBVisitType visitType, DbParameterCollection parameters = null);
        #endregion
    }
}
