using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBBase.Interface;
using UtilZ.Dotnet.DBSQLite.Interface;

namespace UtilZ.Dotnet.DBSQLite.Write
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
        /// 条件属性集合
        /// </summary>
        private IEnumerable<string> _conditionProperties;

        /// <summary>
        /// 删除项集合
        /// </summary>
        private IEnumerable<T> _items;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waitTimeout">等待超时时间(-1表示无限等待)</param>
        /// <param name="conditionProperties">条件属性集合[该集合为空或null时仅用主键字段]</param>
        /// <param name="item">删除项集合</param>
        public SQLiteDeleteT(int waitTimeout, T item, IEnumerable<string> conditionProperties) : base(waitTimeout, 1)
        {
            this._item = item;
            this._conditionProperties = conditionProperties;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waitTimeout">等待超时时间(-1表示无限等待)</param>
        /// <param name="items">删除项集合</param>
        public SQLiteDeleteT(int waitTimeout, IEnumerable<T> items) : base(waitTimeout, 2)
        {
            this._items = items;
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
                    this.Result = sqliteDBAccess.BaseDeleteT<T>(this._item, this._conditionProperties);
                    break;
                case 2:
                    this.Result = sqliteDBAccess.BaseBatchDeleteT<T>(this._items);
                    break;
                default:
                    throw new NotImplementedException(string.Format("未实现的泛型插入类型{0}", this._type));
            }

            return this.Result;
        }
    }
}
