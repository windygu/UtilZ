using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBModel.Model;

namespace UtilZ.Dotnet.DBBase.Interface
{
    //数据库访问接口-删除
    public partial interface IDBAccess
    {
        #region 单项删除
        /// <summary>
        /// 删除记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        long Delete(string sqlStr, NDbParameterCollection collection = null);

        /// <summary>
        /// 删除记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColName">主键名</param>
        /// <param name="value">主键值</param>
        /// <returns>返回受影响的行数</returns>
        long Delete(string tableName, string priKeyColName, object value);

        /// <summary>
        /// 删除记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColValues">主键列名值映射字典</param>
        /// <returns>返回受影响的行数</returns>
        long Delete(string tableName, Dictionary<string, object> priKeyColValues);

        /// <summary>
        /// 删除泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="item">要删除的对象</param>
        /// <param name="conditionProperties">条件属性集合[该集合为空或null时仅用主键字段]</param>
        /// <returns>返回受影响的行数</returns>
        long DeleteT<T>(T item, IEnumerable<string> conditionProperties = null) where T : class;
        #endregion

        #region 批量删除
        /// <summary>
        /// 批量删除记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStrs">Sql语句集合</param>
        /// <returns>返回受影响的行数</returns>
        long BatchDelete(IEnumerable<string> sqlStrs);

        /// <summary>
        /// 批量删除记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColValues">主键列名值映射字典</param>
        /// <returns>返回受影响的行数</returns>
        long BatchDelete(string tableName, IEnumerable<Dictionary<string, object>> priKeyColValues);

        /// <summary>
        /// 批量删除记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collections">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        long BatchDelete(string sqlStr, IEnumerable<NDbParameterCollection> collections);

        /// <summary>
        /// 批量删除泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="items">要删除的集合</param>
        /// <returns>返回受影响的行数</returns>
        long BatchDeleteT<T>(IEnumerable<T> items) where T : class;
        #endregion
    }
}
