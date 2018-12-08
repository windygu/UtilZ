using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBBase.Core;
using UtilZ.Dotnet.DBBase.Model;
using UtilZ.Dotnet.DBIBase.DBModel.Common;
using UtilZ.Dotnet.DBIBase.DBModel.DBInfo;
using UtilZ.Dotnet.DBIBase.DBModel.Model;

namespace UtilZ.Dotnet.DBOracle.Core
{
    //SQLServer数据库访问类-查询
    public partial class OracleDBAccess
    {
        #region 分页查询数据
        /// <summary>
        /// 创建分页查询语句
        /// </summary>
        /// <param name="sqlStr">查询SQL语句</param>
        /// <param name="orderByColName">排序列名</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">查询页索引</param>
        /// <param name="orderFlag">排序类型[true:升序;false:降序]</param>
        /// <returns>分页查询语句</returns>
        public override string CreatePagingQuerySql(string sqlStr, string orderByColName, int pageSize, int pageIndex, bool orderFlag, IEnumerable<string> priKeyCols)
        {
            string pagingAssistColName = this.GetPagingAssistColName(sqlStr);//分页辅助列列名
            return this.SplicedOracleSql(sqlStr, new DBOrderInfo[] { new DBOrderInfo(orderByColName, orderFlag) }, pageIndex, pageSize, pagingAssistColName, priKeyCols);
        }

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
            string pagingAssistColName = this.GetPagingAssistColName(sqlStr);//分页辅助列列名
            string pageSql = this.SplicedOracleSql(sqlStr, orderInfos, pageIndex, pageSize, pagingAssistColName, priKeyCols);
            DataTable dt = this.QueryData(pageSql, collection);
            if (dt.Columns.Contains(pagingAssistColName))
            {
                dt.Columns.Remove(pagingAssistColName);
            }

            return dt;
        }

        /// <summary>
        /// 拼接Oracle分页语句
        /// </summary>
        /// <param name="sqlStr">原SQL语句</param>
        /// <param name="orderInfos">排序列名集合[null为或空不排序]</param>
        /// <param name="pageIndex">查询页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="WQS_RNCol">分页辅助列列名</param>
        /// <param name="priKeyCols">主键列集合[因为Oracle数据的分页查询中排序规则不稳定,所以需要根据主键再来一次排序,此值为空或null则只根据排序列排序]</param>
        /// <returns>分页SQL语句</returns>
        private string SplicedOracleSql(string sqlStr, IEnumerable<DBOrderInfo> orderInfos, int pageIndex, int pageSize, string WQS_RNCol, IEnumerable<string> priKeyCols)
        {
            //eg:SELECT * FROM (SELECT WQS_T.*,ROWNUM AS PACNRN FROM (SELECT *  FROM WX_BGAN_GPS WHERE STARTTIME>=:para1 AND STARTTIME<=:para2 ORDER BY STARTTIME DESC,ID DESC) WQS_T WHERE ROWNUM<13) WHERE PACNRN>=10
            //参数验证
            base.InvalidateSQL(sqlStr, pageIndex, pageSize);

            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = startIndex + pageSize;
            string dstSqlStr = null;
            if (orderInfos == null || orderInfos.Count() == 0)
            {
                dstSqlStr = string.Format(@"SELECT * FROM (SELECT WQS_T.*,ROWNUM AS {0} FROM ({1}) WQS_T WHERE ROWNUM<{2}) WHERE {3}>={4}",
                WQS_RNCol, sqlStr, endIndex, WQS_RNCol, startIndex);
            }
            else
            {
                string orderStr = this.CreateOrderStr(orderInfos, priKeyCols);
                dstSqlStr = string.Format(@"SELECT * FROM (SELECT WQS_T.*,ROWNUM AS {0} FROM ({1} ORDER BY {2}) WQS_T WHERE ROWNUM<{3}) WHERE {4}>={5}",
                WQS_RNCol, sqlStr, orderStr, endIndex, WQS_RNCol, startIndex);
            }

            return dstSqlStr;
        }

        /// <summary>
        /// 创建排序字符串
        /// </summary>
        /// <param name="orderInfos">排序列名集合</param>
        /// <param name="priKeyCols">主键列集合</param>
        /// <returns>排序字符串</returns>
        protected override string CreateOrderStr(IEnumerable<DBOrderInfo> orderInfos, IEnumerable<string> priKeyCols)
        {
            if (orderInfos == null || orderInfos.Count() == 0)
            {
                return "1";
            }

            //排序列
            StringBuilder sbOrder = new StringBuilder();
            foreach (var orderInfo in orderInfos)
            {
                sbOrder.Append(orderInfo.FieldName);
                sbOrder.Append(" ");
                sbOrder.Append(orderInfo.OrderFlag ? "ASC" : "DESC");
                sbOrder.Append(",");
            }

            //根据主键排序
            if (priKeyCols != null && priKeyCols.Count() > 0)
            {
                foreach (var priKeyCol in priKeyCols)
                {
                    sbOrder.Append(priKeyCol);
                    sbOrder.Append(" ");
                    sbOrder.Append("DESC");
                    sbOrder.Append(",");
                }
            }

            //移除最后一个
            sbOrder = sbOrder.Remove(sbOrder.Length - 1, 1);
            return sbOrder.ToString();
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
