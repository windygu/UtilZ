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
        private readonly T[][] _data;

        public Array64Page(long begin, long end, int colSize, int rowSize)
        {
            this._begin = begin;
            this._end = end;
            this._colSize = colSize;
            this._rowSize = rowSize;

            long length = end - begin;
            long rowCount = length / colSize;
            long mod = length % colSize;
            if (mod > 0)
            {
                rowCount += 1;
            }

            this._data = new T[rowCount][];
            long lastIndex = rowCount - 1;
            for (int i = 0; i < rowCount; i++)
            {
                if (i == lastIndex && mod > 0)
                {
                    this._data[i] = new T[mod];
                }
                else
                {
                    this._data[i] = new T[colSize];
                }
            }
        }

        public Array64Page(Array64Page<T> page)
        {
            this._begin = page._begin;
            this._end = page._end;
            this._colSize = page._colSize;
            this._rowSize = page._rowSize;
            int rowCount = page._data.GetLength(0);
            this._data = new T[rowCount][];
            for (int i = 0; i < rowCount; i++)
            {
                this._data[i] = new T[page._data[i].Length];
            }
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
            T[] buffer;
            for (int i = 0; i < this._data.GetLength(0); i++)
            {
                buffer = this._data[i];
                if (needReadLength <= buffer.Length)
                {
                    Array.Copy(buffer, 0, destBuffer, offset, needReadLength);
                    readLength += needReadLength;
                    break;
                }
                else
                {
                    Array.Copy(buffer, 0, destBuffer, offset, buffer.Length);
                    needReadLength = needReadLength - buffer.Length;
                    readLength += buffer.Length;
                    offset += buffer.Length;
                }
            }

            return readLength;
        }

        internal int Set<T>(T[] buffer, int offset, long beginIndex, long length)
        {
            long currentNeedOffset = beginIndex - this._begin;
            long currentTotalOffset = 0;
            int rowCount = this._data.GetLength(0);
            int rowLength;
            for (int i = 0; i < rowCount; i++)
            {
                rowLength = this._data[i].Length;
                if (currentTotalOffset + rowLength >= currentNeedOffset)//如果当前页总共偏移量已经大于等于当前页需要偏移的量,则找到起始行
                {
                    int needWriteLength = buffer.Length - offset;//还需要写的数据长度
                    int currentRowWriteBeginPosition = (int)(currentNeedOffset - currentTotalOffset);//起始行写数据起始位置
                    var row = this._data[i];//起始行数据
                    int currentRowWriteLength = rowLength - currentRowWriteBeginPosition;//当前行可写长度
                    if (needWriteLength <= currentRowWriteLength)//如果需要写的数据长度小于等于起始行可写数据长度,则只写起始行就可以了
                    {
                        Array.Copy(buffer, offset, row, currentRowWriteBeginPosition, needWriteLength);
                        return needWriteLength;
                    }
                    else
                    {
                        int writeLength = 0;//总代写的数据长度
                        Array.Copy(buffer, offset, row, currentRowWriteBeginPosition, currentRowWriteLength);
                        writeLength += currentRowWriteLength;
                        offset += currentRowWriteLength;

                        for (int j = i + 1; j < rowCount; j++)
                        {
                            row = this._data[j];
                            rowLength = row.Length;
                            needWriteLength = buffer.Length - offset;//还需要写的数据长度
                            if (needWriteLength <= rowLength)
                            {
                                Array.Copy(buffer, offset, row, 0, needWriteLength);
                                writeLength += needWriteLength;
                                return writeLength;
                            }
                            else
                            {
                                Array.Copy(buffer, offset, row, 0, rowLength);
                                writeLength += rowLength;
                                offset += rowLength;
                            }
                        }

                        return writeLength;
                    }
                }
                else
                {
                    currentTotalOffset += rowLength;
                }
            }

            return 0;
        }

        public Array64Page<T> ToPage()
        {
            return new Array64Page<T>(this);
        }
    }
}
