using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Lib.DBBase.Interface;
using UtilZ.Lib.DBSqlite.Core;
using UtilZ.Lib.DBSqlite.Interface;

namespace UtilZ.Lib.DBSqlite.Write
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
        /// <param name="para">参数</param>
        /// <param name="function">事务执行委托</param>
        public SQLiteTransaction(object para, Func<IDbConnection, IDbTransaction, object, object> function) : base(-1)
        {
            this._para = para;
            this._function = function;
        }

        /// <summary>
        /// 执行写入操作
        /// </summary>
        /// <param name="sqliteDBAccess">SQLite数据库访问对象</param>
        /// <param name="con">数据库连接</param>
        public override object Excute(ISQLiteDBAccessBase sqliteDBAccess, IDbConnection con)
        {
            ISQLiteDBAccessBase sqliteDBAccessBase = (ISQLiteDBAccessBase)sqliteDBAccess;
            this.Result = sqliteDBAccessBase.BaseExcuteAdoNetTransaction(this._para, this._function, con);
            return this.Result;
        }
    }
}
