using System;
using System.Data;
using UtilZ.Dotnet.DBIBase.DBModel.Interface;
using UtilZ.Dotnet.DBIBase.DBModel.Model;

namespace UtilZ.Dotnet.DBIBase.DBBase.Interface
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

        /// <summary>
        /// 外部数据库交互接口
        /// </summary>
        IDBInteractionEx InteractionEx { get; }
        #endregion

        /// <summary>
        /// 初始化读写连接池
        /// </summary>
        void Init();

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
        /// <returns>连接正常返回true;否则返回false</returns>
        bool CheckDbConnection();

        /// <summary>
        /// 创建命令
        /// </summary>
        /// <param name="con">连接对象</param>
        /// <returns>命令</returns>
        IDbCommand CreateCommand(IDbConnection con);

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
        object ExecuteScalar(string sqlStr, DBVisitType visitType, NDbParameterCollection parameters = null);

        /// <summary>
        /// ExecuteNonQuery执行SQL语句,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="visitType">数据库访问类型</param>
        /// <param name="parameters">命令的参数集合</param>
        /// <returns>返回执行结果</returns>
        int ExecuteNonQuery(string sqlStr, DBVisitType visitType, NDbParameterCollection parameters = null);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="para">参数</param>
        /// <returns>执行结果</returns>
        StoredProcedureResult ExcuteStoredProcedure(StoredProcedurePara para);

        /// <summary>
        /// 执行ADO.NET事务
        /// </summary>
        /// <param name="para">参数</param>
        /// <param name="function">事务执行委托[数据库连接,事务,入参,出参]</param>
        /// <returns>事务返回值</returns>
        object ExcuteAdoNetTransaction(object para, Func<IDbConnection, IDbTransaction, object, object> function);

        /// <summary>
        /// 创建EF上下文接口
        /// </summary>
        /// <param name="visitType">数据库访问类型</param>
        /// <returns>IEFDbContext</returns>
        IEFDbContext CreateEFDbContext(DBVisitType visitType);
        #endregion
    }
}
