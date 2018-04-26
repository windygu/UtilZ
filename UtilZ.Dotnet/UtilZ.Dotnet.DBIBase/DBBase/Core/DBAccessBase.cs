using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBIBase.DBBase.Factory;
using UtilZ.Dotnet.DBIBase.DBModel.Config;
using UtilZ.Dotnet.DBIBase.DBModel.Constant;
using UtilZ.Dotnet.DBIBase.DBModel.Interface;
using UtilZ.Dotnet.DBIBase.DBModel.Model;
using UtilZ.Dotnet.DBIBase.DBBase.Interface;

namespace UtilZ.Dotnet.DBIBase.DBBase.Core
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
        /// 数据库交互实例
        /// </summary>
        protected readonly IDBInteraction _interaction;

        /// <summary>
        /// 数据库配置实例
        /// </summary>
        protected DBConfigElement Config { get; private set; }

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

        /// <summary>
        /// 外部数据库交互接口
        /// </summary>
        public IDBInteractionEx InteractionEx
        {
            get { return this._interaction; }
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        public DBAccessBase(int dbid)
        {
            this._dbid = dbid;
            this.Config = ConfigManager.GetConfigItem(dbid);
            DBFactoryBase dbFactory = DBFactoryManager.GetDBFactory(dbid);
            this._interaction = dbFactory.GetDBInteraction(this.Config);
            //this.ConditionGenerator = dbFactory.GetConditionGenerator();
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
        /// <returns>连接正常返回true;否则返回false</returns>
        public bool CheckDbConnection()
        {
            try
            {
                this.GetDataBaseSysTime();
                //using (var con = new DbConnectionInfo(this._dbid, DBVisitType.R))
                //{
                //    con.Con.Open();
                //}

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建命令
        /// </summary>
        /// <param name="con">连接对象</param>
        /// <returns>命令</returns>
        public IDbCommand CreateCommand(IDbConnection con)
        {
            var cmd = con.CreateCommand();
            if (this.Config.CommandTimeout != DBConstant.CommandTimeout)
            {
                cmd.CommandTimeout = this.Config.CommandTimeout;
            }

            return cmd;
        }

        /// <summary>
        /// 创建DbDataAdapter
        /// </summary>
        /// <returns>DbDataAdapter</returns>
        public IDbDataAdapter CreateDbDataAdapter()
        {
            return this._interaction.CreateDbDataAdapter();
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
                return this.InnerExecuteScalar(conInfo.Con, sqlStr, collection);
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
                return this.InnerExecuteNonQuery(conInfo.Con, sqlStr, collection);
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="para">参数</param>
        /// <returns>执行结果</returns>
        public virtual StoredProcedureResult ExcuteStoredProcedure(StoredProcedurePara para)
        {
            if (para == null)
            {
                throw new ArgumentNullException("参数不能为null", "para");
            }

            if (string.IsNullOrEmpty(para.StoredProcedureName))
            {
                throw new ArgumentNullException("存储过程名称不能为null或空", "para.StoredProcedureName");
            }

            using (var conInfo = new DbConnectionInfo(this._dbid, para.VisitType))
            {
                IDbCommand cmd = this.CreateCommand(conInfo.Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = para.StoredProcedureName;
                Dictionary<NDbParameter, IDbDataParameter> outputParas = new Dictionary<NDbParameter, IDbDataParameter>();//输出或返回值参数

                IDbDataParameter cmdParameter = null;
                foreach (var parameter in para)
                {
                    cmdParameter = this._interaction.CreateDbParameter(parameter);
                    cmd.Parameters.Add(cmdParameter);
                    if (parameter.Direction == ParameterDirection.InputOutput
                        || parameter.Direction == ParameterDirection.Output
                        || parameter.Direction == ParameterDirection.ReturnValue)
                    {
                        //输出或输入及输出或返回值参数
                        outputParas.Add(parameter, cmdParameter);
                    }
                }

                var result = new StoredProcedureResult();
                switch (para.ExcuteType)
                {
                    case DBExcuteType.NonQuery:
                        result.Value = cmd.ExecuteNonQuery();
                        break;
                    case DBExcuteType.Scalar:
                        result.Value = cmd.ExecuteScalar();
                        break;
                    case DBExcuteType.Query:
                        IDbDataAdapter da = this._interaction.CreateDbDataAdapter();
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        result.Value = ds;
                        break;
                    default:
                        throw new Exception(string.Format("不支持的执行类型{0}", para.ExcuteType));
                }

                foreach (var outputPara in outputParas)
                {
                    result.ParaValue.Add(outputPara.Key, outputPara.Value.Value);
                }

                return result;
            }
        }

        /// <summary>
        /// 执行ADO.NET事务
        /// </summary>
        /// <param name="para">参数</param>
        /// <param name="function">事务执行委托</param>
        /// <returns>事务返回值</returns>
        public virtual object ExcuteAdoNetTransaction(object para, Func<IDbConnection, IDbTransaction, object, object> function)
        {
            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.W))
            {
                var con = conInfo.Con;
                using (IDbTransaction tranaction = con.BeginTransaction())
                {
                    try
                    {
                        object obj = function(con, tranaction, para);
                        tranaction.Commit();
                        return obj;
                    }
                    catch (Exception)
                    {
                        tranaction.Rollback();//出现出错执行回滚操作
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 创建EF上下文接口
        /// </summary>
        /// <param name="visitType">数据库访问类型</param>
        /// <returns>IEFDbContext</returns>
        public IEFDbContext CreateEFDbContext(DBVisitType visitType)
        {
            return new EFDbContext(new DbConnectionInfo(this._dbid, visitType));
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
