using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Model;
using UtilZ.Dotnet.SHPBase.Base;

namespace UtilZ.Dotnet.SHPBase.Model
{
    [Serializable]
    public class HostProcessInfoItem : SHPBaseModel
    {
        /// <summary>
        /// 进程Id
        /// </summary>
        [DisplayName("进程Id")]
        public int Id { get; set; }

        [DisplayName("进程名称")]
        public string Name { get; set; }

        
        private float _cpu = 0f;
        /// <summary>
        /// Cpu使用率
        /// </summary>
        [DisplayName("CPU")]
        public float Cpu
        {
            get { return _cpu; }
            set
            {
                _cpu = value;
                base.OnRaisePropertyChanged(nameof(Cpu));
            }
        }

        private float _memory = 0;
        [DisplayName("内存大小/MB")]
        public float Memory
        {
            get { return _memory; }
            set
            {
                _memory = value;
                base.OnRaisePropertyChanged(nameof(Memory));
            }
        }

        private int _threadCount = 0;
        [DisplayName("线程数")]
        public int ThreadCount
        {
            get { return _threadCount; }
            set
            {
                _threadCount = value;
                base.OnRaisePropertyChanged(nameof(ThreadCount));
            }
        }

        private int _handleCount = 0;
        [DisplayName("句柄数")]
        public int HandleCount
        {
            get { return _handleCount; }
            set
            {
                _handleCount = value;
                base.OnRaisePropertyChanged(nameof(HandleCount));
            }
        }

        public HostProcessInfoItem()
        {

        }

        public void Update(HostProcessInfoItem value)
        {
            this.Cpu = value._cpu;
            this.Memory = value._memory;
            this.ThreadCount = value._threadCount;
            this.HandleCount = value._handleCount;
        }
    }
}

