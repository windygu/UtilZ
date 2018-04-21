using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBIBase.DBBase.Interface;
using UtilZ.Dotnet.DBIBase.DBModel.Model;
using UtilZ.Dotnet.DBSQLite.Interface;

namespace UtilZ.Dotnet.DBSQLite.Write
{
    /// <summary>
    /// SQLite更新操作对象
    /// </summary>
    [Serializable]
    public class SQLiteUpdate : SQLiteWriteBase
    {
        /// <summary>
        /// Sql语句集合
        /// </summary>
        private IEnumerable<string> _sqlStrs;

        /// <summary>
        /// 参数集合
        /// </summary>
        private NDbParameterCollection _collection;

        /// <summary>
        /// 参数集合
        /// </summary>
        private IEnumerable<NDbParameterCollection> _collections;

        /// <summary>
        /// 要删除的表条件值集合[key:列名;value:值]
        /// </summary>
        private Dictionary<string, object> _priKeyColValues;

        /// <summary>
        /// 修改列名值字典
        /// </summary>
        private Dictionary<string, object> _colValues;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waitTimeout">等待超时时间(-1表示无限等待)</param>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColValues">主键列名值映射字典</param>
        /// <param name="colValues">修改列名值字典</param>
        public SQLiteUpdate(int waitTimeout, string tableName, Dictionary<string, object> priKeyColValues, Dictionary<string, object> colValues) : base(waitTimeout, 1)
        {
            this._tableName = tableName;
            this._priKeyColValues = priKeyColValues;
            this._colValues = colValues;
        }

        /// <summary>
        /// 批量更新记录,返回受影响的行数
        /// </summary>
        /// <param name="waitTimeout">等待超时时间(-1表示无限等待)</param>
        /// <param name="sqlStrs">Sql语句集合</param>
        public SQLiteUpdate(int waitTimeout, IEnumerable<string> sqlStrs) : base(waitTimeout, 2)
        {
            this._sqlStrs = sqlStrs;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waitTimeout">等待超时时间(-1表示无限等待)</param>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        public SQLiteUpdate(int waitTimeout, string sqlStr, NDbParameterCollection collection) : base(waitTimeout, 3)
        {
            this._sqlStr = sqlStr;
            this._collection = collection;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waitTimeout">等待超时时间(-1表示无限等待)</param>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collections">命令的参数集合</param>
        public SQLiteUpdate(int waitTimeout, string sqlStr, IEnumerable<NDbParameterCollection> collections) : base(waitTimeout, 4)
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
                    this.Result = sqliteDBAccess.BaseUpdate(this._tableName, this._priKeyColValues, this._colValues);
                    break;
                case 2:
                    this.Result = sqliteDBAccess.BaseBatchUpdate(this._sqlStrs);
                    break;
                case 3:
                    this.Result = sqliteDBAccess.BaseUpdate(this._sqlStr, this._collection);
                    break;
                case 4:
                    this.Result = sqliteDBAccess.BaseBatchUpdate(this._sqlStr, this._collections);
                    break;
                default:
                    throw new NotImplementedException(string.Format("未实现的泛型插入类型{0}", this._type));
            }

            return this.Result;
        }
    }
}
