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
    /// SQLite插入操作对象
    /// </summary>
    [Serializable]
    public class SQLiteInsert : SQLiteWriteBase
    {
        /// <summary>
        /// 参数集合
        /// </summary>
        private NDbParameterCollection _collection;

        /// <summary>
        /// 参数集合集合
        /// </summary>
        private IEnumerable<NDbParameterCollection> _collections;

        /// <summary>
        /// 列名集合
        /// </summary>
        private IEnumerable<string> _cols;

        /// <summary>
        /// 数据
        /// </summary>
        private IEnumerable<object[]> _data;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waitTimeout">等待超时时间(-1表示无限等待)</param>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        public SQLiteInsert(int waitTimeout, string sqlStr, NDbParameterCollection collection) : base(waitTimeout, 1)
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
        public SQLiteInsert(int waitTimeout, string sqlStr, IEnumerable<NDbParameterCollection> collections) : base(waitTimeout, 2)
        {
            this._sqlStr = sqlStr;
            this._collections = collections;
        }

        /// <summary>
        /// 批量插入数据,返回受影响的行数
        /// </summary>
        /// <param name="waitTimeout">等待超时时间(-1表示无限等待)</param>
        /// <param name="tableName">表名</param>
        /// <param name="cols">列名集合</param>
        /// <param name="data">数据</param>
        public SQLiteInsert(int waitTimeout, string tableName, IEnumerable<string> cols, IEnumerable<object[]> data) : base(waitTimeout, 3)
        {
            this._tableName = tableName;
            this._cols = cols;
            this._data = data;
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
                    this.Result = sqliteDBAccess.BaseInsert(this._sqlStr, this._collection);
                    break;
                case 2:
                    this.Result = sqliteDBAccess.BaseBatchInsert(this._sqlStr, this._collections);
                    break;
                case 3:
                    this.Result = sqliteDBAccess.BaseBatchInsert(this._tableName, this._cols, this._data);
                    break;
                default:
                    throw new NotImplementedException(string.Format("未实现的泛型插入类型{0}", this._type));
            }

            return this.Result;
        }
    }
}
