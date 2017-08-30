using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Lib.Base.Extend;

namespace UtilZ.Lib.Base.Threading
{
    /// <summary>
    /// 执行带参数委托线程子类
    /// </summary>
    /// <typeparam name="T">参数类型</typeparam>
    internal class BreezeActionParaThread<T> : NThread
    {
        /// <summary>
        /// 要执行的带参数委托
        /// </summary>
        private Action<CancellationToken, T> _breezeActionPara;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="action">要执行的带参数委托</param>
        internal BreezeActionParaThread(Action<CancellationToken, T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => action), "要执行的委托不能为null");
            }

            this._breezeActionPara = action;
        }

        /// <summary>
        /// 重写带参数的线程方法
        /// </summary>
        /// <param name="token">线程取消通知参数</param>
        /// <param name="parameter">参数</param>
        protected override void ExcuteThreadMethodByPara(CancellationToken token, object parameter)
        {
            if (parameter == null)
            {
                this._breezeActionPara(token, default(T));
            }
            else
            {
                this._breezeActionPara(token, (T)parameter);
            }
        }
    }
}
