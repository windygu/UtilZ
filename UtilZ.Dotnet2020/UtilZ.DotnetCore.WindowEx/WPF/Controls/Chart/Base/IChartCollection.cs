using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls.Chart
{
    public interface IChartCollection<T> : IList<T>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        event EventHandler<ChartCollectionChangedEventArgs<T>> ChartCollectionChanged;

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="items">The items.</param>
        void AddRange(IEnumerable<T> items);

        /// <summary>
        /// Inserts the range.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="collection">The collection.</param>
        void InsertRange(int index, IEnumerable<T> collection);
    }

    public class ChartCollectionChangedEventArgs<T> : EventArgs
    {
        public ChartCollectionChangedAction Action { get; private set; }

        public List<T> NewItems { get; private set; }

        public List<T> OldItems { get; private set; }

        public ChartCollectionChangedEventArgs(ChartCollectionChangedAction action, List<T> newItems, List<T> oldItems)
        {
            this.Action = action;
            this.NewItems = newItems;
            this.OldItems = oldItems;
        }

        public ChartCollectionChangedEventArgs(ChartCollectionChangedAction action, T newItem, T oldItem)
        {
            this.Action = action;
            if (newItem != null)
            {
                this.NewItems = new List<T>() { newItem };
            }

            if (oldItem != null)
            {
                this.OldItems = new List<T>() { oldItem };
            }
        }
    }


    public enum ChartCollectionChangedAction
    {
        Add = 0,
        Remove = 1,
        Replace = 2,
        Move = 3,
    }
}
