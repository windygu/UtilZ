using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBIBase.DBBase.Interface;
using UtilZ.Dotnet.DBIBase.DBModel.Interface;
using UtilZ.Dotnet.DBIBase.DBModel.Model;
using UtilZ.Dotnet.DBSQLite.Interface;

namespace UtilZ.Dotnet.DBSQLite.Write
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
        /// <param name="waitTimeout">等待超时时间(-1表示无限等待)</param>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        public SQLiteDelete(int waitTimeout, string sqlStr, NDbParameterCollection collection) : base(waitTimeout, 1)
        {
            this._sqlStr = sqlStr;
            this._collection = collection;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waitTimeout">等待超时时间(-1表示无限等待)</param>
        /// <param name="tableName">要删除的表名</param>
        /// <param name="priKeyColValues">要删除的表条件值集合[key:列名;value:值]</param>
        public SQLiteDelete(int waitTimeout, string tableName, Dictionary<string, object> priKeyColValues) : base(waitTimeout, 2)
        {
            this._tableName = tableName;
            this._priKeyColValues = new Dictionary<string, object>(priKeyColValues);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waitTimeout">等待超时时间(-1表示无限等待)</param>
        /// <param name="sqlStrs">Sql语句集合</param>
        public SQLiteDelete(int waitTimeout, IEnumerable<string> sqlStrs) : base(waitTimeout, 3)
        {
            this._sqlStrs = sqlStrs;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waitTimeout">等待超时时间(-1表示无限等待)</param>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColValues">主键列名值字典集合</param>
        public SQLiteDelete(int waitTimeout, string tableName, IEnumerable<Dictionary<string, object>> priKeyColValues) : base(waitTimeout, 4)
        {
            this._tableName = tableName;
            this._priKeyColValuess = priKeyColValues;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waitTimeout">等待超时时间(-1表示无限等待)</param>
        /// <param name="sqlStr">Sql语句集合</param>
        /// <param name="collections">参数集合</param>
        public SQLiteDelete(int waitTimeout, string sqlStr, IEnumerable<NDbParameterCollection> collections) : base(waitTimeout, 5)
        {
            this._sqlStr = sqlStr;
            this._collections = collections;
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
                    this.Result = sqliteDBAccess.BaseDelete(this._sqlStr, this._collection);
                    break;
                case 2:
                    this.Result = sqliteDBAccess.BaseDelete(this._tableName, this._priKeyColValues);
                    break;
                case 3:
                    this.Result = sqliteDBAccess.BaseBatchDelete(this._sqlStrs);
                    break;
                case 4:
                    this.Result = sqliteDBAccess.BaseBatchDelete(this._tableName, this._priKeyColValuess);
                    break;
                case 5:
                    this.Result = sqliteDBAccess.BaseBatchDelete(this._sqlStr, this._collections);
                    break;
                default:
                    throw new NotImplementedException(string.Format("未实现的泛型插入类型{0}", this._type));
            }

            return this.Result;
        }
    }
}
