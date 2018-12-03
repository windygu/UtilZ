using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBBase.Common;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.Dotnet.DBBase.Model;
using UtilZ.Dotnet.DBIBase.DBModel.Model;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.DBBase.Core
{
    /// <summary>
    /// 数据库访问基类
    /// </summary>
    public abstract partial class DBAccessBase : IDBAccess
    {
        #region 属性
        /// <summary>
        /// 数据库编号ID
        /// </summary>
        protected readonly int _dbid;

        /// <summary>
        /// 数据库编号ID
        /// </summary>
        public int DBID
        {
            get { return _dbid; }
        }

        /// <summary>
        /// 数据库参数字符
        /// </summary>
        public abstract string ParaSign { get; }

        /// <summary>
        /// 数据库类型名称
        /// </summary>
        public abstract string DatabaseTypeName { get; }

        /// <summary>
        /// sql语句最大长度
        /// </summary>
        public abstract long SqlMaxLength { get; protected set; }
        #endregion

        /// <summary>
        /// 数据库交互构建对象
        /// </summary>
        protected readonly IDBInteractiveBuilder _interactiveBuilder;

        /// <summary>
        /// 数据库配置实例
        /// </summary>
        protected DBConfig _config { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <param name="interactiveBuilder">数据库交互构建对象</param>
        public DBAccessBase(int dbid, IDBInteractiveBuilder interactiveBuilder)
        {
            this._dbid = dbid;
            this._config = ConfigManager.GetConfigByDBID(dbid);
            this._interactiveBuilder = interactiveBuilder;
            DbConnectionManager.AddDbConnectionPool(dbid, new DbConnectionPool(this._config, interactiveBuilder));
        }

        #region ADO.NET执行原子操作方法
        /// <summary>
        /// 创建数据库接连对象
        /// </summary>
        /// <param name="visitType">数据库访问类型</param>
        /// <returns>数据库接连对象</returns>
        public DbConnectionInfo CreateConnection(DBVisitType visitType)
        {
            return new DbConnectionInfo(this._dbid, visitType);
        }

        /// <summary>
        /// 检查数据库连接[连接正常返回true;否则返回false]
        /// </summary>
        /// <param name="visitType">数据库访问类型</param>
        /// <returns>连接正常返回true;否则返回false</returns>
        public bool CheckDbConnection(DBVisitType visitType)
        {
            try
            {
                using (var con = this._interactiveBuilder.CreateConnection(this._config, visitType))
                {
                    con.Open();
                }

                return true;
            }
            catch (Exception ex)
            {
                DBLog.OutLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 创建命令
        /// </summary>
        /// <returns>命令</returns>
        public IDbCommand CreateCommand()
        {
            var cmd = this._interactiveBuilder.CreateCommand();
            if (this._config.CommandTimeout != DBConstant.CommandTimeout)
            {
                cmd.CommandTimeout = this._config.CommandTimeout;
            }

            return cmd;
        }

        /// <summary>
        /// 添加命令参数
        /// </summary>
        /// <param name="cmd">要添加参数的命令对象</param>
        /// <param name="parameterName">参数免</param>
        /// <param name="value">参数值</param>
        public void AddCommandParameter(IDbCommand cmd, string parameterName, object value)
        {
            var parameter = cmd.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            //parameter.DbType = DbType.String;
            cmd.Parameters.Add(parameter);
        }

        /// <summary>
        /// 创建DbDataAdapter
        /// </summary>
        /// <returns>DbDataAdapter</returns>
        public IDbDataAdapter CreateDbDataAdapter()
        {
            return this._interactiveBuilder.CreateDbDataAdapter();
        }

        /// <summary>
        /// ExecuteScalar执行SQL语句,返回执行结果的第一行第一列；
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="visitType">数据库访问类型</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回执行结果</returns>
        public virtual object ExecuteScalar(string sqlStr, DBVisitType visitType, NDbParameterCollection collection = null)
        {
            if (string.IsNullOrWhiteSpace(sqlStr))
            {
                throw new ArgumentNullException("sqlStr");
            }

            using (var conInfo = new DbConnectionInfo(this._dbid, visitType))
            {
                return this.PrimitveExecuteScalar(conInfo.Connection, sqlStr, collection);
            }
        }

        /// <summary>
        /// ExecuteNonQuery执行SQL语句,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="visitType">数据库访问类型</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回执行结果</returns>
        public virtual int ExecuteNonQuery(string sqlStr, DBVisitType visitType, NDbParameterCollection collection = null)
        {
            if (string.IsNullOrWhiteSpace(sqlStr))
            {
                throw new ArgumentNullException("sqlStr");
            }

            using (var conInfo = new DbConnectionInfo(this._dbid, visitType))
            {
                return this.PrimitveExecuteNonQuery(conInfo.Connection, sqlStr, collection);
            }
        }
        #endregion

        #region IDispose接口实现
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="isDisposing">是否释放资源标识</param>
        protected virtual void Dispose(bool isDisposing)
        {

        }
        #endregion
    }
}
