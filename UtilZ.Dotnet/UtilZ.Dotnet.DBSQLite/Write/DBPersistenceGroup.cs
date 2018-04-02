using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBModel.Interface;

namespace UtilZ.Dotnet.DBSQLite.Write
{
    /// <summary>
    /// 数据库持久化组
    /// </summary>
    public class DBPersistenceGroup : IEnumerable<SQLiteWriteBase>
    {
        /// <summary>
        /// 内部集合
        /// </summary>
        private readonly List<SQLiteWriteBase> _items = new List<SQLiteWriteBase>();

        /// <summary>
        /// 线程锁
        /// </summary>
        private readonly object _monitor = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        public DBPersistenceGroup()
        {

        }

        /// <summary>
        /// 组内集合数
        /// </summary>
        public int Count
        {
            get
            {
                lock (_monitor)
                {
                    return this._items.Count;
                }
            }
        }

        /// <summary>
        /// 将项添加到集合
        /// </summary>
        /// <param name="item">要添加的项</param>
        public void Enqueue(SQLiteWriteBase item)
        {
            lock (_monitor)
            {
                this._items.Add(item);
            }
        }

        /// <summary>
        /// 将项添加到集合
        /// </summary>
        /// <param name="items">要添加的集合</param>
        public void Enqueue(IEnumerable<SQLiteWriteBase> items)
        {
            lock (_monitor)
            {
                this._items.AddRange(items);
            }
        }

        /// <summary>
        /// 返回集合内全部集合,不移除
        /// </summary>
        /// <returns>集合内全部集合</returns>
        public List<SQLiteWriteBase> Peek()
        {
            List<SQLiteWriteBase> items;
            lock (_monitor)
            {
                items = new List<SQLiteWriteBase>(this._items);
            }

            return items;
        }

        /// <summary>
        /// 返回集合内全部集合,并移除
        /// </summary>
        /// <returns>集合内全部集合</returns>
        public List<SQLiteWriteBase> Dequeue()
        {
            List<SQLiteWriteBase> items;
            lock (_monitor)
            {
                items = new List<SQLiteWriteBase>(this._items);
                this._items.Clear();
            }

            return items;
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举数
        /// </summary>
        /// <returns>循环访问集合的枚举数</returns>
        public IEnumerator<SQLiteWriteBase> GetEnumerator()
        {
            return new List<SQLiteWriteBase>(this._items).GetEnumerator();
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举数
        /// </summary>
        /// <returns>循环访问集合的枚举数</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
