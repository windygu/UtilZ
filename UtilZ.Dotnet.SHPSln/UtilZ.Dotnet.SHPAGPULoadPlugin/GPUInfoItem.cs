using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPAGPULoadPlugin
{
    public class GPUInfoItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnRaisePropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// GPU索引
        /// </summary>
        [DisplayName("GPU索引")]
        public uint DeviceIndex { get; set; }

        private bool _status = false;
        /// <summary>
        /// 状态[true:正常;false:异常]
        /// </summary>
        [Browsable(false)]
        public bool Status
        {
            get { return _status; }
            set
            {
                if (_status == value)
                {
                    return;
                }

                _status = value;
                this.OnRaisePropertyChanged(nameof(StatusText));
            }
        }

        [DisplayName("状态")]
        public string StatusText
        {
            get { return _status ? "正常" : "异常"; }
        }

        private uint _use = 0;
        /// <summary>
        /// GPU使用率
        /// </summary>
        [DisplayName("GPU使用率")]
        public uint Use
        {
            get { return _use; }
            set
            {
                if (_use == value)
                {
                    return;
                }

                _use = value;
                this.OnRaisePropertyChanged(nameof(Use));
            }
        }

        /// <summary>
        /// 总内存
        /// </summary>
        [DisplayName("总内存")]
        public ulong TotalMem { get; set; }

        private ulong _freeMem = 0;
        /// <summary>
        /// 可用内存
        /// </summary>
        [Browsable(false)]
        public ulong FreeMem
        {
            get { return _freeMem; }
            set
            {
                if (_freeMem == value)
                {
                    return;
                }

                _freeMem = value;
                this.OnRaisePropertyChanged(nameof(FreeMemText));
            }
        }

        [DisplayName("可用内存")]
        public string FreeMemText
        {
            get
            {
                return this.MemText(this._freeMem);
            }
        }

        private ulong _usedMem = 0;
        /// <summary>
        /// 已用内存
        /// </summary>
        [Browsable(false)]
        public ulong UsedMem
        {
            get { return _usedMem; }
            set
            {
                if (_usedMem == value)
                {
                    return;
                }

                _usedMem = value;
                this.OnRaisePropertyChanged(nameof(UsedMemText));
            }
        }

        [DisplayName("已用内存")]
        public string UsedMemText
        {
            get
            {
                return this.MemText(this._usedMem);
            }
        }

        private string MemText(ulong mem)
        {
            return $"{mem / 1024 / 1024}MB";
        }


        public void Update(GPUInfoItem newItem)
        {
            this.Status = newItem._status;
            this.Use = newItem._use;
            this.FreeMem = newItem._freeMem;
            this.UsedMem = newItem._usedMem;
        }

        public GPUInfoItem()
        {

        }
    }
}
