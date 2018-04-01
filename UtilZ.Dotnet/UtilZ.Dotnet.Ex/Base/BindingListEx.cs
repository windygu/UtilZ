using UtilZ.Dotnet.Ex.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.Base
{
    /// <summary>
    /// UI绑定集合
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    [Serializable]
    public class BindingListEx<T> : BindingList<T>
    {
        /// <summary>
        /// 列表是否是环形结构[true:环形显示列表;false:非环形显示列表]
        /// </summary>
        private readonly bool _isRingStruct;

        /// <summary>
        /// 环形结构在容量满之后更新数据的位置索引,默认为0
        /// </summary>
        private int _ringPosition = 0;

        /// <summary>
        /// 重置环形结构在容量满之后更新数据的位置索引
        /// </summary>
        private void ResetRingPosition()
        {
            this._ringPosition = 0;
        }

        /// <summary>
        /// 更新环形结构在容量满之后更新数据的位置索引
        /// </summary>
        /// <param name="flag">是否是增加项标识[true:添加项;false:减少项]</param>
        private void UpdateRingPosition(bool flag)
        {
            if (flag)
            {
                this._ringPosition++;
                if (this._ringPosition >= this.Capcity)
                {
                    this._ringPosition = 0;
                }
            }
            else
            {
                if (base.Items.Count == 0 || this._ringPosition >= base.Items.Count)
                {
                    this._ringPosition = 0;
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="invokeAction">主线程执行委托[Action:主线程要执行的委托;int:优先级[值越小优先级越高,越大越低]]</param>
        public BindingListEx(Action<Action, int> invokeAction = null)
            : base()
        {
            this._isRingStruct = false;
            this.InvokeAction = invokeAction;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="capcity">环形结构时的容量</param>
        public BindingListEx(int capcity)
            : base()
        {
            this._isRingStruct = true;
            this.Capcity = capcity;
        }

        /// <summary>
        /// UI绑定集合发生更改事件
        /// </summary>
        public event EventHandler<BindingListChangedArgs<T>> BindingListChanged;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">改变类型</param>
        /// <param name="item">新项</param>
        /// <param name="oldItem">旧项</param>
        /// <param name="newItems">新项集合</param>
        /// <param name="oldTems">旧项集合</param>
        private void OnBindingListChanged(BindingListChangedType type, T item, T oldItem, List<T> newItems, List<T> oldTems)
        {
            if (this.BindingListChanged != null)
            {
                this.BindingListChanged(this, new BindingListChangedArgs<T>(type, item, oldItem, newItems, oldTems, base.Items.Count, base.IndexOf(item)));
            }
        }

        /// <summary>
        /// 主线程执行委托[Action:主线程要执行的委托;int:优先级[值越小优先级越高,越大越低]]
        /// </summary>
        private Action<Action, int> InvokeAction;

        /// <summary>
        /// 触发一个Action
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="priorityLevel">优先级[值越小优先级越高,越大越低]</param>
        protected void OnRaiseInvokeAction(Action action, int priorityLevel)
        {
            if (InvokeAction == null)
            {
                action();
            }
            else
            {
                this.InvokeAction(action, priorityLevel);
            }
        }

        /// <summary>
        /// 集合改变锁
        /// </summary>
        private readonly object _collectionChangeMonitor = new object();

        /// <summary>
        /// 添加项到末尾
        /// </summary>
        /// <param name="item">要添加的新项</param>
        public virtual new void Add(T item)
        {
            this.Add(item, this._defaultPriority);
        }

        /// <summary>
        /// 重载Add,添加项到末尾
        /// </summary>
        /// <param name="item">要添加的新项</param>
        /// <param name="priorityLevel">优先级[值越小优先级越高,越大越低]</param>
        public virtual void Add(T item, int priorityLevel)
        {
            this.OnRaiseInvokeAction(() =>
            {
                lock (this._collectionChangeMonitor)
                {
                    if (this._isRingStruct && base.Items.Count >= this.Capcity)
                    {
                        T oldItem = this[this._ringPosition];
                        //this[this._ringPosition] = item;
                        base.SetItem(this._ringPosition, item);
                        this.OnBindingListChanged(BindingListChangedType.Update, item, oldItem, null, null);
                        //更新环形结构的索引
                        this.UpdateRingPosition(true);
                    }
                    else
                    {
                        base.Add(item);
                        this.OnBindingListChanged(BindingListChangedType.Add, item, default(T), null, null);
                    }
                }
            }, priorityLevel);
        }

        /// <summary>
        /// 隐藏重写AddNew,将新项添加集合开始
        /// </summary>
        public virtual new void AddNew()
        {
            this.AddNew(this._defaultPriority);
        }

        /// <summary>
        /// 重载AddNew,将新项添加集合开始
        /// </summary>
        /// <param name="priorityLevel">优先级[值越小优先级越高,越大越低]</param>
        public virtual void AddNew(int priorityLevel)
        {
            this.OnRaiseInvokeAction(() =>
            {
                lock (this._collectionChangeMonitor)
                {
                    T item = base.AddNew();
                    if (this._isRingStruct && base.Items.Count >= this.Capcity)
                    {
                        int lastItemIndex = base.Items.Count - 1;//得到最新项的索引
                        T newItem = base[lastItemIndex];//取回最新项
                        base.RemoveAt(lastItemIndex);//移除最新项
                        T oldItem = base[this._ringPosition];//取回旧项
                        base[this._ringPosition] = newItem;//更新旧项位置的数据项为新项
                        this.OnBindingListChanged(BindingListChangedType.Update, newItem, oldItem, null, null);//发送更改通知
                        //更新环形结构的索引
                        this.UpdateRingPosition(true);
                    }
                    else
                    {
                        this.OnBindingListChanged(BindingListChangedType.Add, item, default(T), null, null);
                    }
                }
            }, priorityLevel);
        }

        /// <summary>
        /// 隐藏重写AddNewCore,将新项添加到末尾
        /// </summary>
        public virtual new void AddNewCore()
        {
            this.AddNewCore(this._defaultPriority);
        }

        /// <summary>
        /// 重载AddNewCore,将新项添加到末尾
        /// </summary>
        /// <param name="priorityLevel">优先级[值越小优先级越高,越大越低]</param>
        public virtual void AddNewCore(int priorityLevel)
        {
            this.OnRaiseInvokeAction(() =>
            {
                lock (this._collectionChangeMonitor)
                {
                    T item = (T)base.AddNewCore();
                    if (this._isRingStruct && base.Items.Count >= this.Capcity)
                    {
                        int lastItemIndex = base.Items.Count - 1;//得到最新项的索引
                        T newItem = base[lastItemIndex];//取回最新项
                        base.RemoveAt(lastItemIndex);//移除最新项
                        T oldItem = base[this._ringPosition];//取回旧项
                        base[this._ringPosition] = newItem;//更新旧项位置的数据项为新项
                        this.OnBindingListChanged(BindingListChangedType.Update, newItem, oldItem, null, null);//发送更改通知
                        //更新环形结构的索引
                        this.UpdateRingPosition(true);
                    }
                    else
                    {
                        this.OnBindingListChanged(BindingListChangedType.Add, item, default(T), null, null);
                    }
                }
            }, priorityLevel);
        }

        /// <summary>
        /// 添加集合到末尾
        /// </summary>
        /// <param name="items">集合</param>
        public void AddRange(IEnumerable<T> items)
        {
            this.AddRange(items, this._defaultPriority);
        }

        /// <summary>
        /// 添加集合到末尾
        /// </summary>
        /// <param name="items">集合</param>
        /// <param name="priorityLevel">优先级[值越小优先级越高,越大越低]</param>
        public void AddRange(IEnumerable<T> items, int priorityLevel)
        {
            int count = items.Count();
            if (items == null || count == 0)
            {
                return;
            }

            this.OnRaiseInvokeAction(() =>
            {
                lock (this._collectionChangeMonitor)
                {
                    int totalCount = base.Items.Count + count;
                    T defaultValue = default(T);
                    if (this._isRingStruct && totalCount > this.Capcity)
                    {
                        List<T> oldItems = null;
                        List<T> newItems = null;
                        if (count >= this.Capcity)
                        {
                            oldItems = new List<T>(this);
                            base.Clear();
                            newItems = items.Skip(count - this.Capcity).ToList();
                            foreach (var newItem in newItems)
                            {
                                base.Add(newItem);
                            }
                        }
                        else
                        {
                            oldItems = new List<T>();
                            newItems = new List<T>();
                            foreach (var item in items)
                            {
                                if (base.Items.Count < this.Capcity)
                                {
                                    base.Add(item);
                                }
                                else
                                {
                                    oldItems.Add(base[this._ringPosition]);
                                    //base[this._ringPosition] = item;
                                    base.SetItem(this._ringPosition, item);
                                    //更新环形结构的索引
                                    this.UpdateRingPosition(true);
                                }

                                newItems.Add(item);
                            }
                        }

                        this.OnBindingListChanged(BindingListChangedType.Update, defaultValue, defaultValue, newItems, oldItems);
                    }
                    else
                    {
                        foreach (var item in items)
                        {
                            base.Add(item);
                        }

                        this.OnBindingListChanged(BindingListChangedType.Add, defaultValue, defaultValue, new List<T>(items), null);
                    }
                }
            }, priorityLevel);
        }

        /// <summary>
        /// 隐藏重写Insert,插入指定的项
        /// </summary>
        /// <param name="index">从开始处的索引</param>
        /// <param name="item">插入的项</param>
        public virtual new void Insert(int index, T item)
        {
            this.Insert(index, item, this._defaultPriority);
        }

        /// <summary>
        /// 重载Insert,插入指定的项
        /// </summary>
        /// <param name="index">从开始处的索引</param>
        /// <param name="item">插入的项</param>
        /// <param name="priorityLevel">优先级[值越小优先级越高,越大越低]</param>
        public virtual void Insert(int index, T item, int priorityLevel)
        {
            if (this._isRingStruct)
            {
                throw new Exception("当列表为环形结构时不允许插入");
            }

            this.OnRaiseInvokeAction(() =>
            {
                lock (this._collectionChangeMonitor)
                {
                    base.Insert(index, item);
                    T oldItem = default(T);
                    this.OnBindingListChanged(BindingListChangedType.Insert, item, oldItem, null, null);
                }
            }, priorityLevel);
        }

        /// <summary>
        /// 隐藏重写Remove,移除一个匹配的项
        /// </summary>
        /// <param name="item">移除的项</param>
        public virtual new void Remove(T item)
        {
            this.Remove(item, this._defaultPriority);
        }

        /// <summary>
        /// 重载Remove,移除一个匹配的项
        /// </summary>
        /// <param name="item">移除的项</param>
        /// <param name="priorityLevel">优先级[值越小优先级越高,越大越低]</param>
        public virtual void Remove(T item, int priorityLevel)
        {
            this.OnRaiseInvokeAction(() =>
            {
                lock (this._collectionChangeMonitor)
                {
                    if (base.Contains(item))
                    {
                        base.Remove(item);
                        //更新环形结构的索引
                        this.UpdateRingPosition(false);
                        this.OnBindingListChanged(BindingListChangedType.Remove, default(T), item, null, null);
                    }
                }
            }, this._defaultPriority);
        }

        /// <summary>
        /// 隐藏重写Remove,移除指定位置的项
        /// </summary>
        /// <param name="index">从开始处的索引</param>
        public virtual new void RemoveAt(int index)
        {
            this.RemoveAt(index, this._defaultPriority);
        }

        /// <summary>
        /// 重载Remove,移除指定位置的项
        /// </summary>
        /// <param name="index">从开始处的索引</param>
        /// <param name="priorityLevel">优先级[值越小优先级越高,越大越低]</param>
        public virtual void RemoveAt(int index, int priorityLevel)
        {
            this.OnRaiseInvokeAction(() =>
            {
                lock (this._collectionChangeMonitor)
                {
                    T item = this.Items[index];
                    base.RemoveAt(index);
                    //更新环形结构的索引
                    this.UpdateRingPosition(false);
                    this.OnBindingListChanged(BindingListChangedType.Remove, default(T), item, null, null);
                }
            }, priorityLevel);
        }

        /// <summary>
        /// 隐藏重写RemoveItem,移除指定位置的项
        /// </summary>
        /// <param name="index">从开始处的索引</param>
        public virtual new void RemoveItem(int index)
        {
            this.RemoveItem(index, this._defaultPriority);
        }

        /// <summary>
        /// 重载RemoveItem,移除指定位置的项
        /// </summary>
        /// <param name="index">从开始处的索引</param>
        /// <param name="priorityLevel">优先级[值越小优先级越高,越大越低]</param>
        public virtual void RemoveItem(int index, int priorityLevel)
        {
            this.OnRaiseInvokeAction(() =>
            {
                lock (this._collectionChangeMonitor)
                {
                    T item = this.Items[index];
                    base.RemoveItem(index);
                    //更新环形结构的索引
                    this.UpdateRingPosition(false);
                    this.OnBindingListChanged(BindingListChangedType.Remove, default(T), item, null, null);
                }
            }, priorityLevel);
        }

        /// <summary>
        /// 从指定位置移除指定个数的项
        /// </summary>
        /// <param name="index">移除起始位置</param>
        /// <param name="count">移除项个数</param>
        public virtual void RemoveArea(int index, int count)
        {
            this.RemoveArea(index, count, this._defaultPriority);
        }

        /// <summary>
        /// 重载从指定位置移除指定个数的项
        /// </summary>
        /// <param name="index">移除起始位置</param>
        /// <param name="count">移除项个数</param>
        /// <param name="priorityLevel">优先级[值越小优先级越高,越大越低]</param>
        public virtual void RemoveArea(int index, int count, int priorityLevel)
        {
            this.OnRaiseInvokeAction(() =>
            {
                lock (this._collectionChangeMonitor)
                {
                    int endIndex = index + count;
                    if (this.Count < endIndex)
                    {
                        throw new Exception("集合超出索引");
                    }

                    var items = new List<T>();
                    //每次都移除此索引处的项,因为当前一项移除后,后一项的索引又变成这么多了
                    for (int i = index; i < endIndex; i++)
                    {
                        T item = base[i];
                        base.RemoveItem(index);
                        items.Add(item);
                    }

                    //更新环形结构的索引
                    this.UpdateRingPosition(false);
                    T defaultValue = default(T);
                    this.OnBindingListChanged(BindingListChangedType.RemoveRange, defaultValue, defaultValue, null, items);
                }
            }, priorityLevel);
        }

        /// <summary>
        /// 从指定位置移除指定个数的项
        /// </summary>
        /// <param name="items">要移除的集合</param>
        public virtual void RemoveArrange(IEnumerable<T> items)
        {
            this.RemoveArrange(items, this._defaultPriority);
        }

        /// <summary>
        /// 从指定位置移除指定个数的项
        /// </summary>
        /// <param name="items">要移除的集合</param>
        /// <param name="priorityLevel">优先级[值越小优先级越高,越大越低]</param>
        public virtual void RemoveArrange(IEnumerable<T> items, int priorityLevel)
        {
            if (items == null || items.Count() == 0)
            {
                return;
            }

            this.OnRaiseInvokeAction(() =>
            {
                lock (this._collectionChangeMonitor)
                {
                    var removeItems = new List<T>();
                    //移除项
                    foreach (var item in items)
                    {
                        if (base.Contains(item))
                        {
                            base.Remove(item);
                            removeItems.Add(item);
                        }
                    }

                    //更新环形结构的索引
                    this.UpdateRingPosition(false);
                    T defaultValue = default(T);
                    this.OnBindingListChanged(BindingListChangedType.RemoveRange, defaultValue, defaultValue, null, removeItems);
                }
            }, priorityLevel);
        }

        /// <summary>
        /// 隐藏重写Clear
        /// </summary>
        public virtual new void Clear()
        {
            this.Clear(this._defaultPriority);
        }

        /// <summary>
        /// 重载Clear
        /// </summary>
        /// <param name="priorityLevel">优先级[值越小优先级越高,越大越低]</param>
        public virtual void Clear(int priorityLevel)
        {
            this.OnRaiseInvokeAction(() =>
            {
                lock (this._collectionChangeMonitor)
                {
                    var oldItems = new List<T>(this);
                    base.Clear();
                    //更新环形结构的索引
                    this.UpdateRingPosition(false);
                    T defaultValue = default(T);
                    this.OnBindingListChanged(BindingListChangedType.RemoveRange, defaultValue, defaultValue, null, oldItems);
                }
            }, priorityLevel);
        }

        /// <summary>
        /// 重写排序
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="direction"></param>
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            lock (this._collectionChangeMonitor)
            {
                if (prop.PropertyType.GetInterface("IComparable") == null)
                {
                    //如果成员没有实现IComparable接口,则直接返回,不支持排序
                    throw new Exception(string.Format("成员:{0}没有实现IComparable接口,该字段列不支持排序", prop.Name));
                }

                int count = this.Count;
                ComparerResultType result = ComparerResultType.Equal;
                T tmpItem = default(T);
                object leftValue = null;
                object rightValue = null;
                int compareResult = 0;

                for (int i = 0; i < count; i++)
                {
                    for (int j = i + 1; j < count; j++)
                    {
                        leftValue = prop.GetValue(this.Items[i]);
                        rightValue = prop.GetValue(this.Items[j]);

                        if (leftValue == null && rightValue == null)
                        {
                            result = ComparerResultType.Equal;
                        }
                        else if (leftValue == null && rightValue != null)
                        {
                            result = ComparerResultType.Less;
                        }
                        else if (leftValue != null && rightValue == null)
                        {
                            result = ComparerResultType.Greater;
                        }
                        else
                        {
                            compareResult = ((IComparable)(leftValue)).CompareTo((IComparable)(rightValue));

                            if (compareResult == 1)
                            {
                                result = ComparerResultType.Greater;
                            }
                            else if (compareResult == 0)
                            {
                                result = ComparerResultType.Equal;
                            }
                            else if (compareResult == -1)
                            {
                                result = ComparerResultType.Less;
                            }
                            else
                            {
                                throw new Exception(string.Format("奇怪的比较结果:{0}", compareResult));
                            }
                        }

                        if (result == ComparerResultType.Equal)
                        {
                            continue;
                        }

                        if (direction == ListSortDirection.Ascending)
                        {
                            if (result == ComparerResultType.Greater)
                            {
                                tmpItem = this.Items[i];
                                this.Items[i] = this.Items[j];
                                this.Items[j] = tmpItem;
                            }
                        }
                        else
                        {
                            if (result == ComparerResultType.Less)
                            {
                                tmpItem = this.Items[i];
                                this.Items[i] = this.Items[j];
                                this.Items[j] = tmpItem;
                            }
                        }
                    }
                }
            }

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemMoved, prop));
        }

        /// <summary>
        /// 是否支持排序
        /// </summary>
        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        /// <summary>
        /// 重写索引
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>索引对应的数据项</returns>
        public new T this[int index]
        {
            get
            {
                lock (this._collectionChangeMonitor)
                {
                    return base[index];
                }
            }
            set
            {
                lock (this._collectionChangeMonitor)
                {
                    base[index] = value;
                }
            }
        }

        /// <summary>
        /// 认优先级[默认优先级为0,值越小优先级高]
        /// </summary>
        private int _defaultPriority = 0;

        /// <summary>
        /// 获取或设置默认优先级[默认优先级为0,值越小优先级高]
        /// </summary>
        public int DefaultPriority
        {
            get { return _defaultPriority; }
            set { _defaultPriority = value; }
        }

        /// <summary>
        /// 环形结构容量
        /// </summary>
        private volatile int _capcity = -1;

        /// <summary>
        /// 获取或设置环形结构容量
        /// </summary>
        public int Capcity
        {
            get { return this._capcity; }
            set
            {
                if (!this._isRingStruct)
                {
                    throw new ArgumentException("当前列表为蜚环形结构,不允许设置容量小于");
                }

                if (value < 1)
                {
                    throw new ArgumentException(string.Format("环形结构容量不能小于1,值{0}无效", value), "value");
                }

                if (this._capcity == value)
                {
                    return;
                }

                int oldCapcity = this._capcity;
                this._capcity = value;

                //this.OnRaiseInvokeAction(() =>
                //{
                lock (this._collectionChangeMonitor)
                {
                    if (oldCapcity > this._capcity)
                    {
                        List<T> removeItems = new List<T>();
                        int count = base.Items.Count;
                        if (count >= oldCapcity)//容量满
                        {
                            //先移除从最旧的项开始到结尾的项
                            for (int i = this._ringPosition; i < count && base.Items.Count > this._capcity && i < base.Items.Count; )
                            {
                                removeItems.Add(base[i]);
                                base.RemoveAt(i);
                            }

                            //如果移除最后的项后原始容量还大于新的容量,则从开始移除最旧的项
                            if (base.Items.Count > this._capcity)
                            {
                                for (int i = 0; i < base.Items.Count - this._capcity; )
                                {
                                    removeItems.Add(base[i]);
                                    base.RemoveAt(i);
                                }
                            }
                            else
                            {
                                //将后面旧的项移到最前面
                                for (int i = this._ringPosition; i < base.Items.Count - this._ringPosition; i++)
                                {
                                    T tmpItem = base[i];
                                    base.RemoveAt(i);
                                    base.Insert(0, tmpItem);
                                }
                            }
                        }
                        else if (base.Items.Count > this._capcity)
                        {
                            for (int i = 0; base.Items.Count > this._capcity; )
                            {
                                removeItems.Add(base[i]);
                                base.RemoveAt(i);
                            }
                        }

                        //重置环形结构在容量满之后更新数据的位置索引
                        this.ResetRingPosition();

                        T defaultValue = default(T);
                        this.OnBindingListChanged(BindingListChangedType.RemoveRange, defaultValue, defaultValue, null, removeItems);
                    }
                    else
                    {
                        //重置环形结构在容量满之后更新数据的位置索引
                        this.ResetRingPosition();
                    }
                }
                //}, 0);
            }
        }
    }

    /// <summary>
    /// UI绑定集合改变类型
    /// </summary>
    public enum BindingListChangedType
    {
        /// <summary>
        /// 添加项
        /// </summary>
        Add,

        /// <summary>
        /// 添加多项
        /// </summary>
        AddRange,

        /// <summary>
        /// 插入
        /// </summary>
        Insert,

        /// <summary>
        /// 更新项
        /// </summary>
        Update,

        /// <summary>
        /// 删除项
        /// </summary>
        Remove,

        /// <summary>
        /// 移除多项
        /// </summary>
        RemoveRange,

        /// <summary>
        /// 清空项
        /// </summary>
        Clear
    }

    /// <summary>
    /// 比较结果类型
    /// </summary>
    internal enum ComparerResultType
    {
        /// <summary>
        /// 等于
        /// </summary>
        Equal = 0,

        /// <summary>
        /// 大于
        /// </summary>
        Greater = 1,

        /// <summary>
        /// 小于
        /// </summary>
        Less = -1
    }

    /// <summary>
    /// UI绑定集合发生更改委托参数
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class BindingListChangedArgs<T> : EventArgs
    {
        /*
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">改变类型</param>
        /// <param name="oldItem">改变项</param>
        /// <param name="count">当前集合项组数</param>
        /// <param name="index">当前项在集合中的位置索引</param>
        public BindingListChangedArgs(BindingListChangedType type, T item, int count, int index)
            : this(type, item, default(T), null, count, index)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">改变类型</param>
        /// <param name="items">改变项集合</param>
        /// <param name="count">当前集合项组数</param>
        /// <param name="index">当前项在集合中的位置索引</param>
        public BindingListChangedArgs(BindingListChangedType type, List<T> items, int count, int index)
            : this(type, default(T), default(T), items, count, index)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">改变类型</param>
        /// <param name="item">改变项</param>
        /// <param name="count">当前集合项组数</param>
        /// <param name="index">当前项在集合中的位置索引</param>
        public BindingListChangedArgs(BindingListChangedType type, T oldItem, int count, int index)
            : this(type, default(T), oldItem, null, count, index)
        { }*/

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">改变类型</param>
        /// <param name="item">新项</param>
        /// <param name="oldItem">旧项</param>
        /// <param name="newItems">新项集合</param>
        /// <param name="oldtems">旧项集合</param>
        /// <param name="count">当前集合项组数</param>
        /// <param name="index">当前项在集合中的位置索引</param>
        public BindingListChangedArgs(BindingListChangedType type, T item, T oldItem, List<T> newItems, List<T> oldtems, int count, int index)
        {
            this.Type = type;
            this.NewItem = item;
            this.OldItem = oldItem;
            this.NewItems = newItems;
            this.Oldtems = oldtems;
            this.Count = count;
            this.Index = index;
        }

        /// <summary>
        /// UI绑定集合改变类型
        /// </summary>
        public BindingListChangedType Type { get; private set; }

        /// <summary>
        /// 新项
        /// </summary>
        public T NewItem { get; private set; }

        /// <summary>
        /// 旧项
        /// </summary>
        public T OldItem { get; private set; }

        /// <summary>
        /// 新项集合
        /// </summary>
        public List<T> NewItems { get; private set; }

        /// <summary>
        /// 旧项集合
        /// </summary>
        public List<T> Oldtems { get; private set; }

        /// <summary>
        /// 当前集合项组数
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// 当前项在集合中的位置索引
        /// </summary>
        public int Index { get; private set; }
    }
}
