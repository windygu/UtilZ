using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Lib.DBModel.Model;
using UtilZ.Lib.DBSqlite.Write;

namespace UtilZ.Lib.DBSqlite.Core
{
    //SQLite数据库访问类-插入泛型数据
    public partial class SQLiteDBAccess
    {
        #region 单项插入
        /// <summary>
        /// 插入数据,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        public override long Insert(string sqlStr, NDbParameterCollection collection = null)
        {
            if (string.IsNullOrWhiteSpace(sqlStr))
            {
                return 0L;
            }

            var item = new SQLiteInsert(sqlStr, collection);
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
        /// 插入泛型T类型对象数据到数据到库,返回受影响的行数,仅支持单表
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="item">插入项</param>
        /// <returns>返回受影响的行数</returns>
        public override long InsertT<T>(T item)
        {
            if (item == null)
            {
                return 0L;
            }

            var sqliteItem = new SQLiteInsert<T>(item);
            this._writeQueue.Enqueue(sqliteItem);
            sqliteItem.WaitOne();
            if (sqliteItem.ExcuteResult)
            {
                return Convert.ToInt32(sqliteItem.Result);
            }
            else
            {
                throw sqliteItem.Result as Exception;
            }
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
        public override long BatchInsert(string tableName, IEnumerable<string> cols, IEnumerable<object[]> data)
        {
            if (cols == null || cols.Count() == 0 || data == null || data.Count() == 0)
            {
                return 0L;
            }

            //验证批量插入数据参数
            this.ValidateBatchInsert(tableName, cols, data);

            var item = new SQLiteInsert(tableName, cols, data);
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
        /// 批量插入数据,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collections">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        public override long BatchInsert(string sqlStr, IEnumerable<NDbParameterCollection> collections)
        {
            if (string.IsNullOrWhiteSpace(sqlStr))
            {
                return 0;
            }

            var item = new SQLiteInsert(sqlStr, collections);
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
        /// 批量插入泛型T类型对象数据到数据到库,返回受影响的行数,仅支持单表
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="items">插入项集合</param>
        /// <returns>返回受影响的行数</returns>
        public override long BatchInsertT<T>(IEnumerable<T> items)
        {
            if (items == null || items.Count() == 0)
            {
                throw new ArgumentNullException("items");
            }

            var item = new SQLiteInsert<T>(items);
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
