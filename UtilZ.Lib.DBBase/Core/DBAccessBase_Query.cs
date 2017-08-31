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
        public virtual DBPageInfo QueryPageInfo(int pageSize, string sqlStr, NDbParameterCollection collection = null)
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
        protected virtual DataSet InnerQueryDataSet(IDbConnection con, string sqlStr, NDbParameterCollection collection = null)
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
                this.Interaction.SetParameter(cmd, collection);
                IDbDataAdapter da = this.Interaction.CreateDbDataAdapter();
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
        protected virtual DataTable InnerQueryData(IDbConnection con, string sqlStr, NDbParameterCollection collection = null)
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
        public virtual DataSet QueryDataSet(string sqlStr, NDbParameterCollection collection = null)
        {
            return this.InnerQueryDataSet(null, sqlStr, collection);
        }

        /// <summary>
        /// 执行SQL语句,返回查询结果
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回执行结果</returns>
        public virtual DataTable QueryData(string sqlStr, NDbParameterCollection collection = null)
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
        /// 查询数据并返回List泛型集合,带分页
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="orderByColName">排序列名[为空或null不排序]</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">查询页索引</param>
        /// <param name="orderFlag">排序类型[true:升序;false:降序]</param>
        /// <param name="queryColumns">要查询的列集合[该集合为空或null时查询全部字段]</param>
        /// <param name="priKeyCols">主键集合[当为oracle时此值不能为空或null,否则查询出的结果可能出现重复,因为Oracle数据的分页查询中排序规则不稳定]</param>
        /// <returns>数据集合</returns>
        public List<T> QueryT<T>(string orderByColName, int pageSize, int pageIndex, bool orderFlag, IEnumerable<string> queryColumns = null, IEnumerable<string> priKeyCols = null) where T : class, new()
        {
            List<DBOrderInfo> orderInfos = null;
            if (!string.IsNullOrWhiteSpace(orderByColName))
            {
                orderInfos = new List<DBOrderInfo>();
                orderInfos.Add(new DBOrderInfo(orderByColName, orderFlag));
            }

            return this.QueryT<T>(orderInfos, pageSize, pageIndex, orderFlag, queryColumns, priKeyCols);
        }

        /// <summary>
        /// 查询数据并返回List泛型集合,带分页
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="orderInfos">排序列名[为空或null不排序]</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">查询页索引</param>
        /// <param name="orderFlag">排序类型[true:升序;false:降序]</param>
        /// <param name="queryColumns">要查询的列集合[该集合为空或null时查询全部字段]</param>
        /// <param name="priKeyCols">主键集合[当为oracle时此值不能为空或null,否则查询出的结果可能出现重复,因为Oracle数据的分页查询中排序规则不稳定]</param>
        /// <returns>数据集合</returns>
        public List<T> QueryT<T>(IEnumerable<DBOrderInfo> orderInfos, int pageSize, int pageIndex, bool orderFlag, IEnumerable<string> queryColumns = null, IEnumerable<string> priKeyCols = null) where T : class, new()
        {
            Type type = typeof(T);
            //数据模型信息
            DataModelInfo dataTableInfo = ORMManager.GetDataModelInfo(type);
            string sqlStr = null;
            if (queryColumns == null || queryColumns.Count() == 0)
            {
                sqlStr = string.Format("select * from {0}", dataTableInfo.DBTable.Name);
            }
            else
            {
                sqlStr = string.Format("select {0} from {1}", string.Join(",", queryColumns), dataTableInfo.DBTable.Name);
            }

            DataTable dt = this.QueryPagingData(sqlStr, orderInfos, pageSize, pageIndex, orderFlag, null, priKeyCols);
            if (dt == null || dt.Rows.Count == 0)
            {
                return new List<T>();
            }
            else
            {
                return ORMManager.ConvertData<T>(dt, this);
            }
        }
        #endregion
        #endregion
    }
}
