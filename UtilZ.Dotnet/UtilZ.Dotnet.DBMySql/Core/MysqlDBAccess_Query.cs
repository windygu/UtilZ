using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBIBase.DBModel.Model;

namespace UtilZ.Dotnet.DBMySql.Core
{
    //Mysql数据库访问类-查询
    public partial class MySqlDBAccess
    {
        #region 分页查询数据
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
            string pageSql = this.SplicedMySQLSql(sqlStr, orderInfos, pageIndex, pageSize);
            return this.QueryData(pageSql, collection);
        }

        #region 拼接MySQL分页语句
        /// <summary>
        /// 拼接MySQL分页语句
        /// </summary>
        /// <param name="sqlStr">原SQL语句</param>
        /// <param name="orderByColName">排序列名[为空或null不排序]</param>
        /// <param name="orderFlag">排序类型[true:升序;false:降序]</param>
        /// <param name="pageIndex">查询页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>分页SQL语句</returns>
        private string SplicedMySQLSql(string sqlStr, string orderByColName, bool orderFlag, int pageIndex, int pageSize)
        {
            return this.SplicedMySQLSql(sqlStr, new List<DBOrderInfo> { new DBOrderInfo(orderByColName, orderFlag) }, pageIndex, pageSize);
        }

        /// <summary>
        /// 拼接MySQL分页语句
        /// </summary>
        /// <param name="sqlStr">原SQL语句</param>
        /// <param name="orderByColNames">排序列名集合[null为或空不排序]</param>
        /// <param name="pageIndex">查询页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>分页SQL语句</returns>
        private string SplicedMySQLSql(string sqlStr, IEnumerable<DBOrderInfo> orderByColNames, int pageIndex, int pageSize)
        {
            //参数断言
            this.SQLAssert(sqlStr, pageIndex, pageSize);

            string lowerSqlStr = sqlStr.ToLower();
            int selectStrLenth = "select".Length;
            int fromStrLenth = "from".Length;
            int fromStrIndex = lowerSqlStr.IndexOf("from");
            string queryColStr = sqlStr.Substring(selectStrLenth, fromStrIndex - selectStrLenth).Trim();
            string tableName = sqlStr.Substring(fromStrIndex + fromStrLenth).TrimStart();
            string whereSql = null;
            if (tableName.StartsWith("("))//原SQL语句的最外层的from后面为子查询结果
            {
                //eg:select * from (select * from Person WHERE ID<100) T
                whereSql = null;//条件语句为null
            }
            else
            {
                int spaceIndex = tableName.IndexOf(" ");
                if (spaceIndex == -1)
                {
                    //eg:select * from Person
                    whereSql = null;//条件语句为null
                }
                else
                {
                    string whereStr = tableName.Substring(spaceIndex).TrimStart();
                    tableName = tableName.Substring(0, spaceIndex);
                    if (string.IsNullOrEmpty(whereStr))
                    {
                        //理论上说来是不会出现这种情况的,因为如果进了这个分支,则说明SQL语句会和spaceIndex=-1是同一种情况
                        //但是为了健壮性,加上了这种不太可能的逻辑处理
                        //eg:select * from Person
                        whereSql = null;//条件语句为null
                    }
                    else
                    {
                        //eg:select * from Person WHERE ID<100
                        whereSql = whereStr.Substring("where".Length).TrimStart();
                    }
                }
            }

            return this.SplicedMySQLSql(tableName, queryColStr, whereSql, orderByColNames, pageIndex, pageSize);
        }

        /// <summary>
        /// 拼接MySQL分页语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="queryCols">查询列集合</param>
        /// <param name="whereSql">条件语句</param>
        /// <param name="orderByColName">排序列名[为空或null不排序]</param>
        /// <param name="orderFlag">排序类型[true:升序;false:降序]</param>
        /// <param name="pageIndex">查询页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>分页SQL语句</returns>
        private string SplicedMySQLSql(string tableName, IEnumerable<string> queryCols, string whereSql, string orderByColName, bool orderFlag, int pageIndex, int pageSize)
        {
            return this.SplicedMySQLSql(tableName, queryCols, whereSql, new List<DBOrderInfo> { new DBOrderInfo(orderByColName, orderFlag) }, pageIndex, pageSize);
        }

        /// <summary>
        /// 拼接MySQL分页语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="queryCols">查询列集合</param>
        /// <param name="whereSql">条件语句</param>
        /// <param name="orderByColNames">排序列名集合[null为或空不排序]</param>
        /// <param name="pageIndex">查询页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>分页SQL语句</returns>
        private string SplicedMySQLSql(string tableName, IEnumerable<string> queryCols, string whereSql, IEnumerable<DBOrderInfo> orderByColNames, int pageIndex, int pageSize)
        {
            string queryColStr = null;
            if (queryCols == null || queryCols.Count() == 0)
            {
                queryColStr = "*";
            }
            else
            {
                queryColStr = string.Join(",", queryCols);
            }

            return this.SplicedMySQLSql(tableName, queryColStr, whereSql, orderByColNames, pageIndex, pageSize);
        }

        /// <summary>
        /// 拼接MySQL分页语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="queryColStr">查询列字符串,多个列之间通过逗号分隔</param>
        /// <param name="whereSql">条件语句</param>
        /// <param name="orderByColName">排序列名[为空或null不排序]</param>
        /// <param name="orderFlag">排序类型[true:升序;false:降序]</param>
        /// <param name="pageIndex">查询页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>分页SQL语句</returns>
        private string SplicedMySQLSql(string tableName, string queryColStr, string whereSql, string orderByColName, bool orderFlag, int pageIndex, int pageSize)
        {
            return this.SplicedMySQLSql(tableName, queryColStr, whereSql, new List<DBOrderInfo> { new DBOrderInfo(orderByColName, orderFlag) }, pageIndex, pageSize);
        }

        /// <summary>
        /// 拼接MySQL分页语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="queryColStr">查询列字符串,多个列之间通过逗号分隔</param>
        /// <param name="whereSql">条件语句</param>
        /// <param name="orderInfos">排序列名集合[null为或空不排序]</param>
        /// <param name="pageIndex">查询页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>分页SQL语句</returns>
        private string SplicedMySQLSql(string tableName, string queryColStr, string whereSql, IEnumerable<DBOrderInfo> orderInfos, int pageIndex, int pageSize)
        {
            //SELECT SQL_NO_CACHE SQL_CALC_FOUND_ROWS NAME,AGE FROM Person WHERE ID>100 order by ID ASC limit 0,20;           
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("查询表名不能为空或null", "tableName");
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT SQL_NO_CACHE SQL_CALC_FOUND_ROWS ");
            sb.Append(queryColStr);
            sb.Append(" FROM ");
            sb.Append(tableName);
            if (!string.IsNullOrEmpty(whereSql))
            {
                sb.Append(" WHERE ");
                sb.Append(whereSql);
            }

            if (orderInfos != null && orderInfos.Count() > 0)
            {
                sb.Append(" order by ");
                sb.Append(this.CreateOrderStr(orderInfos, null));
            }

            sb.Append(" limit ");
            sb.Append((pageIndex - 1) * pageSize);
            sb.Append(",");
            sb.Append(pageSize);

            return sb.ToString();
        }
        #endregion
        #endregion
    }
}
