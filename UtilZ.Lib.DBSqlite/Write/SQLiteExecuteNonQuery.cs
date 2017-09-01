using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Lib.DBBase.Interface;
using UtilZ.Lib.DBModel.Model;
using UtilZ.Lib.DBSqlite.Interface;

namespace UtilZ.Lib.DBSqlite.Write
{
    /// <summary>
    /// SQLiteExecuteNonQuery操作对象
    /// </summary>
    [Serializable]
    public class SQLiteExecuteNonQuery : SQLiteWriteBase
    {
        /// <summary>
        /// 参数集合
        /// </summary>
        private NDbParameterCollection _collection;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        public SQLiteExecuteNonQuery(string sqlStr, NDbParameterCollection collection) : base(1)
        {
            this._sqlStr = sqlStr;
            this._collection = collection;
        }

        /// <summary>
        /// 执行写入操作
        /// </summary>
        /// <param name="sqliteDBAccess">SQLite数据库访问对象</param>
        /// <param name="con">数据库连接</param>
        public override object Excute(ISQLiteDBAccessBase sqliteDBAccess, IDbConnection con)
        {
            switch (this._type)
            {
                case 1:
                    this.Result = sqliteDBAccess.BaseExecuteNonQuery(this._sqlStr, DBVisitType.W, con, this._collection);
                    break;
                default:
                    throw new NotImplementedException(string.Format("未实现的写类型{0}", this._type));
            }

            return this.Result;
        }
    }
}
