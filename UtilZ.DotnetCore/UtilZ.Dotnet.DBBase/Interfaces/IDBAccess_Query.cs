using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBIBase.DBModel.Model;

namespace UtilZ.Dotnet.DBBase.Interfaces
{
    //数据库访问接口-查询
    public partial interface IDBAccess
    {
        #region 查询页数
        /// <summary>
        /// 查询分页信息
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>分页信息</returns>
        DBPageInfo QueryPageInfo(int pageSize, string sqlStr, NDbParameterCollection collection = null);
        #endregion

        #region 基础查询
        /// <summary>
        /// 执行SQL语句,返回查询DataSet
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回执行结果</returns>
        DataSet QueryDataSet(string sqlStr, NDbParameterCollection collection = null);

        /// <summary>
        /// 执行SQL语句,返回查询结果
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回执行结果</returns>
        DataTable QueryData(string sqlStr, NDbParameterCollection collection = null);
        #endregion

        #region sql语句分页查询数据
        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="sqlStr">查询SQL语句</param>
        /// <param name="orderByColName">排序列名</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">查询页索引</param>
        /// <param name="orderFlag">排序类型[true:升序;false:降序]</param>
        /// <param name="collection">命令的参数集合</param>
        /// <param name="priKeyCols">主键集合[当为oracle时此值不能为空或null,否则查询出的结果可能出现重复,因为Oracle数据的分页查询中排序规则不稳定]</param>
        /// <returns>数据表</returns>
        DataTable QueryPagingData(string sqlStr, string orderByColName, int pageSize, int pageIndex, bool orderFlag, NDbParameterCollection collection = null, IEnumerable<string> priKeyCols = null);

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
        DataTable QueryPagingData(string sqlStr, IEnumerable<DBOrderInfo> orderInfos, int pageSize, int pageIndex, bool orderFlag, NDbParameterCollection collection = null, IEnumerable<string> priKeyCols = null);
        #endregion
    }
}
