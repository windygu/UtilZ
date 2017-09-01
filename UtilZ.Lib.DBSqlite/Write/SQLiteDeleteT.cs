using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Lib.DBBase.Interface;
using UtilZ.Lib.DBSqlite.Interface;

namespace UtilZ.Lib.DBSqlite.Write
{
    /// <summary>
    /// SQLite泛型删除操作对象
    /// </summary>
    [Serializable]
    public class SQLiteDeleteT<T> : SQLiteWriteBase where T : class
    {
        /// <summary>
        /// 删除项
        /// </summary>
        private T _item;

        /// <summary>
        /// 删除项集合
        /// </summary>
        private IEnumerable<T> _items;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="item">删除项集合</param>
        public SQLiteDeleteT(T item) : base(1)
        {
            this._item = item;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="items">删除项集合</param>
        public SQLiteDeleteT(IEnumerable<T> items) : base(2)
        {
            this._items = items;
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
                    this.Result = sqliteDBAccess.BaseDeleteT<T>(this._item, con);
                    break;
                case 2:
                    this.Result = sqliteDBAccess.BaseBatchDeleteT<T>(this._items, con);
                    break;
                default:
                    throw new NotImplementedException(string.Format("未实现的泛型插入类型{0}", this._type));
            }

            return this.Result;
        }
    }
}
