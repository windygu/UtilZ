using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Dotnet.Ex.AsynWait;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.WindowEx.Base.PartAsynWait.Interface;
using UtilZ.Dotnet.WindowEx.Base.PartAsynWait.Model;

namespace UtilZ.Dotnet.WindowEx.Base.PartAsynWait.Excute
{
    /// <summary>
    /// 执行异步等待基类
    /// </summary>
    /// <typeparam name="T">异步执行参数类型</typeparam>
    /// <typeparam name="TContainer">容器控件类型</typeparam>
    /// <typeparam name="TResult">异步执行返回值类型</typeparam>
    public abstract class PartAsynExcuteAbs<T, TContainer, TResult> : IAsynExcute<T, TContainer, TResult> where TContainer : class
    {
        /// <summary>
        /// 异步执行线程
        /// </summary>
        private Thread _asynExcuteThread = null;

        /// <summary>
        /// 异步执行线程取消对象
        /// </summary>
        private CancellationTokenSource _asynExcuteThreadCts = null;

        /// <summary>
        /// 异步等待执行参数
        /// </summary>
        protected PartAsynWaitPara<T, TResult> _asynWaitPara = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PartAsynExcuteAbs()
        {

        }

        /// <summary>
        /// 执行异步委托
        /// </summary>
        /// <param name="asynWaitPara">异步等待执行参数</param>
        /// <param name="containerControl">容器控件</param>
        public abstract void Excute(Model.PartAsynWaitPara<T, TResult> asynWaitPara, TContainer containerControl);

        /// <summary>
        /// 断言对象类型是IAsynWait和UserControl的子类对象类型
        /// </summary>
        /// <param name="value">要断言的对象类型</param>
        /// <param name="asynControlType">异步等待控件基类型</param>
        protected static void AssertIAsynWait(Type value, Type asynControlType)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            Type iShadeType = typeof(IPartAsynWait);
            if (value.GetInterface(iShadeType.Name) == null)
            {
                throw new Exception(string.Format("类型:{0}没有实现接口:{1}", value.Name, iShadeType.Name));
            }

            if (!value.IsSubclassOf(asynControlType))
            {
                throw new Exception(string.Format("类型:{0}不是{1}的子类型", value.Name, asynControlType.FullName));
            }
        }

        /// <summary>
        /// 根据异步等待遮罩层类型创建遮罩层
        /// </summary>
        /// <param name="shadeType">异步等待遮罩层类</param>
        /// <param name="para">异步等待UI参数</param>
        /// <returns>异步等待遮罩层类型创建遮罩层</returns>
        protected IPartAsynWait CreateAsynWaitShadeControl(Type shadeType, PartAsynWaitParaAbs para)
        {
            if (shadeType == null)
            {
                throw new Exception("没有指定自定义异步等待遮罩层类型");
            }

            IPartAsynWait ishade = (IPartAsynWait)Activator.CreateInstance(shadeType);
            ishade.Caption = para.Caption;
            ishade.Hint = para.Hint;
            ishade.IsShowCancel = para.IsShowCancel;
            return ishade;
        }

        /// <summary>
        /// 启动执行线程
        /// </summary>
        protected void StartAsynExcuteThread()
        {
            //取消执行委托
            this._asynWaitPara.AsynWait.Canceled += _excuteShade_Cancell;

            //启动滚动条动画
            this._asynWaitPara.AsynWait.StartAnimation();

            this._asynExcuteThreadCts = new CancellationTokenSource();
            this._asynExcuteThread = new Thread(this.ExcuteThreadMethod);
            this._asynExcuteThread.IsBackground = true;
            this._asynExcuteThread.Name = "UI异步执行线程";
            this._asynExcuteThread.Start();
        }

        /// <summary>
        /// UI异步执行线程方法
        /// </summary>
        private void ExcuteThreadMethod()
        {
            TResult result = default(TResult);
            PartAsynExcuteStatus excuteStatus;
            Exception excuteEx = null;
            try
            {

                var function = this._asynWaitPara.Function;
                if (function != null)
                {
                    result = function(new PartAsynFuncPara<T>(this._asynWaitPara.Para, this._asynExcuteThreadCts.Token, this._asynWaitPara.AsynWait));
                }

                if (this._asynExcuteThreadCts.Token.IsCancellationRequested)
                {
                    excuteStatus = PartAsynExcuteStatus.Cancel;
                }
                else
                {
                    excuteStatus = PartAsynExcuteStatus.Completed;
                }
            }
            catch (ThreadAbortException)
            {
                excuteStatus = PartAsynExcuteStatus.Cancel;
                this.ExcuteCompleted(result, excuteStatus, excuteEx);
                return;
            }
            catch (Exception ex)
            {
                excuteStatus = PartAsynExcuteStatus.Exception;
                excuteEx = ex;
            }

            this.ExcuteCompleted(result, excuteStatus, excuteEx);
        }

        private void ExcuteCompleted(TResult result, PartAsynExcuteStatus excuteStatus, Exception excuteEx)
        {
            var asynExcuteResult = new PartAsynExcuteResult<T, TResult>(this._asynWaitPara.Para, excuteStatus, result, excuteEx);
            //设置对象锁结束
            PartAsynUIParaProxy.UnLock(this._asynWaitPara);
            this.ReleseResource();

            this.OnRaiseCompleted(asynExcuteResult);
        }

        private void OnRaiseCompleted(PartAsynExcuteResult<T, TResult> asynExcuteResult)
        {
            if (this._asynWaitPara.AsynWait.InvokeRequired)
            {
                this._asynWaitPara.AsynWait.Invoke(new Action(() =>
                {
                    this.OnRaiseCompleted(asynExcuteResult);
                }));
            }
            else
            {
                var endAction = this._asynWaitPara.Completed;
                if (endAction != null)
                {
                    endAction(asynExcuteResult);
                }
            }
        }

        /// <summary>
        /// 取消执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _excuteShade_Cancell(object sender, EventArgs e)
        {
            this._asynExcuteThreadCts.Cancel();

            if (this._asynWaitPara.CancelAbort)
            {
                this._asynExcuteThread.Abort();
            }
        }

        /// <summary>
        /// 释放异步委托资源
        /// </summary>
        private void ReleseResource()
        {
            try
            {
                this._asynWaitPara.AsynWait.Canceled -= _excuteShade_Cancell;
                this._asynWaitPara.AsynWait.StopAnimation();

                this.PrimitiveReleseResource();
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        /// <summary>
        /// 释放异步委托资源
        /// </summary>
        protected abstract void PrimitiveReleseResource();




        #region IDisposable
        /// <summary>
        /// 是否释放标识
        /// </summary>
        private bool _isDisposed = false;

        /// <summary>
        /// 资源释放 
        /// </summary>
        public void Dispose()
        {
            if (this._isDisposed)
            {
                return;
            }

            this._isDisposed = true;
            this.Dispose(this._isDisposed);
        }

        /// <summary>
        /// 释放资源方法
        /// </summary>
        /// <param name="isDispose">是否释放标识</param>
        protected virtual void Dispose(bool isDispose)
        {

        }
        #endregion
    }
}
