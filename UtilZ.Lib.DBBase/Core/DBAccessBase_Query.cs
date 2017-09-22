using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Lib.DBBase.Factory;
using UtilZ.Lib.DBModel.DBObject;
using UtilZ.Lib.DBModel.Model;

namespace UtilZ.Lib.DBBase.Core
{
    //数据库访问基类-查询
    public abstract partial class DBAccessBase
    {
        #region 查询页数
        /// <summary>
        /// 查询分页信息
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>分页信息</returns>
        public DBPageInfo QueryPageInfo(int pageSize, string sqlStr, NDbParameterCollection collection = null)
        {
            if (pageSize < 1)
            {
                throw new ArgumentException("页大小不能小于1", "pageSize");
            }

            object obj;
            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.R))
            {
                obj = this.InnerExecuteScalar(conInfo.Con, sqlStr, collection);
            }

            long totalCount = 0;
            try
            {
                totalCount = Convert.ToInt64(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("SQL语句{0}查询结果无效,不能用于计算查询页数", sqlStr), ex);
            }

            long pageCount = totalCount / pageSize;
            if (totalCount % pageSize != 0)
            {
                pageCount++;
            }

            return new DBPageInfo(Convert.ToInt32(pageCount), totalCount, pageSize);
        }
        #endregion

        #region 基础查询
        /// <summary>
        /// 执行SQL语句,返回查询DataSet
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回执行结果</returns>
        protected DataSet InnerQueryDataSet(IDbConnection con, string sqlStr, NDbParameterCollection collection = null)
        {
            DbConnectionInfo conInfo = null;
            if (con == null)
            {
                conInfo = new DbConnectionInfo(this._dbid, DBVisitType.R);
                con = conInfo.Con;
            }
            try
            {
                IDbCommand cmd = this.CreateCommand(con);
                cmd.CommandText = sqlStr;
                this._interaction.SetParameter(cmd, collection);
                IDbDataAdapter da = this._interaction.CreateDbDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            finally
            {
                if (conInfo != null)
                {
                    conInfo.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行SQL语句,返回查询结果
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回执行结果</returns>
        protected DataTable InnerQueryData(IDbConnection con, string sqlStr, NDbParameterCollection collection = null)
        {
            DataSet ds = this.InnerQueryDataSet(con, sqlStr, collection);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        /// <summary>
        /// 执行SQL语句,返回查询DataSet
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回执行结果</returns>
        public DataSet QueryDataSet(string sqlStr, NDbParameterCollection collection = null)
        {
            return this.InnerQueryDataSet(null, sqlStr, collection);
        }

        /// <summary>
        /// 执行SQL语句,返回查询结果
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回执行结果</returns>
        public DataTable QueryData(string sqlStr, NDbParameterCollection collection = null)
        {
            return this.InnerQueryData(null, sqlStr, collection);
        }
        #endregion

        #region 分页查询
        #region 分页查询基础方法
        /// <summary>
        /// SQL参数断言
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <param name="pageIndex">查询页</param>
        /// <param name="pageSize">页大小</param>
        protected virtual void SQLAssert(string sqlStr, int pageIndex, int pageSize)
        {
            if (string.IsNullOrEmpty(sqlStr))
            {
                throw new ArgumentNullException("查询语句不能为空或null", "sqlStr");
            }

            sqlStr = sqlStr.Trim();
            if (string.IsNullOrWhiteSpace(sqlStr))
            {
                throw new ArgumentException("查询语句不能为全空格", "sqlStr");
            }

            if (pageIndex < 1)
            {
                throw new ArgumentException(string.Format("查询页索引值不能小于1,值{0}无效", pageIndex), "pageIndex");
            }

            if (pageSize < 1)
            {
                throw new ArgumentException(string.Format("查询页大小不能小于1,值{0}无效", pageSize), "pageSize");
            }
        }

        /// <summary>
        /// 创建排序字符串
        /// </summary>
        /// <param name="orderInfos">排序列名集合</param>
        /// <param name="priKeyCols">主键列集合</param>
        /// <returns>排序字符串</returns>
        protected virtual string CreateOrderStr(IEnumerable<DBOrderInfo> orderInfos, IEnumerable<string> priKeyCols)
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

            //移除最后一个,
            sbOrder = sbOrder.Remove(sbOrder.Length - 1, 1);
            return sbOrder.ToString();
        }

        /// <summary>
        /// 创建T泛型查询
        /// </summary>
        /// <typeparam name="T">T泛型</typeparam>
        /// <param name="query">查询条件对象</param>
        /// <param name="conditionProperties">条件属性集合</param>
        /// <param name="queryProperties">查询属性集合</param>
        /// <param name="dataTableInfo">数据模型信息</param>
        /// <param name="sbSqlStr">SQL StringBuilder</param>
        /// <param name="collection">参数集合</param>
        protected void GenerateTQuery<T>(T query, IEnumerable<string> conditionProperties, IEnumerable<string> queryProperties, out DataModelInfo dataTableInfo, out StringBuilder sbSqlStr, out NDbParameterCollection collection) where T : class, new()
        {
            Type type = typeof(T);
            //数据模型信息
            dataTableInfo = ORMManager.GetDataModelInfo(type);
            var propertyNameColNameMapDic = dataTableInfo.PropertyNameColNameMapDic;
            sbSqlStr = new StringBuilder();
            if (queryProperties == null || queryProperties.Count() == 0)
            {
                sbSqlStr.Append(string.Format("select * from {0}", dataTableInfo.DBTable.Name));
            }
            else
            {
                var queryColNames = new List<string>();
                foreach (var queryPropertyName in queryProperties)
                {
                    if (propertyNameColNameMapDic.ContainsKey(queryPropertyName))
                    {
                        queryColNames.Add(propertyNameColNameMapDic[queryPropertyName]);
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("类型:{0}中不包含属性:{1}", type.FullName, queryPropertyName));
                    }
                }

                sbSqlStr.Append(string.Format("select {0} from {1}", string.Join(",", queryColNames), dataTableInfo.DBTable.Name));
            }
            if (query != null)
            {
                collection = new NDbParameterCollection();
                string tmpConditionColName;
                Dictionary<string, DBColumnPropertyInfo> dicDBColumnProperties = dataTableInfo.DicDBColumnProperties;
                DBColumnPropertyInfo dbColumnPropertyInfo;

                var conditionList = new List<string>();
                if (conditionProperties == null || conditionProperties.Count() == 0)
                {
                    //条件属性集合[该集合为空或null时仅用主键字段]
                    foreach (var priKeyColName in dataTableInfo.DicPriKeyDBColumnProperties.Keys.ToArray())
                    {
                        if (propertyNameColNameMapDic.ContainsKey(priKeyColName))
                        {
                            tmpConditionColName = propertyNameColNameMapDic[priKeyColName];
                            conditionList.Add(string.Format("{0}={1}{2}", tmpConditionColName, this.ParaSign, tmpConditionColName));
                            dbColumnPropertyInfo = dicDBColumnProperties[tmpConditionColName];
                            collection.Add(tmpConditionColName, dbColumnPropertyInfo.PropertyInfo.GetValue(query, dbColumnPropertyInfo.DBColumn.Index));
                        }
                        else
                        {
                            throw new ArgumentException(string.Format("表:{0}中不包含列:{1}", dataTableInfo.DBTable.Name, priKeyColName));
                        }
                    }
                }
                else
                {
                    //条件属性集合[该集合为空或null时仅用主键字段]
                    foreach (var conditionPropertyName in conditionProperties)
                    {
                        if (propertyNameColNameMapDic.ContainsKey(conditionPropertyName))
                        {
                            tmpConditionColName = propertyNameColNameMapDic[conditionPropertyName];
                            conditionList.Add(string.Format("{0}={1}{2}", tmpConditionColName, this.ParaSign, tmpConditionColName));
                            dbColumnPropertyInfo = dicDBColumnProperties[tmpConditionColName];
                            collection.Add(tmpConditionColName, dbColumnPropertyInfo.PropertyInfo.GetValue(query, dbColumnPropertyInfo.DBColumn.Index));
                        }
                        else
                        {
                            throw new ArgumentException(string.Format("类型:{0}中不包含属性:{1}", type.FullName, conditionPropertyName));
                        }
                    }
                }

                sbSqlStr.Append(" where ");
                sbSqlStr.Append(string.Join(" AND ", conditionList));
            }
            else
            {
                collection = null;
            }
        }
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
        public DataTable QueryPagingData(string sqlStr, string orderByColName, int pageSize, int pageIndex, bool orderFlag, NDbParameterCollection collection = null, IEnumerable<string> priKeyCols = null)
        {
            List<DBOrderInfo> orderInfos = null;
            if (!string.IsNullOrWhiteSpace(orderByColName))
            {
                orderInfos = new List<DBOrderInfo>();
                orderInfos.Add(new DBOrderInfo(orderByColName, orderFlag));
            }

            return this.QueryPagingData(sqlStr, orderInfos, pageSize, pageIndex, orderFlag, collection, priKeyCols);
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
        public abstract DataTable QueryPagingData(string sqlStr, IEnumerable<DBOrderInfo> orderInfos, int pageSize, int pageIndex, bool orderFlag, NDbParameterCollection collection = null, IEnumerable<string> priKeyCols = null);
        #endregion

        #region 泛型
        /// <summary>
        /// 查询数据并返回List泛型集合
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <returns>数据集合</returns>
        public List<T> QueryT<T>(string sqlStr, NDbParameterCollection collection = null) where T : class, new()
        {
            DataTable dt = this.QueryData(sqlStr, collection);
            return ORMManager.ConvertData<T>(dt, this);
        }

        /// <summary>
        /// 查询数据并返回List泛型集合
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <returns>数据集合</returns>
        public List<T> QueryT<T>() where T : class, new()
        {
            return this.QueryT<T>(null, null, null, string.Empty, false);
        }

        /// <summary>
        /// 查询数据并返回List泛型集合
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="query">查询对象[为null时无条件查询]</param>
        /// <param name="conditionProperties">条件属性集合[该集合为空或null时仅用主键字段]</param>
        /// <returns>数据集合</returns>
        public List<T> QueryT<T>(T query, IEnumerable<string> conditionProperties) where T : class, new()
        {
            return this.QueryT<T>(query, conditionProperties, null, string.Empty, false);
        }

        /// <summary>
        /// 查询数据并返回List泛型集合
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="query">查询对象[为null时无条件查询]</param>
        /// <param name="conditionProperties">条件属性集合[该集合为空或null时仅用主键字段]</param>
        /// <param name="queryProperties">要查询的列集合[该集合为空或null时查询全部字段]</param>
        /// <returns>数据集合</returns>
        public List<T> QueryT<T>(T query, IEnumerable<string> conditionProperties, IEnumerable<string> queryProperties) where T : class, new()
        {
            return this.QueryT<T>(query, conditionProperties, queryProperties, string.Empty, false);
        }

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
        public List<T> QueryT<T>(T query = null, IEnumerable<string> conditionProperties = null, IEnumerable<string> queryProperties = null, string orderProperty = null, bool orderFlag = false) where T : class, new()
        {
            List<DBOrderInfo> orderInfos;
            if (string.IsNullOrWhiteSpace(orderProperty))
            {
                orderInfos = null;
            }
            else
            {
                orderInfos = new List<DBOrderInfo>();
                orderInfos.Add(new DBOrderInfo(orderProperty, orderFlag));
            }

            return this.QueryT<T>(query, conditionProperties, queryProperties, orderInfos);
        }

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
        public List<T> QueryT<T>(T query = null, IEnumerable<string> conditionProperties = null, IEnumerable<string> queryProperties = null, IEnumerable<DBOrderInfo> orderInfos = null, bool orderFlag = false) where T : class, new()
        {
            DataModelInfo dataTableInfo;
            StringBuilder sbSqlStr;
            NDbParameterCollection collection;
            this.GenerateTQuery(query, conditionProperties, queryProperties, out dataTableInfo, out sbSqlStr, out collection);

            if (orderInfos != null && orderInfos.Count() > 0)
            {
                sbSqlStr.Append(" order by ");
                sbSqlStr.Append(this.CreateOrderStr(orderInfos, dataTableInfo.DicPriKeyDBColumnProperties.Keys.ToArray()));
            }

            DataTable dt = this.QueryData(sbSqlStr.ToString(), collection);
            return ORMManager.ConvertData<T>(dt, this);
        }

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
        public List<T> QueryTPaging<T>(string sqlStr, string orderByColName, int pageSize, int pageIndex, bool orderFlag, NDbParameterCollection collection = null) where T : class, new()
        {
            List<DBOrderInfo> orderInfos = null;
            if (!string.IsNullOrWhiteSpace(orderByColName))
            {
                orderInfos = new List<DBOrderInfo>();
                orderInfos.Add(new DBOrderInfo(orderByColName, orderFlag));
            }

            return this.QueryTPaging<T>(sqlStr, orderInfos, pageSize, pageIndex, orderFlag, collection);
        }

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
        public List<T> QueryTPaging<T>(string sqlStr, IEnumerable<DBOrderInfo> orderInfos, int pageSize, int pageIndex, bool orderFlag, NDbParameterCollection collection = null) where T : class, new()
        {
            Type type = typeof(T);
            //数据模型信息
            var dataTableInfo = ORMManager.GetDataModelInfo(type);
            DataTable dt = this.QueryPagingData(sqlStr, orderInfos, pageSize, pageIndex, orderFlag, collection, dataTableInfo.DicPriKeyDBColumnProperties.Keys.ToArray());
            return ORMManager.ConvertData<T>(dt, this);
        }

        /// <summary>
        /// 查询数据并返回List泛型集合,带分页
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">查询页索引</param>
        /// <param name="orderProperty">排序属性名称[为null不排序]</param>
        /// <param name="orderFlag">排序类型[true:升序;false:降序,默认false]</param>
        /// <returns>数据集合</returns>
        public List<T> QueryTPaging<T>(int pageSize, int pageIndex, string orderProperty = null, bool orderFlag = false) where T : class, new()
        {
            return QueryTPaging<T>(pageSize, pageIndex, null, null, orderProperty, orderFlag);
        }

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
        public List<T> QueryTPaging<T>(int pageSize, int pageIndex, T query, IEnumerable<string> conditionProperties, string orderProperty = null, bool orderFlag = false) where T : class, new()
        {
            return QueryTPaging<T>(pageSize, pageIndex, query, conditionProperties, null, orderProperty, orderFlag);
        }

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
        public List<T> QueryTPaging<T>(int pageSize, int pageIndex, T query = null, IEnumerable<string> conditionProperties = null, IEnumerable<string> queryProperties = null, string orderProperty = null, bool orderFlag = false) where T : class, new()
        {
            List<DBOrderInfo> orderInfos;
            if (string.IsNullOrWhiteSpace(orderProperty))
            {
                orderInfos = null;
            }
            else
            {
                orderInfos = new List<DBOrderInfo>();
                orderInfos.Add(new DBOrderInfo(orderProperty, orderFlag));
            }

            return this.QueryTPaging<T>(pageSize, pageIndex, query, conditionProperties, queryProperties, orderInfos);
        }

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
        public List<T> QueryTPaging<T>(int pageSize, int pageIndex, T query = null, IEnumerable<string> conditionProperties = null, IEnumerable<string> queryProperties = null, IEnumerable<DBOrderInfo> orderInfos = null, bool orderFlag = false) where T : class, new()
        {
            DataModelInfo dataTableInfo;
            StringBuilder sbSqlStr;
            NDbParameterCollection collection;
            this.GenerateTQuery(query, conditionProperties, queryProperties, out dataTableInfo, out sbSqlStr, out collection);

            DataTable dt = this.QueryPagingData(sbSqlStr.ToString(), orderInfos, pageSize, pageIndex, orderFlag, collection, dataTableInfo.DicPriKeyDBColumnProperties.Keys.ToArray());
            return ORMManager.ConvertData<T>(dt, this);
        }
        #endregion
        #endregion
    }
}
