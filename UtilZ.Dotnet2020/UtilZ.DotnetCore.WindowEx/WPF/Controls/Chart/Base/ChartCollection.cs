using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.DotnetStd.Ex.Base;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public class ChartCollection<T> : BaseModelAbs, IChartCollection<T>
    {
        private readonly object _sourceLock = new object();
        private readonly List<T> _source = new List<T>();

        public ChartCollection()
        {

        }




        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnRaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var handler = this.CollectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<ChartCollectionChangedEventArgs<T>> ChartCollectionChanged;
        private void OnRaiseChartCollectionChanged(ChartCollectionChangedEventArgs<T> e)
        {
            var handler = this.ChartCollectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }




        public T this[int index]
        {
            get
            {
                lock (this._sourceLock)
                {
                    return _source[index];
                }
            }
            set
            {
                lock (this._sourceLock)
                {
                    var oldItem = _source[index];
                    _source[index] = value;
                    this.OnRaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldItem, index));
                    this.OnRaiseChartCollectionChanged(new ChartCollectionChangedEventArgs<T>(ChartCollectionChangedAction.Replace, value, oldItem));
                }
            }
        }

        public int Count
        {
            get
            {
                lock (this._sourceLock)
                {
                    return _source.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }











        public void Add(T item)
        {
            lock (this._sourceLock)
            {
                this._source.Add(item);
                this.OnRaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
                this.OnRaiseChartCollectionChanged(new ChartCollectionChangedEventArgs<T>(ChartCollectionChangedAction.Add, item, default(T)));
            }
        }


        public void AddRange(IEnumerable<T> items)
        {
            if (items == null || items.Count() == 0)
            {
                return;
            }

            lock (this._sourceLock)
            {
                this._source.AddRange(items);
                foreach (var item in items)
                {
                    this.OnRaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
                }
                this.OnRaiseChartCollectionChanged(new ChartCollectionChangedEventArgs<T>(ChartCollectionChangedAction.Add, items.ToList(), null));
            }
        }

        public void Insert(int index, T item)
        {
            lock (this._sourceLock)
            {
                this._source.Insert(index, item);
                this.OnRaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
                this.OnRaiseChartCollectionChanged(new ChartCollectionChangedEventArgs<T>(ChartCollectionChangedAction.Add, item, default(T)));
            }
        }

        public void InsertRange(int index, IEnumerable<T> items)
        {
            if (items == null || items.Count() == 0)
            {
                return;
            }

            lock (this._sourceLock)
            {
                this._source.InsertRange(index, items);
                foreach (var item in items)
                {
                    this.OnRaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
                }
                this.OnRaiseChartCollectionChanged(new ChartCollectionChangedEventArgs<T>(ChartCollectionChangedAction.Add, items.ToList(), null));
            }
        }





        public bool Remove(T item)
        {
            lock (this._sourceLock)
            {
                var index = this._source.IndexOf(item);
                bool ret = this._source.Remove(item);
                if (ret)
                {
                    this.OnRaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
                    this.OnRaiseChartCollectionChanged(new ChartCollectionChangedEventArgs<T>(ChartCollectionChangedAction.Remove, default(T), item));
                }

                return ret;
            }
        }

        public int RemoveAll(Predicate<T> match)
        {
            if (match == null)
            {
                return 0;
            }

            List<T> list = null;
            lock (this._sourceLock)
            {
                foreach (var item in this._source.ToArray())
                {
                    if (match(item))
                    {
                        if (list == null)
                        {
                            list = new List<T>();
                        }

                        list.Add(item);
                        this.OnRaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
                        this._source.Remove(item);
                    }
                }
            }

            if (list != null)
            {
                this.OnRaiseChartCollectionChanged(new ChartCollectionChangedEventArgs<T>(ChartCollectionChangedAction.Remove, null, list));
                return list.Count;
            }
            else
            {
                return 0;
            }

        }

        public void RemoveAt(int index)
        {
            lock (this._sourceLock)
            {
                var oldItem = this._source.ElementAt(index);
                this._source.RemoveAt(index);
                this.OnRaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, index));
                this.OnRaiseChartCollectionChanged(new ChartCollectionChangedEventArgs<T>(ChartCollectionChangedAction.Remove, default(T), oldItem));
            }
        }



        public void Clear()
        {
            lock (this._sourceLock)
            {
                var list = this._source.ToList();
                this._source.Clear();
                this.OnRaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, null));
                this.OnRaiseChartCollectionChanged(new ChartCollectionChangedEventArgs<T>(ChartCollectionChangedAction.Remove, null, list));
            }
        }



        public bool Contains(T item)
        {
            lock (this._sourceLock)
            {
                return this._source.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (this._sourceLock)
            {
                this._source.CopyTo(array, arrayIndex);
            }
        }



        public int IndexOf(T item)
        {
            lock (this._sourceLock)
            {
                return this._source.IndexOf(item);
            }
        }




        public IEnumerator<T> GetEnumerator()
        {
            lock (_sourceLock)
            {
                return new List<T>(_source).GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
