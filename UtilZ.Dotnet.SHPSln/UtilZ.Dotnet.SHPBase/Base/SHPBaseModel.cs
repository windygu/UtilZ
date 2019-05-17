using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.Base
{
    [Serializable]
    public abstract class SHPBaseModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        /// <summary>
        /// 属性值改变通知事件
        /// </summary>
        [field: NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 属性值改变通知方法
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        protected void OnRaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        public SHPBaseModel()
        {

        }
    }
}
