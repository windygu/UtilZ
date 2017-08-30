using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Lib.Base.Extend;

namespace UtilZ.Lib.Base.Threading
{
    /// <summary>
    /// 执行委托线程子类
    /// </summary>
    internal class NActionThread : NThread
    {
        /// <summary>
        /// 要执行的无参数委托
        /// </summary>
        private Action<CancellationToken> _breezeAction;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="action">要执行的无参数委托</param>
        internal NActionThread(Action<CancellationToken> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => action), "要执行的委托不能为null");
            }

            this._breezeAction = action;
        }

        /// <summary>
        /// 重写无参数线程方法
        /// </summary>
        /// <param name="token">线程取消通知参数</param>
        protected override void ExcuteThreadMethod(CancellationToken token)
        {
            this._breezeAction(token);
        }
    }
}
