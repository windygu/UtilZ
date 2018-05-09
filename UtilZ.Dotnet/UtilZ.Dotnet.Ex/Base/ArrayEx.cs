using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.Base
{
    /// <summary>
    /// Array扩展类
    /// </summary>
    public class ArrayEx
    {
        /// <summary>
        /// 报告指定的 System.Byte[] 在此实例中的第一个匹配项的索引。
        /// </summary>
        /// <param name="srcBytes">被运行查找的 System.Byte[]。</param>
        /// <param name="searchBytes">要查找的 System.Byte[]。</param>
        /// <returns>假设找到该字节数组。则为 searchBytes 的索引位置。假设未找到该字节数组。则为 -1。假设 searchBytes 为 null 或者长度为0。则返回值为 -1。</returns>
        public static int IndexOf(IEnumerable<byte> srcBytes, IEnumerable<byte> searchBytes)
        {
            if (srcBytes == null)
            {
                return -1;
            }

            if (searchBytes == null)
            {
                return -1;
            }

            int srcBytesCount = srcBytes.Count();
            if (srcBytesCount == 0)
            {
                return -1;
            }

            int searchBytesCount = searchBytes.Count();
            if (searchBytesCount == 0)
            {
                return -1;
            }

            if (srcBytesCount < searchBytesCount)
            {
                return -1;
            }

            bool flag;
            int count = srcBytesCount - searchBytesCount + 1;
            for (int i = 0; i < count; i++)
            {
                if (srcBytes.ElementAt(i) == searchBytes.ElementAt(0))
                {
                    if (searchBytesCount == 1)
                    {
                        return i;
                    }

                    flag = true;
                    for (int j = 1; j < searchBytesCount; j++)
                    {
                        if (srcBytes.ElementAt(i + j) != searchBytes.ElementAt(j))
                        {
                            flag = false;
                            break;
                        }
                    }

                    if (flag)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
    }
}
