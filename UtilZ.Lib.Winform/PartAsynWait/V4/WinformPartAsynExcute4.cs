using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UtilZ.Lib.Base.Extend;
using UtilZ.Lib.Base.Log;
using UtilZ.Lib.Base.PartAsynWait.Model;
using UtilZ.Lib.Winform.Extend;

namespace UtilZ.Lib.Winform.PartAsynWait.Excute.Winform.V4
{
    /// <summary>
    /// Winfrom异步执行类
    /// </summary>
    /// <typeparam name="T">异步执行参数类型</typeparam>
    /// <typeparam name="TContainer">容器控件类型</typeparam>
    /// <typeparam name="TResult">异步执行返回值类型</typeparam>
    internal class WinformPartAsynExcute4<T, TContainer, TResult> : WinformPartAsynExcuteBase<T, TContainer, TResult> where TContainer : class
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static WinformPartAsynExcute4()
        {
            _shadeType = typeof(UCMetroShadeControl4);
        }

        /// <summary>
        /// 默认当遮罩层类型为自定义类型时用于创建遮罩层的类型
        /// </summary>
        private static Type _shadeType = null;

        /// <summary>
        /// 当遮罩层类型为自定义类型时用于创建遮罩层的类型
        /// </summary>
        public static Type ShadeType
        {
            get
            {
                return _shadeType;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                //断言对象类型是IAsynWait和UserControl的子类对象类型
                AssertIAsynWait(value);
                _shadeType = value;
            }
        }

        /// <summary>
        /// 容器控件
        /// </summary>
        private System.Windows.Forms.Control _containerControl;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WinformPartAsynExcute4()
            : base()
        {

        }

        /// <summary>
        /// 执行异步委托
        /// </summary>
        /// <param name="asynWaitPara">异步等待执行参数</param>
        /// <param name="containerControl">容器控件</param>
        public override void Excute(PartAsynWaitPara<T, TResult> asynWaitPara, TContainer containerControl)
        {
            if (asynWaitPara.AsynWait == null)
            {
                PartAsynUIParaProxy.SetAsynWait(asynWaitPara, this.CreateAsynWaitShadeControl(_shadeType, asynWaitPara));
            }

            if (asynWaitPara.Islock)
            {
                return;
            }

            lock (asynWaitPara.SyncRoot)
            {
                if (asynWaitPara.Islock)
                {
                    return;
                }

                PartAsynUIParaProxy.Lock(asynWaitPara);
            }

            var container = containerControl as Control;
            this._asynWaitPara = asynWaitPara;
            this._containerControl = container;

            //禁用容器控件内的子控件的Tab焦点选中功能
            WinformPartAsynExcuteHelper.DisableTab(container, this._asynModifyControls);

            //设置遮罩层控件
            asynWaitPara.AsynWait.AsynWaitBackground = asynWaitPara.AsynWaitBackground;

            //添加遮罩层控件
            var shadeControl = (Control)asynWaitPara.AsynWait;
            shadeControl.Dock = DockStyle.Fill;
            this.SetShadeBackgroundImg(container, shadeControl);
            container.Controls.Add(shadeControl);

            //设置遮罩层控件在最上层
            container.Controls.SetChildIndex(shadeControl, 0);
            container.SizeChanged += containerControl_SizeChanged;

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

        void containerControl_SizeChanged(object sender, EventArgs e)
        {
            this.SetShadeBackgroundImg(this._containerControl, (Control)this._asynWaitPara.AsynWait);
        }

        private void SetShadeBackgroundImg(System.Windows.Forms.Control containerControl, Control shadeControl)
        {
            shadeControl.Visible = false;
            System.Drawing.Image bitmap;
            if (containerControl is Form)
            {
                bitmap = NExtendWinfromControl.CreateFormImg(containerControl as Form);
            }
            else
            {
                bitmap = NExtendWinfromControl.CreateControlImg(containerControl);
            }

            bitmap = NExtendImg.ChangeImageOpacity(bitmap, 50);
            shadeControl.BackgroundImage = bitmap;
            shadeControl.Visible = true;
        }

        /// <summary>
        /// 执行异步委托线程方法
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
            catch (Exception ex)
            {
                excuteStatus = PartAsynExcuteStatus.Exception;
                excuteEx = ex;
            }

            var asynExcuteResult = new PartAsynExcuteResult<T, TResult>(this._asynWaitPara.Para, excuteStatus, result, excuteEx);
            //设置对象锁结束
            PartAsynUIParaProxy.UnLock(this._asynWaitPara);
            this.ReleseResource();

            var endAction = this._asynWaitPara.Completed;
            if (endAction != null)
            {
                endAction(asynExcuteResult);
            }
        }

        /// <summary>
        /// 释放异步委托资源
        /// </summary>
        private void ReleseResource()
        {
            try
            {
                var containerControl = this._containerControl;
                if (containerControl.InvokeRequired)
                {
                    containerControl.Invoke(new Action(this.ReleseResource));
                }
                else
                {
                    try
                    {
                        WinformPartAsynExcuteHelper.EnableTab(this._asynModifyControls);
                        containerControl.SizeChanged -= containerControl_SizeChanged;
                        containerControl.Controls.Remove((Control)this._asynWaitPara.AsynWait);
                        this._asynWaitPara.AsynWait.Canceled -= _excuteShade_Cancell;
                        this._asynWaitPara.AsynWait.StopAnimation();
                    }
                    catch (Exception exi)
                    {
                        Loger.Error(exi);
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        /// <summary>
        /// 取消执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _excuteShade_Cancell(object sender, EventArgs e)
        {
            this._asynExcuteThreadCts.Cancel();
        }
    }
}
