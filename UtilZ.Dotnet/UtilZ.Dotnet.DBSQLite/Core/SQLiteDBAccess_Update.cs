using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBModel.Model;
using UtilZ.Dotnet.DBSQLite.Write;

namespace UtilZ.Dotnet.DBSQLite.Core
{
    //SQLite数据库访问类-更新
    public partial class SQLiteDBAccess
    {
        #region 单项更新
        /// <summary>
        /// 更新记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColValues">主键列名值映射字典</param>
        /// <param name="colValues">修改列名值字典</param>
        /// <returns>返回受影响的行数</returns>
        public override long Update(string tableName, Dictionary<string, object> priKeyColValues, Dictionary<string, object> colValues)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("tableName");
            }

            var item = new SQLiteUpdate(this._waitTimeout, tableName, priKeyColValues, colValues);
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
        /// 更新记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        public override long Update(string sqlStr, NDbParameterCollection collection = null)
        {
            if (string.IsNullOrWhiteSpace(sqlStr))
            {
                throw new ArgumentNullException("sqlStr");
            }

            var item = new SQLiteUpdate(this._waitTimeout, sqlStr, collection);
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
        /// 更新泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="item">对象</param>
        /// <param name="updateProperties">要更新到数据库表的属性名称集合,为null时全部更新</param>
        /// <returns>返回受影响的行数</returns>
        public override long UpdateT<T>(T item, IEnumerable<string> updateProperties = null)
        {
            if (item == null)
            {
                return 0;
            }

            if (updateProperties != null && updateProperties.Count() == 0)
            {
                throw new ArgumentException("要更新到表的属性名称集合当不为null时元素不能为空", "updateProperties");
            }

            var updateItem = new SQLiteUpdateT<T>(this._waitTimeout, updateProperties, item);
            this._writeQueue.Enqueue(updateItem);
            updateItem.WaitOne();
            if (updateItem.ExcuteResult)
            {
                return Convert.ToInt32(updateItem.Result);
            }
            else
            {
                throw updateItem.Result as Exception;
            }
        }
        #endregion

        #region 批量更新
        /// <summary>
        /// 批量更新记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collections">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        public override long BatchUpdate(string sqlStr, IEnumerable<NDbParameterCollection> collections)
        {
            if (string.IsNullOrWhiteSpace(sqlStr))
            {
                throw new ArgumentNullException("sqlStr");
            }

            var item = new SQLiteUpdate(this._waitTimeout, sqlStr, collections);
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
        /// 批量更新记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStrs">Sql语句集合</param>
        /// <returns>返回受影响的行数</returns>
        public override long BatchUpdate(IEnumerable<string> sqlStrs)
        {
            if (sqlStrs == null || sqlStrs.Count() == 0)
            {
                return 0L;
            }

            var item = new SQLiteUpdate(this._waitTimeout, sqlStrs);
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
        /// 批量更新泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="items">要更新的对象集合</param>
        /// <param name="updateProperties">要更新到数据库表的属性名称集合,为null时全部更新</param>
        /// <returns>返回受影响的行数</returns>
        public override long BatchUpdateT<T>(IEnumerable<T> items, IEnumerable<string> updateProperties = null)
        {
            if (items == null || items.Count() == 0)
            {
                return 0L;
            }

            if (updateProperties != null && updateProperties.Count() == 0)
            {
                throw new ArgumentException("要更新到表的属性名称集合当不为null时元素不能为空", "updateProperties");
            }

            var updateItem = new SQLiteUpdateT<T>(this._waitTimeout, updateProperties, items);
            this._writeQueue.Enqueue(updateItem);
            updateItem.WaitOne();
            if (updateItem.ExcuteResult)
            {
                return Convert.ToInt32(updateItem.Result);
            }
            else
            {
                throw updateItem.Result as Exception;
            }
        }
        #endregion
    }
}
