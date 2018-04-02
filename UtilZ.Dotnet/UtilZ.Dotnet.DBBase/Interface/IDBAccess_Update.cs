using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBModel.Model;

namespace UtilZ.Dotnet.DBBase.Interface
{
    //数据库访问接口-更新
    public partial interface IDBAccess
    {
        #region 单项更新
        /// <summary>
        /// 更新记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColName">主键名</param>
        /// <param name="priKeyValue">主键值</param>
        /// <param name="colName">修改列名</param>
        /// <param name="value">修改列值</param>
        /// <returns>返回受影响的行数</returns>
        long Update(string tableName, string priKeyColName, object priKeyValue, string colName, object value);

        /// <summary>
        /// 更新记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColName">主键名</param>
        /// <param name="priKeyValue">主键值</param>
        /// <param name="colValues">列名值字典</param>
        /// <returns>返回受影响的行数</returns>
        long Update(string tableName, string priKeyColName, object priKeyValue, Dictionary<string, object> colValues);

        /// <summary>
        /// 更新记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColValues">主键列名值映射字典</param>
        /// <param name="colValues">修改列名值字典</param>
        /// <returns>返回受影响的行数</returns>
        long Update(string tableName, Dictionary<string, object> priKeyColValues, Dictionary<string, object> colValues);

        /// <summary>
        /// 更新记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        long Update(string sqlStr, NDbParameterCollection collection = null);

        /// <summary>
        /// 更新泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="item">对象</param>
        /// <param name="updateProperties">要更新到数据库表的属性名称集合,为null时全部更新</param>
        /// <returns>返回受影响的行数</returns>
        long UpdateT<T>(T item, IEnumerable<string> updateProperties = null) where T : class;
        #endregion

        #region 批量更新
        /// <summary>
        /// 批量更新记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collections">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        long BatchUpdate(string sqlStr, IEnumerable<NDbParameterCollection> collections = null);

        /// <summary>
        /// 批量更新记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStrs">Sql语句集合</param>
        /// <returns>返回受影响的行数</returns>
        long BatchUpdate(IEnumerable<string> sqlStrs);

        /// <summary>
        /// 批量更新泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="items">要更新的对象集合</param>
        /// <param name="updateProperties">要更新到数据库表的属性名称集合,为null时全部更新</param>
        /// <returns>返回受影响的行数</returns>
        long BatchUpdateT<T>(IEnumerable<T> items, IEnumerable<string> updateProperties = null) where T : class;
        #endregion
    }
}
