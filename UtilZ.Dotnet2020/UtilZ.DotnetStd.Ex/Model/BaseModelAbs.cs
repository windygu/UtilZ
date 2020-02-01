using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace UtilZ.DotnetStd.Ex.Model
{
    /// <summary>
    /// 模型基类
    /// </summary>
    [Serializable]
    public abstract class BaseModelAbs : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 触发属性值改变事件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected virtual void OnRaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// 触发属性值改变事件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected virtual void OnRaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseModelAbs()
        {

        }
    }
}
