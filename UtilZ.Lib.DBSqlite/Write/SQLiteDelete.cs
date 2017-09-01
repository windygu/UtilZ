using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Lib.DBBase.Interface;
using UtilZ.Lib.DBModel.Interface;
using UtilZ.Lib.DBModel.Model;
using UtilZ.Lib.DBSqlite.Interface;

namespace UtilZ.Lib.DBSqlite.Write
{
    /// <summary>
    /// SQLite删除操作对象
    /// </summary>
    [Serializable]
    public class SQLiteDelete : SQLiteWriteBase
    {
        /// <summary>
        /// 参数集合
        /// </summary>
        private NDbParameterCollection _collection;

        /// <summary>
        /// 要删除的表条件值集合[key:列名;value:值]
        /// </summary>
        private Dictionary<string, object> _priKeyColValues;

        /// <summary>
        /// Sql语句集合
        /// </summary>
        private IEnumerable<string> _sqlStrs;

        /// <summary>
        /// 要删除的表条件值集合[key:列名;value:值]
        /// </summary>
        private IEnumerable<Dictionary<string, object>> _priKeyColValuess;

        /// <summary>
        /// 参数集合
        /// </summary>
        private IEnumerable<NDbParameterCollection> _collections;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        public SQLiteDelete(string sqlStr, NDbParameterCollection collection) : base(1)
        {
            this._sqlStr = sqlStr;
            this._collection = collection;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">要删除的表名</param>
        /// <param name="priKeyColValues">要删除的表条件值集合[key:列名;value:值]</param>
        public SQLiteDelete(string tableName, Dictionary<string, object> priKeyColValues) : base(2)
        {
            this._tableName = tableName;
            this._priKeyColValues = new Dictionary<string, object>(priKeyColValues);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlStrs">Sql语句集合</param>
        public SQLiteDelete(IEnumerable<string> sqlStrs) : base(3)
        {
            this._sqlStrs = sqlStrs;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColValues">主键列名值字典集合</param>
        public SQLiteDelete(string tableName, IEnumerable<Dictionary<string, object>> priKeyColValues) : base(4)
        {
            this._tableName = tableName;
            this._priKeyColValuess = priKeyColValues;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlStr">Sql语句集合</param>
        /// <param name="collections">参数集合</param>
        public SQLiteDelete(string sqlStr, IEnumerable<NDbParameterCollection> collections) : base(5)
        {
            this._sqlStr = sqlStr;
            this._collections = collections;
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
                    this.Result = sqliteDBAccess.BaseDelete(this._sqlStr, con, this._collection);
                    break;
                case 2:
                    this.Result = sqliteDBAccess.BaseDelete(this._tableName, this._priKeyColValues, con);
                    break;
                case 3:
                    this.Result = sqliteDBAccess.BaseBatchDelete(this._sqlStrs, con);
                    break;
                case 4:
                    this.Result = sqliteDBAccess.BaseBatchDelete(this._tableName, this._priKeyColValuess, con);
                    break;
                case 5:
                    this.Result = sqliteDBAccess.BaseBatchDelete(this._sqlStr, this._collections, con);
                    break;
                default:
                    throw new NotImplementedException(string.Format("未实现的泛型插入类型{0}", this._type));
            }

            return this.Result;
        }
    }
}
