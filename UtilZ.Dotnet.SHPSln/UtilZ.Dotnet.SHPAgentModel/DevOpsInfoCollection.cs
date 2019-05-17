using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SHPBase.Commands.Host;

namespace UtilZ.Dotnet.SHPAgentModel
{
    public class DevOpsInfoCollection : ICollection<DevOpsInfo>
    {
        private readonly List<DevOpsInfo> _devOpsInfoList = new List<DevOpsInfo>();
        private readonly object _devOpsInfoListLock = new object();

        public DevOpsInfoCollection()
        {

        }

        public int Count
        {
            get
            {
                lock (this._devOpsInfoListLock)
                {
                    return this._devOpsInfoList.Count;
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

        public void Add(DevOpsInfo item)
        {
            lock (this._devOpsInfoListLock)
            {
                this._devOpsInfoList.Add(item);
            }
        }

        public void AddRange(DevOpsInfo[] items)
        {
            lock (this._devOpsInfoListLock)
            {
                foreach (var item in items)
                {
                    this._devOpsInfoList.Add(item);
                }
            }
        }

        public bool Remove(DevOpsInfo item)
        {
            lock (this._devOpsInfoListLock)
            {
                return this._devOpsInfoList.Remove(item);
            }
        }

        public List<DevOpsInfo> RemoveByHostId(long hostId)
        {
            var deleDevOpsList = new List<DevOpsInfo>();
            lock (this._devOpsInfoListLock)
            {
                foreach (var devOpsInfo in this._devOpsInfoList.ToArray())
                {
                    if (devOpsInfo.HostId == hostId)
                    {
                        this._devOpsInfoList.Remove(devOpsInfo);
                        deleDevOpsList.Add(devOpsInfo);
                    }
                }
            }

            return deleDevOpsList;
        }

        public void Clear()
        {
            lock (this._devOpsInfoListLock)
            {
                this._devOpsInfoList.Clear();
            }
        }

        public bool Contains(DevOpsInfo item)
        {
            lock (this._devOpsInfoListLock)
            {
                return this._devOpsInfoList.Contains(item);
            }
        }

        public void CopyTo(DevOpsInfo[] array, int arrayIndex)
        {
            lock (this._devOpsInfoListLock)
            {
                this._devOpsInfoList.CopyTo(array, arrayIndex);
            }
        }

        public IEnumerator<DevOpsInfo> GetEnumerator()
        {
            lock (this._devOpsInfoListLock)
            {
                return this._devOpsInfoList.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Update(EvnChangedNotifyCommand cmd)
        {
            lock (this._devOpsInfoListLock)
            {
                var devOpsInfo = this._devOpsInfoList.Where(t => { return t.Id == cmd.DevOpsId; }).FirstOrDefault();
                if (devOpsInfo != null)
                {
                    devOpsInfo.StatusUploadIntervalMilliseconds = cmd.StatusUploadIntervalMilliseconds;
                }
            }
        }

        public int GetStatusUploadIntervalMilliseconds()
        {
            int statusUploadIntervalMilliseconds;
            lock (this._devOpsInfoListLock)
            {
                if (this._devOpsInfoList.Count == 0)
                {
                    statusUploadIntervalMilliseconds = 30000;
                }
                else
                {
                    statusUploadIntervalMilliseconds = (int)this._devOpsInfoList.Min(t => { return t.StatusUploadIntervalMilliseconds; });
                    if (statusUploadIntervalMilliseconds < 1)
                    {
                        statusUploadIntervalMilliseconds = 10000;
                    }
                }
            }

            return statusUploadIntervalMilliseconds;
        }
    }
}
