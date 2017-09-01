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
        /// <param name="con">数据库连接对象</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回执行结果</returns>
        public object BaseExecuteScalar(string sqlStr, DBVisitType visitType, IDbConnection con = null, NDbParameterCollection collection = null)
        {
            return base.InnerExecuteScalar(con, sqlStr, collection);
        }

        /// <summary>
        /// ExecuteNonQuery执行SQL语句,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="visitType">数据库访问类型</param>
        /// <param name="con">数据库连接对象</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回执行结果</returns>
        public int BaseExecuteNonQuery(string sqlStr, DBVisitType visitType, IDbConnection con = null, NDbParameterCollection collection = null)
        {
            return base.InnerExecuteNonQuery(con, sqlStr, collection);
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="para">参数</param>
        /// <param name="con">数据库连接对象</param>
        /// <returns>执行结果</returns>
        public StoredProcedureResult BaseExcuteStoredProcedure(StoredProcedurePara para, IDbConnection con = null)
        {
            return base.ExcuteStoredProcedure(para, con);
        }

        /// <summary>
        /// 执行ADO.NET事务
        /// </summary>
        /// <param name="para">参数</param>
        /// <param name="function">事务执行委托</param>
        /// <param name="con">数据库连接对象</param>
        /// <returns>事务返回值</returns>
        public object BaseExcuteAdoNetTransaction(object para, Func<IDbConnection, IDbTransaction, object, object> function, IDbConnection con = null)
        {
            return base.ExcuteAdoNetTransaction(para, function, con);
        }
        #endregion

        #region 插入
        #region 单项插入
        /// <summary>
        /// 插入数据,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <param name="con">数据库连接对象</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseInsert(string sqlStr, NDbParameterCollection collection = null, IDbConnection con = null)
        {
            return base.Insert(sqlStr, collection, con);
        }

        /// <summary>
        /// 插入泛型T类型对象数据到数据到库,返回受影响的行数,仅支持单表
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="item">插入项</param>
        /// <param name="con">数据库连接对象</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseInsertT<T>(T item, IDbConnection con = null) where T : class
        {
            return base.InsertT<T>(item, con);
        }
        #endregion

        #region 批量插入
        /// <summary>
        /// 批量插入数据,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols">列名集合</param>
        /// <param name="data">数据</param>
        /// <param name="con">数据库连接对象</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseBatchInsert(string tableName, IEnumerable<string> cols, IEnumerable<object[]> data, IDbConnection con = null)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("tableName");
            }

            if (cols == null || cols.Count() == 0 || data == null || data.Count() == 0)
            {
                return 0L;
            }

            if (con == null)
            {
                con = this.GetDbConnection(DBVisitType.W);
            }

            bool closeFlag = false;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
                closeFlag = true;
            }

            try
            {
                using (IDbTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        IDbCommand cmd = con.CreateCommand();
                        cmd.Transaction = transaction;
                        cmd.CommandText = this.Interaction.GenerateSqlInsert(tableName, this.ParaSign, cols);
                        cmd.Prepare();
                        this.InitCommand(cmd);
                        long effectCount = 0;

                        foreach (var arr in data)
                        {
                            this.Interaction.SetParameter(cmd, cols, arr);
                            effectCount += cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return effectCount;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            finally
            {
                if (closeFlag)
                {
                    con.Close();
                }
            }
        }

        /// <summary>
        /// 批量插入数据,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collections">命令的参数集合</param>
        /// <param name="con">数据库连接对象</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseBatchInsert(string sqlStr, IEnumerable<NDbParameterCollection> collections, IDbConnection con = null)
        {
            return base.BatchInsert(sqlStr, collections, con);
        }

        /// <summary>
        /// 批量插入泛型T类型对象数据到数据到库,返回受影响的行数,仅支持单表
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="items">插入项集合</param>
        /// <param name="con">数据库连接对象</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseBatchInsertT<T>(IEnumerable<T> items, IDbConnection con = null) where T : class
        {
            if (items == null || items.Count() == 0)
            {
                return 0;
            }

            if (con == null)
            {
                con = this.GetDbConnection(DBVisitType.W);
            }

            bool closeFlag = false;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
                closeFlag = true;
            }

            try
            {
                List<string> cols;
                List<object[]> data;
                string tableName = this.ConvertTToBatchValue(items, con, out cols, out data);
                return base.BatchInsert(tableName, cols, data, con);
            }
            finally
            {
                if (closeFlag)
                {
                    con.Close();
                }
            }
        }
        #endregion
        #endregion

        #region 删除
        #region 单项删除
        /// <summary>
        /// 删除记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="con">数据库连接对象</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseDelete(string sqlStr, IDbConnection con = null, NDbParameterCollection collection = null)
        {
            return base.Delete(sqlStr, con, collection);
        }

        /// <summary>
        /// 删除记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColValues">主键列名值映射字典</param>
        /// <param name="con">数据库连接对象</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseDelete(string tableName, Dictionary<string, object> priKeyColValues, IDbConnection con = null)
        {
            return base.Delete(tableName, priKeyColValues, con);
        }

        /// <summary>
        /// 删除泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="item">要删除的对象</param>
        /// <param name="con">数据库连接对象</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseDeleteT<T>(T item, IDbConnection con = null) where T : class
        {
            return base.DeleteT<T>(item, con);
        }
        #endregion

        #region 批量删除
        /// <summary>
        /// 批量删除记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStrs">Sql语句集合</param>
        /// <param name="con">数据库连接对象</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseBatchDelete(IEnumerable<string> sqlStrs, IDbConnection con = null)
        {
            return base.BatchDelete(sqlStrs, con);
        }

        /// <summary>
        /// 批量删除记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColValues">主键列名值映射字典</param>
        /// <param name="con">数据库连接对象</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseBatchDelete(string tableName, IEnumerable<Dictionary<string, object>> priKeyColValues, IDbConnection con = null)
        {
            return base.BatchDelete(tableName, priKeyColValues, con);
        }

        /// <summary>
        /// 批量删除记录,返回受影响的行数
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collections">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseBatchDelete(string sqlStr, IEnumerable<NDbParameterCollection> collections, IDbConnection con = null)
        {
            return base.BatchDelete(sqlStr, collections, con);
        }

        /// <summary>
        /// 批量删除泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="items">要删除的集合</param>
        /// <param name="con">数据库连接对象</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseBatchDeleteT<T>(IEnumerable<T> items, IDbConnection con = null) where T : class
        {
            return base.BatchDeleteT<T>(items, con);
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
        /// <param name="con">数据库连接对象</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseUpdate(string tableName, Dictionary<string, object> priKeyColValues, Dictionary<string, object> colValues, IDbConnection con = null)
        {
            return base.Update(tableName, priKeyColValues, colValues, con);
        }

        /// <summary>
        /// 更新记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <param name="con">数据库连接对象</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseUpdate(string sqlStr, NDbParameterCollection collection = null, IDbConnection con = null)
        {
            return base.Update(sqlStr, collection, con);
        }

        /// <summary>
        /// 更新泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="item">对象</param>
        /// <param name="updateProperties">要更新到数据库表的属性名称集合,为null时全部更新</param>
        /// <param name="con">数据连接对象</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseUpdateT<T>(T item, IEnumerable<string> updateProperties = null, IDbConnection con = null) where T : class
        {
            return base.UpdateT<T>(item, updateProperties, con);
        }
        #endregion

        #region 批量更新
        /// <summary>
        /// 批量更新记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collections">命令的参数集合</param>
        /// <param name="con">数据连接对象</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseBatchUpdate(string sqlStr, IEnumerable<NDbParameterCollection> collections = null, IDbConnection con = null)
        {
            return base.BatchUpdate(sqlStr, collections, con);
        }

        /// <summary>
        /// 批量更新记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStrs">Sql语句集合</param>
        /// <param name="con">数据连接对象</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseBatchUpdate(IEnumerable<string> sqlStrs, IDbConnection con = null)
        {
            return base.BatchUpdate(sqlStrs, con);
        }

        /// <summary>
        /// 批量更新泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="items">要更新的对象集合</param>
        /// <param name="updateProperties">要更新到数据库表的属性名称集合,为null时全部更新</param>
        /// <param name="con">数据连接对象</param>
        /// <returns>返回受影响的行数</returns>
        public long BaseBatchUpdateT<T>(IEnumerable<T> items, IEnumerable<string> updateProperties = null, IDbConnection con = null) where T : class
        {
            return base.BatchUpdateT<T>(items, updateProperties, con);
        }
        #endregion
        #endregion
    }
}
