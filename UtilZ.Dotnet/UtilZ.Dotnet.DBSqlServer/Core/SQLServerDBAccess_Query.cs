using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBModel.Model;

namespace UtilZ.Dotnet.DBSqlServer.Core
{
    //SQLServer数据库访问类-查询
    public partial class SQLServerDBAccess
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
        public override DataTable QueryPagingData( string sqlStr, IEnumerable<DBOrderInfo> orderInfos, int pageSize, int pageIndex, bool orderFlag, NDbParameterCollection collection = null, IEnumerable<string> priKeyCols = null)
        {
            string pagingAssistColName = this.GetPagingAssistColName(sqlStr);//分页辅助列列名
            string pageSql = this.SplicedSqlServerSql(sqlStr, orderInfos, pageIndex, pageSize, pagingAssistColName);
            DataTable dt = this.QueryData(pageSql, collection);
            if (dt.Columns.Contains(pagingAssistColName))
            {
                dt.Columns.Remove(pagingAssistColName);
            }

            return dt;
        }

        /// <summary>
        /// 拼接SQL Server分页语句
        /// </summary>
        /// <param name="sqlStr">原SQL语句</param>
        /// <param name="orderInfos">排序列名集合[null为或空不排序]</param>
        /// <param name="pageIndex">数据起始位置</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pagingAssistColName">分页辅助列列名</param>
        /// <returns>分页SQL语句</returns>
        private string SplicedSqlServerSql(string sqlStr, IEnumerable<DBOrderInfo> orderInfos, int pageIndex, int pageSize, string pagingAssistColName)
        {
            //参数断言
            this.SQLAssert(sqlStr, pageIndex, pageSize);
            //eg:

            if (orderInfos == null || orderInfos.Count() == 0)
            {
                orderInfos = new List<DBOrderInfo> { new DBOrderInfo("(select 0)", false) };
            }

            string orderStr = this.CreateOrderStr(orderInfos, null);
            string lowerSqlStr = sqlStr.ToLower();
            string selectStr = "select";
            string ronumSqlStr = sqlStr.Insert(lowerSqlStr.IndexOf(selectStr) + selectStr.Length, string.Format(" (ROW_NUMBER() OVER (ORDER BY {0})) AS {1},", orderStr, pagingAssistColName));
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM (");
            sb.Append(ronumSqlStr);
            sb.Append(string.Format(") AS WQS_T WHERE {0} BETWEEN ", pagingAssistColName));
            int startIndex = (pageIndex - 1) * pageSize;
            sb.Append(startIndex);
            sb.Append(" AND ");
            sb.Append(startIndex + pageSize);
            return sb.ToString();

            /*
           declare @pageIndex int
           declare @pageSize int
           set @pageIndex = 1
           set @pageSize = 10
           select * from person order by ID asc OFFSET (@pageSize * (@pageIndex-1)) ROW FETCH NEXT @pageSize rows only;
            */
            //SQLServer2012+新特性,在高并发时性能强于ROW_NUMBER方式
            //return string.Format("{0} order by {1} {2} OFFSET {3} ROW FETCH NEXT {4} rows only", sqlStr, orderByColName, orderFlag ? "ASC" : "DESC", (pageIndex - 1) * pageSize, pageSize);
        }

        /// <summary>
        /// 获取分页辅助列名
        /// </summary>
        /// <param name="sqlStr">查询SQL语句</param>
        /// <returns>分页辅助列名</returns>
        private string GetPagingAssistColName(string sqlStr)
        {
            string pagingAssistColName = "PACNRN";
            int index = 1;
            while (sqlStr.Contains(pagingAssistColName))
            {
                pagingAssistColName = "PACNRN" + index.ToString();
            }

            return pagingAssistColName;//分页辅助列列名
        }
        #endregion
    }
}
