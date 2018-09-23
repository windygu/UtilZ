using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBBase.Core;
using UtilZ.Dotnet.DBBase.Model;
using UtilZ.Dotnet.DBIBase.DBModel.Model;

namespace UtilZ.Dotnet.DBBase.Core
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
                obj = this.InnerExecuteScalar(conInfo.Connection, sqlStr, collection);
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
                con = conInfo.Connection;
            }

            try
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandText = sqlStr;
                this.SetParameter(cmd, collection);
                IDbDataAdapter da = this._interactiveBuilder.CreateDbDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                ds.EnforceConstraints = false;
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
    }
}
