using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Lib.Base;
using UtilZ.Lib.Base.DataStruct;
using UtilZ.Lib.DBBase.Core;
using UtilZ.Lib.DBBase.Factory;
using UtilZ.Lib.DBModel.Common;
using UtilZ.Lib.DBModel.Constant;
using UtilZ.Lib.DBModel.Model;
using UtilZ.Lib.DBSqlite.Interface;
using UtilZ.Lib.DBSqlite.Write;

namespace UtilZ.Lib.DBSqlite.Core
{
    /// <summary>
    /// SQLite数据库访问类
    /// </summary>
    public partial class SQLiteDBAccess : DBAccessBase, ISQLiteDBAccessBase
    {
        #region 重写父类方法属性
        /// <summary>
        /// 数据库参数字符
        /// </summary>
        private const string PARASIGN = "@";

        /// <summary>
        /// 数据库参数字符
        /// </summary>
        public override string ParaSign
        {
            get { return PARASIGN; }
        }

        /// <summary>
        /// 数据库程序集名称
        /// </summary>
        private readonly string _databaseName;

        /// <summary>
        /// 数据库类型名称
        /// </summary>
        public override string DatabaseTypeName
        {
            get { return _databaseName; }
        }

        /// <summary>
        /// sql语句最大长度
        /// </summary>
        public override long SqlMaxLength { get; protected set; }
        #endregion

        #region 重写父类方法
        #region ADO.NET执行原子操作方法
        /// <summary>
        /// ExecuteScalar执行SQL语句,返回执行结果的第一行第一列；
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="visitType">数据库访问类型</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回执行结果</returns>
        public override object ExecuteScalar(string sqlStr, DBVisitType visitType, NDbParameterCollection collection = null)
        {
            if (string.IsNullOrWhiteSpace(sqlStr))
            {
                return null;
            }

            if (visitType == DBVisitType.R)
            {
                return base.ExecuteScalar(sqlStr, visitType, collection);
            }
            else
            {
                var item = new SQLiteExecuteScalar(this._waitTimeout, sqlStr, collection);
                this._writeQueue.Enqueue(item);
                item.WaitOne();
                if (item.ExcuteResult)
                {
                    return item.Result;
                }
                else
                {
                    throw item.Result as Exception;
                }
            }
        }

        /// <summary>
        /// ExecuteNonQuery执行SQL语句,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="visitType">数据库访问类型</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回执行结果</returns>
        public override int ExecuteNonQuery(string sqlStr, DBVisitType visitType, NDbParameterCollection collection = null)
        {
            if (string.IsNullOrWhiteSpace(sqlStr))
            {
                return 0;
            }

            if (visitType == DBVisitType.R)
            {
                return base.ExecuteNonQuery(sqlStr, visitType, collection);
            }
            else
            {
                var item = new SQLiteExecuteNonQuery(this._waitTimeout, sqlStr, collection);
                this._writeQueue.Enqueue(item);
                item.WaitOne();
                if (item.ExcuteResult)
                {
                    return Convert.ToInt32(item.Result);
                }
                else
                {
                    throw item.Result as Exception;
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="para">参数</param>
        /// <returns>执行结果</returns>
        public override StoredProcedureResult ExcuteStoredProcedure(StoredProcedurePara para)
        {
            StoredProcedureResult result;
            if (para.VisitType == DBVisitType.R)
            {
                result = base.ExcuteStoredProcedure(para);
            }
            else
            {
                var item = new SQLiteExcuteStoredProcedure(this._waitTimeout, para);
                this._writeQueue.Enqueue(item);
                item.WaitOne();
                if (item.ExcuteResult)
                {
                    result = item.Result as StoredProcedureResult;
                }
                else
                {
                    throw item.Result as Exception;
                }
            }

            return result;
        }

        /// <summary>
        /// 执行ADO.NET事务
        /// </summary>
        /// <param name="para">参数</param>
        /// <param name="function">事务执行委托</param>
        /// <returns>事务返回值</returns>
        public override object ExcuteAdoNetTransaction(object para, Func<IDbConnection, IDbTransaction, object, object> function)
        {
            var item = new SQLiteTransaction(this._waitTimeout, para, function);
            this._writeQueue.Enqueue(item);
            item.WaitOne();
            if (item.ExcuteResult)
            {
                return item.Result;
            }
            else
            {
                throw item.Result as Exception;
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// 写队列
        /// </summary>
        private readonly AsynQueue<SQLiteWriteBase> _writeQueue;

        /// <summary>
        /// 等待超时时间(-1表示无限等待)
        /// </summary>
        private readonly int _waitTimeout = System.Threading.Timeout.Infinite;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <param name="databaseName">数据库程序集名称</param>
        public SQLiteDBAccess(int dbid, string databaseName)
                    : base(dbid)
        {
            this._databaseName = databaseName;
            this._waitTimeout = this.Config.CommandTimeout;
            if (this.Config.SqlMaxLength == DBConstant.SqlMaxLength)
            {
                //The maximum number of bytes in the text of an SQL statement is limited to SQLITE_MAX_SQL_LENGTH which defaults to 1000000. You can red
                this.SqlMaxLength = 1000000;
            }
            else
            {
                this.SqlMaxLength = this.Config.SqlMaxLength;
            }

            string name = string.Format("{0}写线程", string.IsNullOrEmpty(this.Config.ConName) ? "数据库编号" + this.Config.DBID.ToString() : this.Config.ConName);
            this._writeQueue = new AsynQueue<SQLiteWriteBase>(name,true);
            this._writeQueue.ProcessAction = this.WriteThreadMethod;
        }

        /// <summary>
        /// 写线程方法
        /// </summary>
        private void WriteThreadMethod(SQLiteWriteBase item)
        {
            try
            {
                item.Excute(this);
                item.ExcuteResult = true;
            }
            catch (Exception ex)
            {
                item.ExcuteResult = false;
                item.Result = ex;
                DBLog.OutLog(ex);
            }
            finally
            {
                if (item != null)
                {
                    item.Set();
                    item = null;
                }
            }
        }

        /// <summary>
        /// 初始化读写连接池
        /// </summary>
        public override void Init()
        {
            this.Config.WriteConCount = 1;
            base.Init();
            this._writeQueue.Start();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="isDisposing">是否释放资源标识</param>
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            this._writeQueue.Dispose();
        }
    }
}
