using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Model
{
    [Serializable]
    public class TransferBasicPolicy
    {
        /// <summary>
        /// 数据路由编码
        /// </summary>
        public int DataCode { get; set; }

        /// <summary>
        /// 目地地一致性key
        /// </summary>
        public string DestinationConsistentKey { get; set; }

        /// <summary>
        /// 目地地一致性数据个数
        /// </summary>
        public int DestinationConsistentCount { get; set; }

        /// <summary>
        /// 一致性数据发送超时时长
        /// </summary>
        public int ConsistentMillisecondsTimeout { get; set; }

        ///发送数据优先级[值越小,优先级越高]
        public short Priority { get; set; } = 0;

        private int _millisecondsTimeout = 5000;
        /// <summary>
        /// 单次发送超时时长,单位毫秒[此值不应过大]
        /// </summary>
        public int MillisecondsTimeout
        {
            get { return _millisecondsTimeout; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _millisecondsTimeout = value;
                this.CalTotalMillisecondsTimeout();
            }
        }

        private int _totalMillisecondsTimeout;
        /// <summary>
        /// 总发送超时时长,单位毫秒
        /// </summary>
        public int TotalMillisecondsTimeout
        {
            get { return _totalMillisecondsTimeout; }
        }

        private int _repeatCount = 3;
        /// <summary>
        /// 重试次数,小于2不重试
        /// </summary>
        public int RepeatCount
        {
            get { return _repeatCount; }
            set
            {
                _repeatCount = value;
                this.CalTotalMillisecondsTimeout();
            }
        }

        private void CalTotalMillisecondsTimeout()
        {
            if (this._repeatCount < 2)
            {
                this._totalMillisecondsTimeout = this._millisecondsTimeout;
            }
            else
            {
                this._totalMillisecondsTimeout = this._millisecondsTimeout * this._repeatCount;
            }
        }

        public TransferBasicPolicy()
        {
            this.CalTotalMillisecondsTimeout();
        }

        public TransferBasicPolicy(int dataCode) :
            this()
        {
            this.DataCode = dataCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataCode"></param>
        /// <param name="destinationConsistentKey">目地地一致性key</param>
        /// <param name="destinationConsistentCount">目地地一致性数据个数</param>
        /// <param name="consistentMillisecondsTimeout">一致性数据发送超时时长</param>
        public TransferBasicPolicy(int dataCode, string destinationConsistentKey, int destinationConsistentCount, int consistentMillisecondsTimeout)
            : this()
        {
            this.DataCode = dataCode;
            this.DestinationConsistentKey = destinationConsistentKey;
            this.DestinationConsistentCount = destinationConsistentCount;
            this.ConsistentMillisecondsTimeout = consistentMillisecondsTimeout;
        }
    }
}
