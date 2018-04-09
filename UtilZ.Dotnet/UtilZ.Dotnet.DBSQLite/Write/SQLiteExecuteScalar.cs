using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBBase.Interface;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBModel.Model;
using UtilZ.Dotnet.DBSQLite.Interface;

namespace UtilZ.Dotnet.DBSQLite.Write
{
    /// <summary>
    /// SQLiteExecuteScalar操作对象
    /// </summary>
    [Serializable]
    public class SQLiteExecuteScalar : SQLiteWriteBase
    {
        /// <summary>
        /// 参数集合
        /// </summary>
        private NDbParameterCollection _collection;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waitTimeout">等待超时时间(-1表示无限等待)</param>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        public SQLiteExecuteScalar(int waitTimeout, string sqlStr, NDbParameterCollection collection) : base(waitTimeout, 1)
        {
            this._sqlStr = sqlStr;
            this._collection = collection;
        }

        /// <summary>
        /// 执行写入操作
        /// </summary>
        /// <param name="sqliteDBAccess">SQLite数据库访问对象</param>
        public override object Excute(ISQLiteDBAccessBase sqliteDBAccess)
        {
            switch (this._type)
            {
                case 1:
                    this.Result = sqliteDBAccess.BaseExecuteScalar(this._sqlStr, DBVisitType.W, this._collection);
                    break;
                default:
                    throw new NotImplementedException(string.Format("未实现的写类型{0}", this._type));
            }

            return this.Result;
        }
    }
}
