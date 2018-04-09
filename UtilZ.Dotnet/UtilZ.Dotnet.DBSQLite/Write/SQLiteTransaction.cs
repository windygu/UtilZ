using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBBase.Interface;
using UtilZ.Dotnet.DBSQLite.Core;
using UtilZ.Dotnet.DBSQLite.Interface;

namespace UtilZ.Dotnet.DBSQLite.Write
{
    /// <summary>
    /// SQLite泛型删除操作对象
    /// </summary>
    [Serializable]
    public class SQLiteTransaction : SQLiteWriteBase
    {
        /// <summary>
        /// 参数
        /// </summary>
        private object _para;

        /// <summary>
        /// 事务执行委托
        /// </summary>
        private Func<IDbConnection, IDbTransaction, object, object> _function;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waitTimeout">等待超时时间(-1表示无限等待)</param>
        /// <param name="para">参数</param>
        /// <param name="function">事务执行委托</param>
        public SQLiteTransaction(int waitTimeout, object para, Func<IDbConnection, IDbTransaction, object, object> function) : base(waitTimeout, -1)
        {
            this._para = para;
            this._function = function;
        }

        /// <summary>
        /// 执行写入操作
        /// </summary>
        /// <param name="sqliteDBAccess">SQLite数据库访问对象</param>
        public override object Excute(ISQLiteDBAccessBase sqliteDBAccess)
        {
            ISQLiteDBAccessBase sqliteDBAccessBase = (ISQLiteDBAccessBase)sqliteDBAccess;
            this.Result = sqliteDBAccessBase.BaseExcuteAdoNetTransaction(this._para, this._function);
            return this.Result;
        }
    }
}
