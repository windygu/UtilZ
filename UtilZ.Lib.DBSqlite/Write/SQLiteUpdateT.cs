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
    /// SQLite泛型更新操作对象
    /// </summary>
    [Serializable]
    public class SQLiteUpdateT<T> : SQLiteWriteBase where T : class
    {
        /// <summary>
        /// 要更新到数据库表的属性名称集合,为null时全部更新
        /// </summary>
        private readonly IEnumerable<string> _updateProperties;

        /// <summary>
        /// 更新项集合
        /// </summary>
        private readonly T _item;

        /// <summary>
        /// 更新项集合
        /// </summary>
        private readonly IEnumerable<T> _items;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="updateProperties">要更新到数据库表的属性名称集合,为null时全部更新</param>
        /// <param name="item">更新项</param>
        public SQLiteUpdateT(IEnumerable<string> updateProperties, T item) : base(1)
        {
            this._item = item;
            this._updateProperties = updateProperties;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="updateProperties">要更新到数据库表的属性名称集合,为null时全部更新</param>
        /// <param name="items">更新项集合</param>
        public SQLiteUpdateT(IEnumerable<string> updateProperties, IEnumerable<T> items) : base(2)
        {
            this._items = items;
            this._updateProperties = updateProperties;
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
                    this.Result = sqliteDBAccess.BaseUpdateT<T>(this._item, this._updateProperties, con);
                    break;
                case 2:
                    this.Result = sqliteDBAccess.BaseBatchUpdateT<T>(this._items, this._updateProperties, con);
                    break;
                default:
                    throw new NotImplementedException(string.Format("未实现的泛型插入类型{0}", this._type));
            }

            return this.Result;
        }
    }
}
