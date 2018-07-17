using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.DataStruct
{
    /// <summary>
    /// 长度为Int64的数组类
    /// </summary>
    [Serializable]
    public class Array64<T>
    {
        /************************************************************************************
         * page0             page1             page3
         * (p0,r0)+++++++    (p1,r0)+++++++    (p3,r0)+++++++    
         * (p0,r1)+++++++    (p1,r1)+++++++    (p3,r1)+++++++
         * (p0,r2)+++++++    (p1,r2)+++++++    (p3,r2)+++++
         * (p0,r3)+++++++    (p1,r3)+++++++    
         ************************************************************************************/

        private readonly long _length;
        /// <summary>
        /// 获取数组长度
        /// </summary>
        public long Length
        {
            get { return this._length; }
        }

        private readonly long _pageSize;
        private readonly int _colSize;
        private readonly int _rowSize;

        /// <summary>
        /// 页集合
        /// </summary>
        private readonly Array64Page<T>[] _pages;

        /// <summary>
        /// 获取或设置指定索引处的值
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>指定索引处的值</returns>
        public T this[long index]
        {
            get
            {
                return this.GetPageByPosition(index).GetValueByPosition(index);
            }
            set
            {
                this.GetPageByPosition(index).SetValueByPosition(index, value);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="length">数组长度</param>
        /// <param name="pageSize">存储页大小</param>
        /// <param name="colSize">存储页中的列大小</param>
        /// <param name="rowSize">存储页中的行大小</param>
        public Array64(long length, long pageSize = (long)int.MaxValue * int.MaxValue, int colSize = int.MaxValue, int rowSize = int.MaxValue)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            if (colSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(colSize));
            }

            if (rowSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(rowSize));
            }

            long pageCount = length / pageSize;
            if (pageCount > int.MaxValue)
            {
                throw new ArgumentException("数据长度过长,页大小太小");
            }

            this._length = length;
            this._pageSize = pageSize;
            this._colSize = colSize;
            this._rowSize = rowSize;

            int lastPageIndex = (int)pageCount;
            int mod = (int)(length % pageSize);
            if (mod > 0)
            {
                pageCount += 1;
            }

            this._pages = new Array64Page<T>[pageCount];
            long begin = 0, end = 0;

            for (int i = 0; i < lastPageIndex; i++)
            {
                end = (i + 1) * pageSize;
                this._pages[i] = new Array64Page<T>(begin, end, colSize, rowSize);
                begin = end;
            }

            if (mod > 0)
            {
                this._pages[lastPageIndex] = new Array64Page<T>(end, length, colSize, rowSize);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="array">已知数组</param>
        public Array64(Array64<T> array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            this._length = array._length;
            this._pageSize = array._pageSize;
            this._colSize = array._colSize;
            this._rowSize = array._rowSize;

            this._pages = new Array64Page<T>[array._pages.Length];
            for (int i = 0; i < array._pages.GetLength(0); i++)
            {
                this._pages.SetValue(((Array64Page<T>)array._pages.GetValue(i)).ToPage(), i);
            }
        }

        private Array64Page<T> GetPageByPosition(long position)
        {
            if (position < 0 || position > this._length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            int pageIndex = (int)(position / this._pageSize);
            return this._pages[pageIndex];
        }

        public void Set(long beginIndex, T[] buffer, int length)
        {
            if (beginIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(beginIndex));
            }

            if (beginIndex == this._length ||
                buffer == null || buffer.Length == 0 ||
                length < 1)
            {
                return;
            }

            if (length > buffer.Length)
            {
                length = buffer.Length;
            }

            if (beginIndex + length > this._length)
            {
                length = (int)(this._length - beginIndex);
            }

            int pageIndex = (int)(beginIndex / this._pageSize);
            Array64Page<T> page;
            int bufferOffset = 0, currentSetLength;
            long setBeginIndex = beginIndex;
            for (int i = pageIndex; i < this._pages.Length; i++)
            {
                page = this._pages[i];
                currentSetLength = page.Set(buffer, bufferOffset, setBeginIndex, length);
                length -= currentSetLength;
                bufferOffset += currentSetLength;
                setBeginIndex += currentSetLength;
                if (length <= 0)
                {
                    break;
                }
            }
        }

        public T[] Get(long offset, int length)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if (length == 0 || offset >= this._length)
            {
                return new T[0];
            }

            if (offset + length > this._length)
            {
                length = (int)(this._length - offset);
            }

            T[] buffer = new T[length];
            int bufferOffset = 0, currentGetLength;
            long getIndex = offset;
            int pageIndex = (int)(offset / this._pageSize);
            for (int i = pageIndex; i < this._pages.Length; i++)
            {
                Array64Page<T> page = this._pages[pageIndex];
                currentGetLength = page.Get(buffer, bufferOffset, getIndex, length);
                length -= currentGetLength;
                offset += currentGetLength;
                bufferOffset += currentGetLength;
            }

            return buffer;
        }

        public Array64<T> ToArray()
        {
            return new Array64<T>(this);
        }
    }
}
