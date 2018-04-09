using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBBase.Interface;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBModel.Model;
using UtilZ.Dotnet.DBSQLite.Interface;

namespace UtilZ.Dotnet.DBSQLite.Write
{
    /// <summary>
    /// SQLite数据顺序写操作基类
    /// </summary>
    public abstract class SQLiteWriteBase : IDisposable
    {
        /// <summary>
        /// 等待超时时间(-1表示无限等待)
        /// </summary>
        private readonly int _waitTimeout = System.Threading.Timeout.Infinite;

        /// <summary>
        /// 实例ID
        /// </summary>
        public Guid ID { get; private set; }

        /// <summary>
        /// 写入同步操作线程同步对象
        /// </summary>
        private readonly AutoResetEvent _writeAutoResetEvent;

        /// <summary>
        /// 执行结果[true:执行成功;false:执行异常]
        /// </summary>
        public bool ExcuteResult { get; internal set; }

        /// <summary>
        /// 执行结果值
        /// </summary>
        public object Result { get; internal set; }

        /// <summary>
        /// 执行类型
        /// </summary>
        protected readonly int _type = -1;

        /// <summary>
        /// Sql语句
        /// </summary>
        protected string _sqlStr;

        /// <summary>
        /// 表名
        /// </summary>
        protected string _tableName;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waitTimeout">等待超时时间(-1表示无限等待)</param>
        /// <param name="type">执行类型</param>
        protected SQLiteWriteBase(int waitTimeout, int type)
        {
            this._waitTimeout = waitTimeout;
            this.ID = Guid.NewGuid();
            this._writeAutoResetEvent = new AutoResetEvent(false);
            this._type = type;
        }

        /// <summary>
        /// 通知当前操作已被执行完成
        /// </summary>
        internal void Set()
        {
            this._writeAutoResetEvent.Set();
        }

        /// <summary>
        /// 等等当前操作被执行
        /// </summary>
        /// <returns>等待结果</returns>
        public bool WaitOne()
        {
            return this._writeAutoResetEvent.WaitOne(this._waitTimeout);
        }

        /// <summary>
        /// 执行写入操作
        /// </summary>
        /// <param name="sqliteDBAccess">SQLite数据库访问对象</param>
        public abstract object Excute(ISQLiteDBAccessBase sqliteDBAccess);

        #region IDisposable
        /// <summary>
        /// 资源是否已释放过
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Dispose(this._disposed);
            this._disposed = true;
        }

        /// <summary>
        /// 释放资源方法
        /// </summary>
        /// <param name="isDispose">是否释放标识</param>
        protected virtual void Dispose(bool isDispose)
        {
            if (isDispose)
            {
                return;
            }

            if (this._writeAutoResetEvent != null)
            {
                this._writeAutoResetEvent.Dispose();
            }
        }
        #endregion
    }
}
