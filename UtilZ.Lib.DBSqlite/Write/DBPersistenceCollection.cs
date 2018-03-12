using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.Foundation;
using UtilZ.Lib.Base.Ex;
using UtilZ.Lib.DBModel.Interface;

namespace UtilZ.Lib.DBSqlite.Write
{
    /// <summary>
    /// 数据库持久化集合
    /// </summary>
    public class DBPersistenceCollection : IEnumerable<DBPersistenceGroup>
    {
        /// <summary>
        /// 内部集合
        /// </summary>
        private readonly Dictionary<Type, DBPersistenceGroup> _dicGroups = new Dictionary<Type, DBPersistenceGroup>();

        /// <summary>
        /// 线程锁
        /// </summary>
        private readonly object _monitor = new object();

        /// <summary>
        /// 添加项后事件
        /// </summary>
        public event EventHandler Added;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DBPersistenceCollection()
        {

        }

        /// <summary>
        /// 集合项数
        /// </summary>
        public int Count
        {
            get
            {
                return this._dicGroups.Count;
            }
        }

        /// <summary>
        /// 所有object项数
        /// </summary>
        public int AllCount
        {
            get
            {
                int count = 0;
                lock (this._monitor)
                {
                    foreach (var group in this._dicGroups.Values)
                    {
                        count += group.Count;
                    }
                }

                return count;
            }
        }

        /// <summary>
        /// 添加项
        /// </summary>
        /// <param name="type">数据模型类型</param>
        /// <param name="item">添加项</param>
        public void Add(Type type, SQLiteWriteBase item)
        {
            DBPersistenceGroup group;
            if (this._dicGroups.ContainsKey(type))
            {
                group = this._dicGroups[type];
            }
            else
            {
                lock (this._monitor)
                {
                    if (this._dicGroups.ContainsKey(type))
                    {
                        group = this._dicGroups[type];
                    }
                    else
                    {
                        group = new DBPersistenceGroup();
                        this._dicGroups.Add(type, group);
                    }
                }
            }

            group.Enqueue(item);
            this.Added.OnRaise(this);
        }

        /// <summary>
        /// 添加项
        /// </summary>
        /// <param name="type">数据模型类型</param>
        /// <param name="items">添加项集合</param>
        public void Add(Type type, IEnumerable<SQLiteWriteBase> items)
        {
            DBPersistenceGroup group;
            if (this._dicGroups.ContainsKey(type))
            {
                group = this._dicGroups[type];
            }
            else
            {
                lock (this._monitor)
                {
                    if (this._dicGroups.ContainsKey(type))
                    {
                        group = this._dicGroups[type];
                    }
                    else
                    {
                        group = new DBPersistenceGroup();
                        this._dicGroups.Add(type, group);
                    }
                }
            }

            group.Enqueue(items);
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举数
        /// </summary>
        /// <returns>循环访问集合的枚举数</returns>
        public IEnumerator<DBPersistenceGroup> GetEnumerator()
        {
            return new List<DBPersistenceGroup>(this._dicGroups.Values).GetEnumerator();
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
