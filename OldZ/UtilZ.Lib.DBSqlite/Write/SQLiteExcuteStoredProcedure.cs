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
    /// SQLiteExcuteStoredProcedure操作对象
    /// </summary>
    [Serializable]
    public class SQLiteExcuteStoredProcedure : SQLiteWriteBase
    {
        /// <summary>
        /// 参数
        /// </summary>
        private StoredProcedurePara _para;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waitTimeout">等待超时时间(-1表示无限等待)</param>
        /// <param name="para">参数</param>
        public SQLiteExcuteStoredProcedure(int waitTimeout, StoredProcedurePara para) : base(waitTimeout, 1)
        {
            this._para = para;
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
                    this.Result = sqliteDBAccess.BaseExcuteStoredProcedure(this._para);
                    break;
                default:
                    throw new NotImplementedException(string.Format("未实现的写类型{0}", this._type));
            }

            return this.Result;
        }
    }
}
