using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBIBase.DBModel.Model;

namespace UtilZ.Dotnet.DBSQLite.Interface
{
    /// <summary>
    /// SQLite调用基类方法接口
    /// </summary>
    public interface ISQLiteDBAccessBase
    {
        #region ADO.NET执行原子操作方法
        /// <summary>
        /// ExecuteScalar执行SQL语句,返回执行结果的第一行第一列；
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="visitType">数据库访问类型</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回执行结果</returns>
        object BaseExecuteScalar(string sqlStr, DBVisitType visitType, NDbParameterCollection collection = null);

        /// <summary>
        /// ExecuteNonQuery执行SQL语句,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="visitType">数据库访问类型</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回执行结果</returns>
        int BaseExecuteNonQuery(string sqlStr, DBVisitType visitType, NDbParameterCollection collection = null);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="para">参数</param>
        /// <returns>执行结果</returns>
        StoredProcedureResult BaseExcuteStoredProcedure(StoredProcedurePara para);

        /// <summary>
        /// 执行ADO.NET事务
        /// </summary>
        /// <param name="para">参数</param>
        /// <param name="function">事务执行委托</param>
        /// <returns>事务返回值</returns>
        object BaseExcuteAdoNetTransaction(object para, Func<IDbConnection, IDbTransaction, object, object> function);
        #endregion

        #region 插入
        #region 单项插入
        /// <summary>
        /// 插入数据,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        long BaseInsert(string sqlStr, NDbParameterCollection collection = null);

        /// <summary>
        /// 插入泛型T类型对象数据到数据到库,返回受影响的行数,仅支持单表
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="item">插入项</param>
        /// <returns>返回受影响的行数</returns>
        long BaseInsertT<T>(T item) where T : class;
        #endregion

        #region 批量插入
        /// <summary>
        /// 批量插入数据,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols">列名集合</param>
        /// <param name="data">数据</param>
        /// <returns>返回受影响的行数</returns>
        long BaseBatchInsert(string tableName, IEnumerable<string> cols, IEnumerable<object[]> data);

        /// <summary>
        /// 批量插入数据,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collections">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        long BaseBatchInsert(string sqlStr, IEnumerable<NDbParameterCollection> collections);
        #endregion
        #endregion

        #region 删除
        #region 单项删除
        /// <summary>
        /// 删除记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        long BaseDelete(string sqlStr, NDbParameterCollection collection = null);

        /// <summary>
        /// 删除记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColValues">主键列名值映射字典</param>
        /// <returns>返回受影响的行数</returns>
        long BaseDelete(string tableName, Dictionary<string, object> priKeyColValues);

        /// <summary>
        /// 删除泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="item">要删除的对象</param>
        /// <param name="conditionProperties">条件属性集合[该集合为空或null时仅用主键字段]</param>
        /// <returns>返回受影响的行数</returns>
        long BaseDeleteT<T>(T item, IEnumerable<string> conditionProperties) where T : class;
        #endregion

        #region 批量删除
        /// <summary>
        /// 批量删除记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStrs">Sql语句集合</param>
        /// <returns>返回受影响的行数</returns>
        long BaseBatchDelete(IEnumerable<string> sqlStrs);

        /// <summary>
        /// 批量删除记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColValues">主键列名值映射字典</param>
        /// <returns>返回受影响的行数</returns>
        long BaseBatchDelete(string tableName, IEnumerable<Dictionary<string, object>> priKeyColValues);

        /// <summary>
        /// 批量删除记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collections">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        long BaseBatchDelete(string sqlStr, IEnumerable<NDbParameterCollection> collections);

        /// <summary>
        /// 批量删除泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="items">要删除的集合</param>
        /// <returns>返回受影响的行数</returns>
        long BaseBatchDeleteT<T>(IEnumerable<T> items) where T : class;
        #endregion
        #endregion

        #region 更新
        #region 单项更新
        /// <summary>
        /// 更新记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColValues">主键列名值映射字典</param>
        /// <param name="colValues">修改列名值字典</param>
        /// <returns>返回受影响的行数</returns>
        long BaseUpdate(string tableName, Dictionary<string, object> priKeyColValues, Dictionary<string, object> colValues);

        /// <summary>
        /// 更新记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        long BaseUpdate(string sqlStr, NDbParameterCollection collection = null);

        /// <summary>
        /// 更新泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="item">对象</param>
        /// <param name="updateProperties">要更新到数据库表的属性名称集合,为null时全部更新</param>
        /// <returns>返回受影响的行数</returns>
        long BaseUpdateT<T>(T item, IEnumerable<string> updateProperties = null) where T : class;
        #endregion

        #region 批量更新
        /// <summary>
        /// 批量更新记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collections">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        long BaseBatchUpdate(string sqlStr, IEnumerable<NDbParameterCollection> collections = null);

        /// <summary>
        /// 批量更新记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStrs">Sql语句集合</param>
        /// <returns>返回受影响的行数</returns>
        long BaseBatchUpdate(IEnumerable<string> sqlStrs);

        /// <summary>
        /// 批量更新泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="items">要更新的对象集合</param>
        /// <param name="updateProperties">要更新到数据库表的属性名称集合,为null时全部更新</param>
        /// <returns>返回受影响的行数</returns>
        long BaseBatchUpdateT<T>(IEnumerable<T> items, IEnumerable<string> updateProperties = null) where T : class;
        #endregion
        #endregion
    }
}
