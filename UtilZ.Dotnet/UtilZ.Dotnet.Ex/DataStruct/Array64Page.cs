using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.DataStruct
{
    internal class Array64Page<T>
    {
        private readonly long _begin;
        private readonly long _end;
        private readonly int _colSize;
        private readonly int _rowSize;
        private readonly T[,] _data;
        private readonly T[] _mod;

        public Array64Page(long begin, long end, int colSize, int rowSize)
        {
            this._begin = begin;
            this._end = end;
            this._colSize = colSize;
            this._rowSize = rowSize;

            long length = end - begin;
            long rowCount = length / colSize;
            this._data = new T[rowCount, colSize];
            for (int i = 0; i < rowCount; i++)
            {
                this._data.SetValue(new T[colSize], i);
            }

            long mod = length % colSize;
            this._mod = new T[mod];
        }

        public Array64Page(Array64Page<T> page)
        {
            this._begin = page._begin;
            this._end = page._end;
            this._colSize = page._colSize;
            this._rowSize = page._rowSize;
            this._data = new T[page._data.GetLength(0), this._colSize];
            this._mod = page._mod.ToArray();
        }

        public T GetValueByPosition(long position)
        {
            long currentPostion = position - this._begin;
            int rowIndex = (int)(currentPostion / this._rowSize);
            int colIndex = (int)(currentPostion % this._rowSize);
            return (T)this._data.GetValue(rowIndex, colIndex);
        }

        public void SetValueByPosition(long position, T value)
        {
            long currentPostion = position - this._begin;
            int rowIndex = (int)(currentPostion / this._rowSize);
            int colIndex = (int)(currentPostion % this._rowSize);
            this._data.SetValue(value, rowIndex, colIndex);
        }

        internal int Get(T[] destBuffer, int offset, long beginIndex, int length)
        {
            int needReadLength = destBuffer.Length - offset;
            int readLength = 0;
            bool readModFlag = true;
            T[] buffer;
            for (int i = 0; i < this._data.GetLength(0); i++)
            {
                buffer = (T[])this._data.GetValue(i);
                if (needReadLength <= buffer.Length)
                {
                    Array.Copy(buffer, 0, destBuffer, offset, needReadLength);
                    readLength += needReadLength;
                    readModFlag = false;
                    break;
                }
                else
                {
                    Array.Copy(buffer, 0, destBuffer, offset, buffer.Length);
                    needReadLength = needReadLength - buffer.Length;
                    readLength += buffer.Length;
                }
            }

            if (readModFlag && this._mod.Length > 0)
            {
                if (needReadLength > this._mod.Length)
                {
                    needReadLength = this._mod.Length;
                }

                Array.Copy(this._mod, 0, destBuffer, offset, needReadLength);
                readLength += needReadLength;
            }

            return readLength;
        }

        internal int Set<T>(T[] buffer, int offset, long beginIndex, long length)
        {
            int writeLength = 0;
            int needWriteLength = buffer.Length - offset;
            bool writeModFlag = true;
            T[] value;
            for (int i = 0; i < this._data.GetLength(0); i++)
            {
                value = (T[])this._data.GetValue(i);
                if (needWriteLength > value.Length)
                {
                    Array.Copy(buffer, offset, value, 0, value.Length);
                    needWriteLength -= value.Length;
                    writeLength += value.Length;
                    offset += value.Length;
                }
                else
                {
                    Array.Copy(buffer, offset, value, 0, needWriteLength);
                    writeLength += needWriteLength;
                    writeModFlag = false;
                    break;
                }
            }

            if (writeModFlag && this._mod.Length > 0)
            {
                if (needWriteLength > this._mod.Length)
                {
                    needWriteLength = this._mod.Length;
                }

                Array.Copy(buffer, offset, this._mod, 0, needWriteLength);
                writeLength += needWriteLength;
            }

            return writeLength;
        }

        public Array64Page<T> ToPage()
        {
            return new Array64Page<T>(this);
        }
    }
}
