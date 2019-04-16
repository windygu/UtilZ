using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.Base
{
    public class BindingListChangedArgs<T> : EventArgs
    {
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

        public BindingListChangedType Type { get; private set; }

        public T NewItem { get; private set; }

        public T OldItem { get; private set; }

        public List<T> NewItems { get; private set; }

        public List<T> Oldtems { get; private set; }

        public int Count { get; private set; }

        public int Index { get; private set; }
    }
}
