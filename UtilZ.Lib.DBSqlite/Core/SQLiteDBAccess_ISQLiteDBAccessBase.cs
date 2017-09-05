using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Lib.DBBase.Core;
using UtilZ.Lib.DBModel.DBInfo;
using UtilZ.Lib.DBModel.DBObject;
using UtilZ.Lib.DBModel.Model;

namespace UtilZ.Lib.DBSqlite.Core
{
    //SQLite数据库访问类-ISQLiteDBAccessBase接口
    public partial class SQLiteDBAccess
    {
        #region ADO.NET执行原子操作方法
        /// <summary>
        /// ExecuteScalar执行SQL语句,返回执行结果的第一行第一列；
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="visitType">数据库访问类型</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回执行结果</returns>
        public object BaseExecuteScalar(string sqlStr, DBVisitType visitType, NDbParameterCollection collection = null)
        {
            return base.ExecuteScalar(sqlStr, visitType, collection);
        }

        /// <summary>
        /// ExecuteNonQuery执行SQL语句,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="visitType">数据库访问类型</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回执行结果</returns>
        public int BaseExecuteNonQuery(string sqlStr, DBVisitType visitType, NDbParameterCollection collection = null)
        {
            return base.ExecuteNonQuery(sqlStr, visitType, collection);
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="para">参数</param>
        /// <returns>执行结果</returns>
        public StoredProcedureResult BaseExcuteStoredProcedure(StoredProcedurePara para)
        {
            return base.ExcuteStoredProcedure(para);
        }

        /// <summary>
        /// 执行ADO.NET事务
        /// </summary>
        /// <param name="para">参数</param>
        /// <param name="function">事务执行委托</param>
        /// <returns>事务返回值</returns>
        public object BaseExcuteAdoNetTransaction(object para, Func<IDbConnection, IDbTransaction, object, object> function)
        {
            return base.ExcuteAdoNetTransaction(para, function);
        }
        #endregion

        #region 插入
        #region 单项插入
        /// <summary>
        /// 插入数据,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseInsert(string sqlStr, NDbParameterCollection collection = null)
        {
            return base.Insert(sqlStr, collection);
        }

        /// <summary>
        /// 插入泛型T类型对象数据到数据到库,返回受影响的行数,仅支持单表
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="item">插入项</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseInsertT<T>(T item) where T : class
        {
            return base.InsertT<T>(item);
        }
        #endregion

        #region 批量插入
        /// <summary>
        /// 批量插入数据,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols">列名集合</param>
        /// <param name="data">数据</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseBatchInsert(string tableName, IEnumerable<string> cols, IEnumerable<object[]> data)
        {
            return base.BatchInsert(tableName, cols, data);
        }

        /// <summary>
        /// 批量插入数据,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collections">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseBatchInsert(string sqlStr, IEnumerable<NDbParameterCollection> collections)
        {
            return base.BatchInsert(sqlStr, collections);
        }
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
        public long BaseDelete(string sqlStr, NDbParameterCollection collection = null)
        {
            return base.Delete(sqlStr, collection);
        }

        /// <summary>
        /// 删除记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColValues">主键列名值映射字典</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseDelete(string tableName, Dictionary<string, object> priKeyColValues)
        {
            return base.Delete(tableName, priKeyColValues);
        }

        /// <summary>
        /// 删除泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="item">要删除的对象</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseDeleteT<T>(T item) where T : class
        {
            return base.DeleteT<T>(item);
        }
        #endregion

        #region 批量删除
        /// <summary>
        /// 批量删除记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStrs">Sql语句集合</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseBatchDelete(IEnumerable<string> sqlStrs)
        {
            return base.BatchDelete(sqlStrs);
        }

        /// <summary>
        /// 批量删除记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColValues">主键列名值映射字典</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseBatchDelete(string tableName, IEnumerable<Dictionary<string, object>> priKeyColValues)
        {
            return base.BatchDelete(tableName, priKeyColValues);
        }

        /// <summary>
        /// 批量删除记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collections">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseBatchDelete(string sqlStr, IEnumerable<NDbParameterCollection> collections)
        {
            return base.BatchDelete(sqlStr, collections);
        }

        /// <summary>
        /// 批量删除泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="items">要删除的集合</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseBatchDeleteT<T>(IEnumerable<T> items) where T : class
        {
            return base.BatchDeleteT<T>(items);
        }
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
        public long BaseUpdate(string tableName, Dictionary<string, object> priKeyColValues, Dictionary<string, object> colValues)
        {
            return base.Update(tableName, priKeyColValues, colValues);
        }

        /// <summary>
        /// 更新记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseUpdate(string sqlStr, NDbParameterCollection collection = null)
        {
            return base.Update(sqlStr, collection);
        }

        /// <summary>
        /// 更新泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="item">对象</param>
        /// <param name="updateProperties">要更新到数据库表的属性名称集合,为null时全部更新</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseUpdateT<T>(T item, IEnumerable<string> updateProperties = null) where T : class
        {
            return base.UpdateT<T>(item, updateProperties);
        }
        #endregion

        #region 批量更新
        /// <summary>
        /// 批量更新记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collections">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseBatchUpdate(string sqlStr, IEnumerable<NDbParameterCollection> collections = null)
        {
            return base.BatchUpdate(sqlStr, collections);
        }

        /// <summary>
        /// 批量更新记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStrs">Sql语句集合</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseBatchUpdate(IEnumerable<string> sqlStrs)
        {
            return base.BatchUpdate(sqlStrs);
        }

        /// <summary>
        /// 批量更新泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="items">要更新的对象集合</param>
        /// <param name="updateProperties">要更新到数据库表的属性名称集合,为null时全部更新</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseBatchUpdateT<T>(IEnumerable<T> items, IEnumerable<string> updateProperties = null) where T : class
        {
            return base.BatchUpdateT<T>(items, updateProperties);
        }
        #endregion
        #endregion
    }
}
