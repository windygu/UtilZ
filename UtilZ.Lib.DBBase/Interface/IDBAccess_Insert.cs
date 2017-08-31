using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Lib.DBModel.Model;

namespace UtilZ.Lib.DBBase.Interface
{
    //数据库访问接口-插入数据
    public partial interface IDBAccess
    {
        #region 单项插入
        /// <summary>
        /// 插入数据,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="parameters">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        long Insert(string sqlStr, NDbParameterCollection parameters = null);

        /// <summary>
        /// 插入泛型T类型对象数据到数据到库,返回受影响的行数,仅支持单表
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="item">插入项</param>
        /// <returns>返回受影响的行数</returns>
        long InsertT<T>(T item) where T : class;
        #endregion

        #region 批量插入
        /// <summary>
        /// 批量插入数据,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dt">DataTable</param>
        /// <returns>返回受影响的行数</returns>
        long BatchInsert(string tableName, DataTable dt);

        /// <summary>
        /// 批量插入数据,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols">列名集合</param>
        /// <param name="data">数据</param>
        /// <returns>返回受影响的行数</returns>
        long BatchInsert(string tableName, IEnumerable<string> cols, IEnumerable<object[]> data);

        /// <summary>
        /// 批量插入数据,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collections">命令的参数集合</param>
        /// <param name="con">数据库连接对象</param>
        /// <returns>返回受影响的行数</returns>
        long BatchInsert(string sqlStr, IEnumerable<NDbParameterCollection> collections);

        /// <summary>
        /// 批量插入泛型T类型对象数据到数据到库,返回受影响的行数,仅支持单表
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="items">插入项集合</param>
        /// <returns>返回受影响的行数</returns>
        long BatchInsertT<T>(IEnumerable<T> items) where T : class;
        #endregion
    }
}
