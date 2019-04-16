using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.Base
{
    [Serializable]
    public class BindingCollection<T> : IEnumerable<T> where T : new()
    {
        private readonly ICollectionOwner _owner;
        private readonly Collection<T> _items;
        public readonly object SyncRoot;

        public BindingCollection(ICollectionOwner owner) :
            this(owner, CreateCollectionByOwner(owner))
        {
        }

        public BindingCollection(ICollectionOwner owner, Collection<T> items)
        {
            this.SyncRoot = new object();
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            this._owner = owner;
            this._items = items;
        }

        public void Add(T item)
        {
            //this.OnRaiseInvokeAction(delegate
            //{
            //    object syncRoot = ((BindingCollection<T>)this).SyncRoot;
            //    lock (syncRoot)
            //    {
            //        ((BindingCollection<T>)this)._items.Add(item);
            //    }
            //});
        }

        //public void AddNew()
        //{
        //    this.OnRaiseInvokeAction(delegate
        //    {
        //        lock (this.SyncRoot)
        //        {
        //            T item = Activator.CreateInstance<T>();
        //            this._items.Insert(0, item);
        //        }
        //    });
        //}

        //public void AddNewCore()
        //{
        //    this.OnRaiseInvokeAction(delegate
        //    {
        //        object syncRoot = base.SyncRoot;
        //        lock (syncRoot)
        //        {
        //            T item = Activator.CreateInstance<T>();
        //            base._items.Add(item);
        //        }
        //    });
        //}

        public void AddRange(IEnumerable<T> items)
        {
            int num = items.Count<T>();
            if ((items != null) && (num != 0))
            {
                this.OnRaiseInvokeAction(delegate
                {
                    object syncRoot = ((BindingCollection<T>)this).SyncRoot;
                    lock (syncRoot)
                    {
                        foreach (T local in items)
                        {
                            ((BindingCollection<T>)this)._items.Add(local);
                        }
                    }
                });
            }
        }

        public void Clear()
        {
            //this.OnRaiseInvokeAction(delegate
            //{
            //    object syncRoot = base.SyncRoot;
            //    lock (syncRoot)
            //    {
            //        base._items.Clear();
            //    }
            //});
        }

        public bool Contains(T item)
        {
            object syncRoot = this.SyncRoot;
            lock (syncRoot)
            {
                return this._items.Contains(item);
            }
        }

        public void CopyTo(T[] array, int index)
        {
            object syncRoot = this.SyncRoot;
            lock (syncRoot)
            {
                this._items.CopyTo(array, index);
            }
        }

        private static Collection<T> CreateCollectionByOwner(object owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }
            Type baseType = owner.GetType();
            while (!baseType.FullName.Equals("System.Object"))
            {
                if (baseType.FullName.Equals("System.Windows.Forms.Control"))
                {
                    return new BindingListEx<T>();
                }
                if (baseType.FullName.Equals("System.Windows.Threading.DispatcherObject"))
                {
                    return new ObservableCollection<T>();
                }
                baseType = baseType.BaseType;
            }
            throw new ArgumentException($"类型{baseType.FullName}必须继承{"System.Windows.Forms.Control"}或{"System.Windows.Threading.DispatcherObject"}类型");
        }

        public IEnumerator<T> GetEnumerator() =>
            this._items.GetEnumerator();

        public int IndexOf(T item)
        {
            object syncRoot = this.SyncRoot;
            lock (syncRoot)
            {
                return this._items.IndexOf(item);
            }
        }

        public void Insert(int index, T item)
        {
            this.OnRaiseInvokeAction(delegate
            {
                object syncRoot = ((BindingCollection<T>)this).SyncRoot;
                lock (syncRoot)
                {
                    ((BindingCollection<T>)this)._items.Insert(index, item);
                }
            });
        }

        protected void OnRaiseInvokeAction(Action action)
        {
            ICollectionOwner owner = this._owner;
            if ((owner != null) && owner.InvokeRequired)
            {
                owner.Invoke(action, new object[0]);
            }
            else
            {
                action();
            }
        }

        public bool Remove(T item)
        {
            object syncRoot = this.SyncRoot;
            lock (syncRoot)
            {
                //if (this._items.Contains(item))
                //{
                //    ICollectionOwner owner = this._owner;
                //    if ((owner != null) && owner.InvokeRequired)
                //    {
                //        object[] args = new object[] { item };
                //        return (bool)owner.Invoke(t => ((BindingCollection<T>)this)._items.Remove(item), args);
                //    }
                //    return this._items.Remove(item);
                //}
                return false;
            }
        }

        public void RemoveArea(int index, int count)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            //this.OnRaiseInvokeAction(delegate
            //{
            //    object syncRoot = ((BindingCollection<T>)this).SyncRoot;
            //    lock (syncRoot)
            //    {
            //        int num = index + count;
            //        if (((BindingCollection<T>)this)._items.Count < num)
            //        {
            //            throw new ArgumentOutOfRangeException();
            //        }
            //        for (int i = index; i < num; i++)
            //        {
            //            ((BindingCollection<T>)this)._items.RemoveAt(index);
            //        }
            //    }
            //});
        }

        public void RemoveArrange(IEnumerable<T> items)
        {
            if ((items != null) && (items.Count<T>() != 0))
            {
                //this.OnRaiseInvokeAction(delegate
                //{
                //    object syncRoot = ((BindingCollection<T>)this).SyncRoot;
                //    lock (syncRoot)
                //    {
                //        foreach (T local in items)
                //        {
                //            if (((BindingCollection<T>)this)._items.Contains(local))
                //            {
                //                ((BindingCollection<T>)this)._items.Remove(local);
                //            }
                //        }
                //    }
                //});
            }
        }

        public void RemoveAt(int index)
        {
            //this.OnRaiseInvokeAction(delegate
            //{
            //    object syncRoot = ((BindingCollection<T>)this).SyncRoot;
            //    lock (syncRoot)
            //    {
            //        ((BindingCollection<T>)this)._items.RemoveAt(index);
            //    }
            //});
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        //IEnumerator IEnumerable.GetEnumerator() =>
        //    this.GetEnumerator();

        public Collection<T> DataSource =>
            this._items;

        public T this[int index]
        {
            get
            {
                object syncRoot = this.SyncRoot;
                lock (syncRoot)
                {
                    return this._items[index];
                }
            }
            set
            {
                object syncRoot = this.SyncRoot;
                lock (syncRoot)
                {
                    this._items[index] = value;
                }
            }
        }

        public int Count
        {
            get
            {
                object syncRoot = this.SyncRoot;
                lock (syncRoot)
                {
                    return this._items.Count;
                }
            }
        }

        public bool IsReadOnly =>
            false;
    }
}
