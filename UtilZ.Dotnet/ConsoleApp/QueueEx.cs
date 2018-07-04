using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ConsoleApp
{
    [Serializable, ComVisible(false)]
    public class QueueEx<T> : IEnumerable<T>, IEnumerable, ICollection
    {
        // Fields
        private T[] _array;
        private const int _DefaultCapacity = 4;
        private static T[] _emptyArray;
        private const int _GrowFactor = 200;
        private int _head;
        private const int _MinimumGrow = 4;
        private const int _ShrinkThreshold = 0x20;
        private int _size;
        [NonSerialized]
        private object _syncRoot;
        private int _tail;
        private int _version;

        // Methods
        static QueueEx()
        {
            QueueEx<T>._emptyArray = new T[0];
        }

        public QueueEx()
        {
            this._array = _emptyArray;
        }

        public QueueEx(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            this._array = new T[4];
            this._size = 0;
            this._version = 0;
            using (IEnumerator<T> enumerator = collection.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    this.Enqueue(enumerator.Current);
                }
            }
        }

        public QueueEx(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            this._array = new T[capacity];
            this._head = 0;
            this._tail = 0;
            this._size = 0;
        }

        public void Clear()
        {
            if (this._head < this._tail)
            {
                Array.Clear(this._array, this._head, this._size);
            }
            else
            {
                Array.Clear(this._array, this._head, this._array.Length - this._head);
                Array.Clear(this._array, 0, this._tail);
            }

            this._head = 0;
            this._tail = 0;
            this._size = 0;
            this._version++;
        }

        public bool Contains(T item)
        {
            int index = this._head;
            int num2 = this._size;
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            while (num2-- > 0)
            {
                if (item == null)
                {
                    if (this._array[index] == null)
                    {
                        return true;
                    }
                }
                else if ((this._array[index] != null) && comparer.Equals(this._array[index], item))
                {
                    return true;
                }

                index = (index + 1) % this._array.Length;
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if ((arrayIndex < 0) || (arrayIndex > array.Length))
            {
                throw new ArgumentOutOfRangeException();
            }

            int length = array.Length;
            if ((length - arrayIndex) < this._size)
            {
                throw new ArgumentException();
            }

            int num2 = ((length - arrayIndex) < this._size) ? (length - arrayIndex) : this._size;
            if (num2 != 0)
            {
                int num3 = ((this._array.Length - this._head) < num2) ? (this._array.Length - this._head) : num2;
                Array.Copy(this._array, this._head, array, arrayIndex, num3);
                num2 -= num3;
                if (num2 > 0)
                {
                    Array.Copy(this._array, 0, array, (arrayIndex + this._array.Length) - this._head, num2);
                }
            }
        }

        public T Dequeue()
        {
            if (this._size == 0)
            {
                throw new InvalidOperationException();
            }

            T local = this._array[this._head];
            this._array[this._head] = default(T);
            this._head = (this._head + 1) % this._array.Length;
            this._size--;
            this._version++;
            return local;
        }

        public void Enqueue(T item)
        {
            if (this._size == this._array.Length)
            {
                int capacity = (int)((this._array.Length * 200L) / 100L);
                if (capacity < (this._array.Length + 4))
                {
                    capacity = this._array.Length + 4;
                }

                this.SetCapacity(capacity);
            }

            this._array[this._tail] = item;
            this._tail = (this._tail + 1) % this._array.Length;
            this._size++;
            this._version++;
        }

        internal T GetElement(int i)
        {
            return this._array[(this._head + i) % this._array.Length];
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator((QueueEx<T>)this);
        }

        public T Peek()
        {
            if (this._size == 0)
            {
                throw new InvalidOperationException();
            }

            return this._array[this._head];
        }

        private void SetCapacity(int capacity)
        {
            T[] destinationArray = new T[capacity];
            if (this._size > 0)
            {
                if (this._head < this._tail)
                {
                    Array.Copy(this._array, this._head, destinationArray, 0, this._size);
                }
                else
                {
                    Array.Copy(this._array, this._head, destinationArray, 0, this._array.Length - this._head);
                    Array.Copy(this._array, 0, destinationArray, this._array.Length - this._head, this._tail);
                }
            }

            this._array = destinationArray;
            this._head = 0;
            this._tail = (this._size == capacity) ? 0 : this._size;
            this._version++;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new Enumerator((QueueEx<T>)this);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentException();
            }

            if (array.Rank != 1)
            {
                throw new ArgumentException();
            }

            if (array.GetLowerBound(0) != 0)
            {
                throw new ArgumentException();
            }

            int length = array.Length;
            if ((index < 0) || (index > length))
            {
                throw new ArgumentOutOfRangeException();
            }

            if ((length - index) < this._size)
            {
                throw new ArgumentException();
            }

            int num2 = ((length - index) < this._size) ? (length - index) : this._size;
            if (num2 != 0)
            {
                try
                {
                    int num3 = ((this._array.Length - this._head) < num2) ? (this._array.Length - this._head) : num2;
                    Array.Copy(this._array, this._head, array, index, num3);
                    num2 -= num3;
                    if (num2 > 0)
                    {
                        Array.Copy(this._array, 0, array, (index + this._array.Length) - this._head, num2);
                    }
                }
                catch (ArrayTypeMismatchException)
                {
                    throw new ArgumentException();
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator((QueueEx<T>)this);
        }

        public T[] ToArray()
        {
            T[] destinationArray = new T[this._size];
            if (this._size != 0)
            {
                if (this._head < this._tail)
                {
                    Array.Copy(this._array, this._head, destinationArray, 0, this._size);
                    return destinationArray;
                }
                Array.Copy(this._array, this._head, destinationArray, 0, this._array.Length - this._head);
                Array.Copy(this._array, 0, destinationArray, this._array.Length - this._head, this._tail);
            }
            return destinationArray;
        }

        public void TrimExcess()
        {
            int num = (int)(this._array.Length * 0.9);
            if (this._size < num)
            {
                this.SetCapacity(this._size);
            }
        }

        // Properties
        public int Count =>
            this._size;

        bool ICollection.IsSynchronized =>
            false;

        object ICollection.SyncRoot
        {
            get
            {
                if (this._syncRoot == null)
                {
                    Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
                }
                return this._syncRoot;
            }
        }

        // Nested Types
        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
        {
            private QueueEx<T> _q;
            private int _index;
            private int _version;
            private T _currentElement;
            internal Enumerator(QueueEx<T> q)
            {
                this._q = q;
                this._version = this._q._version;
                this._index = -1;
                this._currentElement = default(T);
            }

            public void Dispose()
            {
                this._index = -2;
                this._currentElement = default(T);
            }

            public bool MoveNext()
            {
                if (this._version != this._q._version)
                {
                    throw new InvalidOperationException();
                }

                if (this._index == -2)
                {
                    return false;
                }

                this._index++;
                if (this._index == this._q._size)
                {
                    this._index = -2;
                    this._currentElement = default(T);
                    return false;
                }

                this._currentElement = this._q.GetElement(this._index);
                return true;
            }

            public T Current
            {
                get
                {
                    if (this._index < 0)
                    {
                        if (this._index == -1)
                        {
                            throw new InvalidOperationException();
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }

                    return this._currentElement;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    if (this._index < 0)
                    {
                        if (this._index == -1)
                        {
                            throw new InvalidOperationException();
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }
                    return this._currentElement;
                }
            }

            void IEnumerator.Reset()
            {
                if (this._version != this._q._version)
                {
                    throw new InvalidOperationException();
                }

                this._index = -1;
                this._currentElement = default(T);
            }
        }
    }
}
