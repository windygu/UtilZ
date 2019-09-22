using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.Ex.Model;

namespace UtilZ.Dotnet.DBIBase.Model
{
    /// <summary>
    /// 数据库发展信息
    /// </summary>
    public class DatabasePropertyInfo
    {
        /// <summary>
        /// 获取内存占用大小，单位/字节
        /// </summary>
        public long MemorySize { get; protected set; }

        /// <summary>
        /// 获取磁盘空间占用大小，单位/字节
        /// </summary>
        public long DiskSize { get; protected set; }

        /// <summary>
        /// 获取最大连接数
        /// </summary>
        public int MaxConnectCount { get; protected set; }

        /// <summary>
        /// 获取连接数
        /// </summary>
        public int ConnectCount { get; protected set; }

        /// <summary>
        /// 获取并发连接数
        /// </summary>
        public int ConcurrentConnectCount { get; protected set; }

        /// <summary>
        /// 获取数据库启动时间
        /// </summary>
        public DateTime StartTime { get; protected set; }

        /// <summary>
        /// 获取数据库创建时间
        /// </summary>
        public DateTime CreatetTime { get; protected set; }


        /// <summary>
        /// 构造函数
        /// </summary>
        public DatabasePropertyInfo(long memorySize, long diskSize, int maxConnectCount,
            int connectCount, int concurrentConnectCount, DateTime startTime, DateTime createtTime)
        {
            this.MemorySize = memorySize;
            this.DiskSize = diskSize;
            this.MaxConnectCount = maxConnectCount;
            this.ConnectCount = connectCount;
            this.ConcurrentConnectCount = concurrentConnectCount;
            this.StartTime = startTime;
            this.CreatetTime = createtTime;
        }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return $@"MemorySize;{this.MemorySize};DiskSize:{this.DiskSize};MaxConnectCount:{this.MaxConnectCount};ConnectCount:{this.ConnectCount};ConcurrentConnectCount:{this.ConcurrentConnectCount};StartTime:{this.StartTime.ToString(UtilConstant.DateTimeFormat)};CreatetTime:{this.CreatetTime.ToString(UtilConstant.DateTimeFormat)}";
        }
    }
}
