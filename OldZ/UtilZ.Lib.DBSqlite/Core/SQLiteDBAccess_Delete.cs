using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Lib.DBModel.Interface;
using UtilZ.Lib.DBModel.Model;
using UtilZ.Lib.DBSqlite.Write;

namespace UtilZ.Lib.DBSqlite.Core
{
    //SQLite数据库访问类-删除
    public partial class SQLiteDBAccess
    {
        #region 单项删除
        /// <summary>
        /// 删除记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        public override long Delete(string sqlStr, NDbParameterCollection collection = null)
        {
            if (string.IsNullOrWhiteSpace(sqlStr))
            {
                throw new ArgumentNullException("sqlStr");
            }

            var item = new SQLiteDelete(this._waitTimeout, sqlStr, collection);
            this._writeQueue.Enqueue(item);
            item.WaitOne();
            if (item.ExcuteResult)
            {
                return Convert.ToInt64(item.Result);
            }
            else
            {
                throw item.Result as Exception;
            }
        }

        /// <summary>
        /// 删除记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColValues">主键列名值映射字典</param>
        /// <returns>返回受影响的行数</returns>
        public override long Delete(string tableName, Dictionary<string, object> priKeyColValues)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("tableName");
            }

            var item = new SQLiteDelete(this._waitTimeout, tableName, priKeyColValues);
            this._writeQueue.Enqueue(item);
            item.WaitOne();
            if (item.ExcuteResult)
            {
                return Convert.ToInt64(item.Result);
            }
            else
            {
                throw item.Result as Exception;
            }
        }

        /// <summary>
        /// 删除泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="item">要删除的对象</param>
        /// <param name="conditionProperties">条件属性集合[该集合为空或null时仅用主键字段]</param>
        /// <returns>返回受影响的行数</returns>
        public override long DeleteT<T>(T item, IEnumerable<string> conditionProperties = null)
        {
            if (item == null)
            {
                return 0;
            }

            var delItem = new SQLiteDeleteT<T>(this._waitTimeout, item, conditionProperties);
            this._writeQueue.Enqueue(delItem);
            delItem.WaitOne();
            if (delItem.ExcuteResult)
            {
                return Convert.ToInt64(delItem.Result);
            }
            else
            {
                throw delItem.Result as Exception;
            }
        }
        #endregion

        #region 批量删除
        /// <summary>
        /// 批量删除记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStrs">Sql语句集合</param>
        /// <returns>返回受影响的行数</returns>
        public override long BatchDelete(IEnumerable<string> sqlStrs)
        {
            if (sqlStrs == null || sqlStrs.Count() == 0)
            {
                return 0L;
            }

            var item = new SQLiteDelete(this._waitTimeout, sqlStrs);
            this._writeQueue.Enqueue(item);
            item.WaitOne();
            if (item.ExcuteResult)
            {
                return Convert.ToInt32(item.Result);
            }
            else
            {
                throw item.Result as Exception;
            }
        }

        /// <summary>
        /// 批量删除记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColValues">主键列名值映射字典</param>
        /// <returns>返回受影响的行数</returns>
        public override long BatchDelete(string tableName, IEnumerable<Dictionary<string, object>> priKeyColValues)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("tableName");
            }

            var item = new SQLiteDelete(this._waitTimeout, tableName, priKeyColValues);
            this._writeQueue.Enqueue(item);
            item.WaitOne();
            if (item.ExcuteResult)
            {
                return Convert.ToInt32(item.Result);
            }
            else
            {
                throw item.Result as Exception;
            }
        }

        /// <summary>
        /// 批量删除记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collections">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        public override long BatchDelete(string sqlStr, IEnumerable<NDbParameterCollection> collections)
        {
            if (string.IsNullOrWhiteSpace(sqlStr))
            {
                throw new ArgumentNullException("sqlStr");
            }

            var item = new SQLiteDelete(this._waitTimeout, sqlStr, collections);
            this._writeQueue.Enqueue(item);
            item.WaitOne();
            if (item.ExcuteResult)
            {
                return Convert.ToInt32(item.Result);
            }
            else
            {
                throw item.Result as Exception;
            }
        }

        /// <summary>
        /// 批量删除泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="items">要删除的集合</param>
        /// <returns>返回受影响的行数</returns>
        public override long BatchDeleteT<T>(IEnumerable<T> items)
        {
            if (items == null || items.Count() == 0)
            {
                return 0L;
            }

            var item = new SQLiteDeleteT<T>(this._waitTimeout, items);
            this._writeQueue.Enqueue(item);
            item.WaitOne();
            if (item.ExcuteResult)
            {
                return Convert.ToInt32(item.Result);
            }
            else
            {
                throw item.Result as Exception;
            }
        }
        #endregion
    }
}
