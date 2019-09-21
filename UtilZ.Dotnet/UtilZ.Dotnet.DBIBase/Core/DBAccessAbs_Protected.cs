﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.DBIBase.Connection;
using UtilZ.Dotnet.DBIBase.Model;

namespace UtilZ.Dotnet.DBIBase.Core
{
    /// <summary>
    /// 保护方法分部类
    /// </summary>
    public abstract partial class DBAccessAbs
    {
        /// <summary>
        /// 转换ExecuteScalar结果为指定对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="obj">ExecuteScalar结果</param>
        /// <returns>转换结果</returns>
        protected T ConvertObject<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T);
            }

            T result;
            Type conversionType = typeof(T);
            if (obj.GetType() == conversionType)
            {
                result = (T)obj;
            }
            else
            {
                result = (T)Convert.ChangeType(obj, conversionType);
            }

            return result;
        }

        /// <summary>
        /// 创建DbDataAdapter
        /// </summary>
        /// <returns>DbDataAdapter</returns>
        protected IDbDataAdapter PrimitiveCreateDbDataAdapter()
        {
            return this._dbInteraction.GetProviderFactory().CreateDataAdapter();
        }

        /// <summary>
        /// 执行SQL语句,返回查询结果
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="parameterNameValueDic">参数名名称及对应的值字典集合[key:参数名称,含参数符号;value:参数值]</param>
        /// <returns>返回执行结果</returns>
        protected DataTable PrimitiveQueryDataToDataTable(string sqlStr, Dictionary<string, object> parameterNameValueDic = null)
        {
            using (var connectionInfo = new DbConnectionInfo(this._dbid, Model.DBVisitType.R))
            {
                return this.PrimitiveQueryDataToDataTable(connectionInfo.DbConnection, sqlStr, parameterNameValueDic);
            }
        }

        /// <summary>
        /// 执行SQL语句,返回查询结果
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="parameterNameValueDic">参数名名称及对应的值字典集合[key:参数名称,含参数符号;value:参数值]</param>
        /// <returns>返回执行结果</returns>
        protected DataTable PrimitiveQueryDataToDataTable(IDbConnection con, string sqlStr, Dictionary<string, object> parameterNameValueDic = null)
        {
            var cmd = this.CreateCommand(con, sqlStr, parameterNameValueDic);
            var da = this.PrimitiveCreateDbDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        /// <summary>
        /// ExecuteScalar执行SQL语句,返回执行结果的第一行第一列；
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="visitType">数据库访问类型</param>
        /// <param name="parameterNameValueDic">参数名名称及对应的值字典集合[key:参数名称,含参数符号;value:参数值]</param>
        /// <returns>返回执行结果</returns>
        protected object PrimitiveExecuteScalar(string sqlStr, DBVisitType visitType, Dictionary<string, object> parameterNameValueDic = null)
        {
            using (var conInfo = new DbConnectionInfo(this._dbid, visitType))
            {
                return this.PrimitiveExecuteScalar(conInfo.DbConnection, sqlStr, parameterNameValueDic);
            }
        }

        /// <summary>
        /// ExecuteScalar执行SQL语句,返回执行结果的第一行第一列；
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="parameterNameValueDic">参数名名称及对应的值字典集合[key:参数名称,含参数符号;value:参数值]</param>
        /// <returns>返回执行结果</returns>
        protected object PrimitiveExecuteScalar(IDbConnection con, string sqlStr, Dictionary<string, object> parameterNameValueDic = null)
        {
            var cmd = this.CreateCommand(con, sqlStr, parameterNameValueDic);
            return cmd.ExecuteScalar();
        }

        /// <summary>
        /// ExecuteNonQuery执行SQL语句,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="visitType">数据库访问类型</param>
        /// <param name="parameterNameValueDic">参数名名称及对应的值字典集合[key:参数名称,含参数符号;value:参数值]</param>
        /// <returns>返回执行结果</returns>
        protected int PrimitiveExecuteNonQuery(string sqlStr, DBVisitType visitType, Dictionary<string, object> parameterNameValueDic = null)
        {
            using (var conInfo = new DbConnectionInfo(this._dbid, visitType))
            {
                return this.PrimitiveExecuteNonQuery(conInfo.DbConnection, sqlStr, parameterNameValueDic);
            }
        }

        /// <summary>
        /// ExecuteNonQuery执行SQL语句,返回受影响的行数
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="parameterNameValueDic">参数名名称及对应的值字典集合[key:参数名称,含参数符号;value:参数值]</param>
        /// <returns>返回执行结果</returns>
        protected int PrimitiveExecuteNonQuery(IDbConnection con, string sqlStr, Dictionary<string, object> parameterNameValueDic = null)
        {
            var cmd = this.CreateCommand(con, sqlStr, parameterNameValueDic);
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 转换原始查询SQL语句为分页查询SQL语句
        /// </summary>
        /// <param name="sqlStr">原始查询SQL语句</param>
        /// <param name="orderInfos">排序列名集合[null为或空不排序]</param>
        /// <param name="pageIndex">目标页索引</param>
        /// <param name="pageSize">页大小</param>   
        /// <param name="dataBaseVersion">数据库版本信息</param>
        /// <param name="pagingAssistFieldName">分页字段名称</param>
        /// <returns>分页查询SQL语句</returns>
        protected string PrimitiveConvertSqlToPagingQuerySql(string sqlStr, IEnumerable<DBOrderInfo> orderInfos, 
            long pageIndex, long pageSize, DataBaseVersionInfo dataBaseVersion, out string pagingAssistFieldName)
        {
            return this._dbInteraction.ConvertSqlToPagingQuerySql(sqlStr, orderInfos, pageIndex, pageSize, dataBaseVersion, out pagingAssistFieldName);
        }
    }
}
