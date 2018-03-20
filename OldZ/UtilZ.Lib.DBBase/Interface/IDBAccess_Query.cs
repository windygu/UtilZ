using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Lib.DBModel.Model;

namespace UtilZ.Lib.DBBase.Interface
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

        /// <summary>
        /// 查询分页信息
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="pageSize">页大小</param>
        /// <param name="query">查询对象[为null时无条件查询]</param>
        /// <param name="conditionProperties">条件属性集合[该集合为空或null时仅用主键字段]</param>
        /// <returns>分页信息</returns>
        DBPageInfo QueryPageInfoT<T>(int pageSize, T query = null, IEnumerable<string> conditionProperties = null) where T : class, new();
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

        #region 泛型
        /// <summary>
        /// 查询数据并返回List泛型集合
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <returns>数据集合</returns>
        List<T> QueryT<T>(string sqlStr, NDbParameterCollection collection = null) where T : class, new();

        /// <summary>
        /// 查询数据并返回List泛型集合
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <returns>数据集合</returns>
        List<T> QueryT<T>() where T : class, new();

        /// <summary>
        /// 查询数据并返回List泛型集合
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="query">查询对象[为null时无条件查询]</param>
        /// <param name="conditionProperties">条件属性集合[该集合为空或null时仅用主键字段]</param>
        /// <returns>数据集合</returns>
        List<T> QueryT<T>(T query, IEnumerable<string> conditionProperties) where T : class, new();

        /// <summary>
        /// 查询数据并返回List泛型集合
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="query">查询对象[为null时无条件查询]</param>
        /// <param name="conditionProperties">条件属性集合[该集合为空或null时仅用主键字段]</param>
        /// <param name="queryProperties">要查询的列集合[该集合为空或null时查询全部字段]</param>
        /// <returns>数据集合</returns>
        List<T> QueryT<T>(T query, IEnumerable<string> conditionProperties, IEnumerable<string> queryProperties) where T : class, new();

        /// <summary>
        /// 查询数据并返回List泛型集合
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="query">查询对象[为null时无条件查询]</param>
        /// <param name="conditionProperties">条件属性集合[该集合为空或null时仅用主键字段]</param>
        /// <param name="queryProperties">要查询的列集合[该集合为空或null时查询全部字段]</param>
        /// <param name="orderProperty">排序属性名称[为null不排序]</param>
        /// <param name="orderFlag">排序类型[true:升序;false:降序,默认false]</param>
        /// <returns>数据集合</returns>
        List<T> QueryT<T>(T query = null, IEnumerable<string> conditionProperties = null, IEnumerable<string> queryProperties = null, string orderProperty = null, bool orderFlag = false) where T : class, new();

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="sqlStr">查询SQL语句</param>
        /// <param name="orderByColName">排序列名</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">查询页索引</param>
        /// <param name="orderFlag">排序类型[true:升序;false:降序]</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>数据表</returns>
        List<T> QueryTPaging<T>(string sqlStr, string orderByColName, int pageSize, int pageIndex, bool orderFlag, NDbParameterCollection collection = null) where T : class, new();

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="sqlStr">查询SQL语句</param>
        /// <param name="orderInfos">排序列名集合[null为或空不排序]</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">查询页索引</param>
        /// <param name="orderFlag">排序类型[true:升序;false:降序]</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>数据表</returns>
        List<T> QueryTPaging<T>(string sqlStr, IEnumerable<DBOrderInfo> orderInfos, int pageSize, int pageIndex, bool orderFlag, NDbParameterCollection collection = null) where T : class, new();

        /// <summary>
        /// 查询数据并返回List泛型集合,带分页
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">查询页索引</param>
        /// <param name="orderProperty">排序属性名称[为null不排序]</param>
        /// <param name="orderFlag">排序类型[true:升序;false:降序,默认false]</param>
        /// <returns>数据集合</returns>
        List<T> QueryTPaging<T>(int pageSize, int pageIndex, string orderProperty = null, bool orderFlag = false) where T : class, new();

        /// <summary>
        /// 查询数据并返回List泛型集合,带分页
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">查询页索引</param>
        /// <param name="query">查询对象[为null时无条件查询]</param>
        /// <param name="conditionProperties">条件属性集合[该集合为空或null时仅用主键字段]</param>
        /// <param name="orderProperty">排序属性名称[为null不排序]</param>
        /// <param name="orderFlag">排序类型[true:升序;false:降序,默认false]</param>
        /// <returns>数据集合</returns>
        List<T> QueryTPaging<T>(int pageSize, int pageIndex, T query, IEnumerable<string> conditionProperties, string orderProperty = null, bool orderFlag = false) where T : class, new();

        /// <summary>
        /// 查询数据并返回List泛型集合,带分页
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">查询页索引</param>
        /// <param name="query">查询对象[为null时无条件查询]</param>
        /// <param name="conditionProperties">条件属性集合[该集合为空或null时仅用主键字段]</param>
        /// <param name="queryProperties">要查询的列集合[该集合为空或null时查询全部字段]</param>
        /// <param name="orderProperty">排序属性名称[为null不排序]</param>
        /// <param name="orderFlag">排序类型[true:升序;false:降序,默认false]</param>
        /// <returns>数据集合</returns>
        List<T> QueryTPaging<T>(int pageSize, int pageIndex, T query = null, IEnumerable<string> conditionProperties = null, IEnumerable<string> queryProperties = null, string orderProperty = null, bool orderFlag = false) where T : class, new();

        /// <summary>
        /// 查询数据并返回List泛型集合
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="query">查询对象[为null时无条件查询]</param>
        /// <param name="conditionProperties">条件属性集合[该集合为空或null时仅用主键字段]</param>
        /// <param name="queryProperties">要查询的列集合[该集合为空或null时查询全部字段]</param>
        /// <param name="orderInfos">排序列名[为空或null不排序]</param>
        /// <param name="orderFlag">排序类型[true:升序;false:降序,默认false]</param>
        /// <returns>数据集合</returns>
        List<T> QueryT<T>(T query = null, IEnumerable<string> conditionProperties = null, IEnumerable<string> queryProperties = null, IEnumerable<DBOrderInfo> orderInfos = null, bool orderFlag = false) where T : class, new();

        /// <summary>
        /// 查询数据并返回List泛型集合,带分页
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">查询页索引</param>
        /// <param name="query">查询对象[为null时无条件查询]</param>
        /// <param name="conditionProperties">条件属性集合[该集合为空或null时仅用主键字段]</param>
        /// <param name="queryProperties">要查询的列集合[该集合为空或null时查询全部字段]</param>
        /// <param name="orderInfos">排序列名[为空或null不排序]</param>
        /// <param name="orderFlag">排序类型[true:升序;false:降序,默认false]</param>
        /// <returns>数据集合</returns>
        List<T> QueryTPaging<T>(int pageSize, int pageIndex, T query = null, IEnumerable<string> conditionProperties = null, IEnumerable<string> queryProperties = null, IEnumerable<DBOrderInfo> orderInfos = null, bool orderFlag = false) where T : class, new();


        #endregion

        /*
      #region 泛型
      #region 不分页
      /// <summary>
      /// 查询数据并返回List泛型集合
      /// </summary>
      /// <typeparam name="T">数据模型类型</typeparam>
      /// <returns>数据集合</returns>
      List<T> Query<T>() where T : new();

      /// <summary>
      /// 查询数据并返回List泛型集合
      /// </summary>
      /// <typeparam name="T">数据模型类型</typeparam>
      /// <param name="queryColumns">要查询的列集合[该集合为空或null时查询全部字段]</param>
      /// <returns>数据集合</returns>
      List<T> Query<T>(IEnumerable<string> queryColumns) where T : new();

      /// <summary>
      /// 查询数据并返回List泛型集合
      /// </summary>
      /// <typeparam name="T">数据模型类型</typeparam>
      /// <param name="sqlStr">sql语句</param>
      /// <param name="collection">命令的参数集合</param>
      /// <returns>数据集合</returns>
      List<T> Query<T>(string sqlStr, NDbParameterCollection collection = null) where T : new();

      /// <summary>
      /// 查询数据并返回List泛型集合
      /// </summary>
      /// <typeparam name="T">数据模型类型</typeparam>
      /// <param name="item">条件值数据模型</param>
      /// <param name="conditionPropertyNames">条件值数据模型中用途查询条件的属性集合[该集合为空或null时使用主键字段作条件,如果没有主键字段则抛出异常,默认为null]</param>
      /// <param name="queryColumns">要查询的列集合[该集合为空或null时查询全部字段,默认为null]</param>
      /// <returns>数据集合</returns>
      List<T> Query<T>(T item, IEnumerable<string> conditionPropertyNames = null, IEnumerable<string> queryColumns = null) where T : new();
      #endregion

      #region 分页
      /// <summary>
      /// 查询数据并返回List泛型集合,带分页
      /// </summary>
      /// <typeparam name="T">数据模型类型</typeparam>
      /// <param name="orderByColName">排序列名[为空或null不排序]</param>
      /// <param name="pageSize">页大小</param>
      /// <param name="pageIndex">查询页索引</param>
      /// <param name="orderFlag">排序类型[true:升序;false:降序]</param>
      /// <returns>数据集合</returns>
      List<T> Query<T>(string orderByColName, int pageSize, int pageIndex, bool orderFlag) where T : new();

      /// <summary>
      /// 查询数据并返回List泛型集合,带分页
      /// </summary>
      /// <typeparam name="T">数据模型类型</typeparam>
      /// <param name="para">数据查询参数Model</param>
      /// <returns>数据集合</returns>
      //List<T> Query<T>(DBQueryModelPara para) where T : new();

      /// <summary>
      /// 查询数据并返回List泛型集合,带分页
      /// </summary>
      /// <typeparam name="T">数据模型类型</typeparam>
      /// <param name="queryColumns">要查询的列集合[该集合为空或null时查询全部字段]</param>
      /// <param name="orderByColName">排序列名[为空或null不排序]</param>
      /// <param name="pageSize">页大小</param>
      /// <param name="pageIndex">查询页索引</param>
      /// <param name="orderFlag">排序类型[true:升序;false:降序]</param>
      /// <returns>数据集合</returns>
      List<T> Query<T>(IEnumerable<string> queryColumns, string orderByColName, int pageSize, int pageIndex, bool orderFlag) where T : new();

      /// <summary>
      /// 查询数据并返回List泛型集合
      /// </summary>
      /// <typeparam name="T">数据模型类型</typeparam>
      /// <param name="para">数据查询参数Sql</param>
      /// <returns>数据集合</returns>
      //List<T> Query<T>(DBQueryModelParaSql para) where T : new();

      /// <summary>
      /// 查询数据并返回List泛型集合
      /// </summary>
      /// <typeparam name="T">数据模型类型</typeparam>
      /// <param name="sqlStr">sql语句</param>
      /// <param name="orderByColName">排序列名[为空或null不排序]</param>
      /// <param name="pageSize">页大小</param>
      /// <param name="pageIndex">查询页索引</param>
      /// <param name="orderFlag">排序类型[true:升序;false:降序]</param>
      /// <param name="collection">命令的参数集合</param>
      /// <returns>数据集合</returns>
      List<T> Query<T>(string sqlStr, string orderByColName, int pageSize, int pageIndex, bool orderFlag, NDbParameterCollection collection = null) where T : new();

      /// <summary>
      /// 查询数据并返回List泛型集合,带分页
      /// </summary>
      /// <typeparam name="T">数据模型类型</typeparam>
      /// <param name="item">条件值数据模型</param>        
      /// <param name="orderByColName">排序列名[为空或null不排序]</param>
      /// <param name="pageSize">页大小</param>
      /// <param name="pageIndex">查询页索引</param>
      /// <param name="orderFlag">排序类型[true:升序;false:降序]</param>
      /// <param name="conditionPropertyNames">条件值数据模型中用途查询条件的属性集合[该集合为空或null时使用主键字段作条件,如果没有主键字段则抛出异常,默认为null]</param>
      /// <param name="queryColumns">要查询的列集合[该集合为空或null时查询全部字段]</param>
      /// <returns>数据集合</returns>
      List<T> Query<T>(T item, string orderByColName, int pageSize, int pageIndex, bool orderFlag, IEnumerable<string> conditionPropertyNames, IEnumerable<string> queryColumns = null) where T : new();

      /// <summary>
      /// 查询数据并返回List泛型集合,带分页
      /// </summary>
      /// <typeparam name="T">数据模型类型</typeparam>
      /// <param name="para">数据查询参数</param>     
      /// <returns>数据集合</returns>
      //List<T> Query<T>(DBQueryModelParaObj para) where T : new();
      #endregion

      /// <summary>
      /// 查询数据并返回List泛型集合
      /// </summary>
      /// <typeparam name="T">数据模型类型</typeparam>
      /// <param name="para">查询参数</param>
      /// <returns>数据集合</returns>
      //List<T> Query<T>(DBQueryPara para) where T : new();
      #endregion
      */
    }
}
