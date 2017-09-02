﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Lib.DBModel.Model;

namespace UtilZ.Lib.DBSqlite.Core
{
    /// <summary>
    /// SQLite数据库访问类-查询
    /// </summary>
    public partial class SQLiteDBAccess
    {
        #region sql语句分页查询数据
        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="sqlStr">查询SQL语句</param>
        /// <param name="orderInfos">排序列名集合[null为或空不排序]</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">查询页索引</param>
        /// <param name="orderFlag">排序类型[true:升序;false:降序]</param>
        /// <param name="collection">命令的参数集合</param>
        /// <param name="priKeyCols">主键集合[当为oracle时此值不能为空或null,否则查询出的结果可能出现重复,因为Oracle数据的分页查询中排序规则不稳定]</param>
        /// <returns>数据表</returns>
        public override DataTable QueryPagingData(string sqlStr, IEnumerable<DBOrderInfo> orderInfos, int pageSize, int pageIndex, bool orderFlag, NDbParameterCollection collection = null, IEnumerable<string> priKeyCols = null)
        {
            string pageSql = this.SplicedPagingSql(sqlStr, orderInfos, pageIndex, pageSize);
            return this.QueryData(pageSql, collection);

            //var dbItem = DatabaseConfig.GetDBConInfoByDBID(dbid);
            //string pagingAssistColName = null;//分页辅助列列名
            //string pageSql = null;
            //DataTable dt = null;
            //switch (dbItem.Type)
            //{
            //    case DataBaseType.SQLServer:
            //        pagingAssistColName = this.GetPagingAssistColName(sqlStr);
            //        pageSql = PagingSqlHelper.SplicedSqlServerSql(sqlStr, orderInfos, pageIndex, pageSize, pagingAssistColName);
            //        dt = this.QueryData(dbid, pageSql, collection);
            //        break;
            //    case DataBaseType.DB2:
            //        throw new NotSupportedException(string.Format("数据库类型:{0}不支持", dbItem.Type));
            //    case DataBaseType.MySQL:
            //        pageSql = PagingSqlHelper.SplicedMySQLSql(sqlStr, orderInfos, pageIndex, pageSize);
            //        dt = this.QueryData(dbid, pageSql, collection);
            //        break;
            //    case DataBaseType.Oracle:
            //        pagingAssistColName = this.GetPagingAssistColName(sqlStr);
            //        pageSql = PagingSqlHelper.SplicedOracleSql(sqlStr, orderInfos, pageIndex, pageSize, pagingAssistColName, priKeyCols);
            //        //查询数据
            //        dt = this.QueryData(dbid, pageSql, collection);
            //        break;
            //    case DataBaseType.SQLite:
            //        pageSql = PagingSqlHelper.SplicedSQLiteSql(sqlStr, orderInfos, pageIndex, pageSize);
            //        //查询数据
            //        dt = this.QueryData(dbid, pageSql, collection);
            //        break;
            //    default:
            //        throw new NotSupportedException(string.Format("数据库类型:{0}不支持", dbItem.Type));
            //}

            ////移除分页辅助列
            //if (!string.IsNullOrEmpty(pagingAssistColName) && dt.Columns.Contains(pagingAssistColName))
            //{
            //    dt.Columns.Remove(pagingAssistColName);
            //}

            //return dt;
        }

        /// <summary>
        ///  拼接分页SQL语句
        /// </summary>
        /// <param name="sqlStr">原SQL语句</param>
        /// <param name="orderInfos">排序列名集合[null为或空不排序]</param>
        /// <param name="pageIndex">查询页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>分页SQL语句</returns>
        private string SplicedPagingSql(string sqlStr, IEnumerable<DBOrderInfo> orderInfos, int pageIndex, int pageSize)
        {
            //参数断言
            this.SQLAssert("*", pageIndex, pageSize);

            //eg:SELECT * from person WHERE ID < 100 ORDER by ID DESC limit 0,10
            int startIndex = (pageIndex - 1) * pageSize;
            string dstSqlStr = null;
            if (orderInfos == null || orderInfos.Count() == 0)
            {
                dstSqlStr = string.Format("{0} limit {1},{2}", sqlStr, startIndex, pageSize);
            }
            else
            {
                string orderStr = this.CreateOrderStr(orderInfos, null);
                dstSqlStr = string.Format("{0} ORDER BY {1} limit {2},{3}", sqlStr, orderStr, startIndex, pageSize);
            }

            return dstSqlStr;
        }
        #endregion
    }
}