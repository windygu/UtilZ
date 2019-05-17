using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SHPBase.Base;

namespace UtilZ.Dotnet.SHPBase.Model
{
    [Serializable]
    public class HostTypeItem : SHPBaseModel
    {
        [Browsable(false)]
        public long Id { get; set; }

        private string _name = string.Empty;
        [DisplayName("名称")]
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.Equals(_name, value))
                {
                    return;
                }

                _name = value;
                base.OnRaisePropertyChanged(nameof(Name));
            }
        }

        private string _des = string.Empty;
        [DisplayName("描述")]
        public string Des
        {
            get { return _des; }
            set
            {
                if (string.Equals(_des, value))
                {
                    return;
                }

                _des = value;
                base.OnRaisePropertyChanged(nameof(Des));
            }
        }

        public HostTypeItem()
        {

        }

        public void Update(HostTypeItem hostTypeItem)
        {
            this.Name = hostTypeItem._name;
            this.Des = hostTypeItem._des;
        }
    }
}
