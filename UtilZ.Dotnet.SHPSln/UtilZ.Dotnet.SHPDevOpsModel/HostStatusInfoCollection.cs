using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.LRPC;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;

namespace UtilZ.Dotnet.SHPDevOpsModel
{
    public class HostStatusInfoCollection : TimeoutBase, ICollection<HostStatusInfo>
    {
        private readonly HostInfo _hostInfo;
        private readonly List<HostStatusInfo> _hostStatusInfoList = new List<HostStatusInfo>();
        public readonly object SynRoot = new object();

        public HostInfo HostInfo
        {
            get { return _hostInfo; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hostInfo">HostInfo</param>
        public HostStatusInfoCollection(HostInfo hostInfo)
            : base(Config.Instance.HostTimeoutMillisecondsTimeout)
        {
            if (hostInfo == null)
            {
                throw new ArgumentNullException(nameof(hostInfo));
            }

            this._hostInfo = hostInfo;
        }

        public int Count
        {
            get
            {
                lock (this.SynRoot)
                {
                    return this._hostStatusInfoList.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(HostStatusInfo hostStatusInfo)
        {
            lock (this.SynRoot)
            {
                this._hostStatusInfoList.Add(hostStatusInfo);
                base.UpdateLastAccessTimestamp();
                while (this._hostStatusInfoList.Count > Config.Instance.HostStatusMaxCount)
                {
                    this._hostStatusInfoList.RemoveAt(0);
                }
            }
        }

        public bool Remove(HostStatusInfo item)
        {
            lock (this.SynRoot)
            {
                return this._hostStatusInfoList.Remove(item);
            }
        }

        public void Clear()
        {
            lock (this.SynRoot)
            {
                this._hostStatusInfoList.Clear();
            }
        }

        public bool Contains(HostStatusInfo item)
        {
            lock (this.SynRoot)
            {
                return this._hostStatusInfoList.Contains(item);
            }
        }

        public void CopyTo(HostStatusInfo[] array, int arrayIndex)
        {
            lock (this.SynRoot)
            {
                this._hostStatusInfoList.CopyTo(array, arrayIndex);
            }
        }

        public IEnumerator<HostStatusInfo> GetEnumerator()
        {
            return this._hostStatusInfoList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
