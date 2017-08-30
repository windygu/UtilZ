﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Lib.Base.PartAsynWait.Interface;
using UtilZ.Lib.Winform.PartAsynWait.Interface;

namespace UtilZ.Lib.Winform.PartAsynWait.Excute.Winform.V3
{
    public partial class UCMetroShadeControl3 : UserControl, IPartAsynWaitWinform
    {
        #region IPartAsynWait接口
        /// <summary>
        /// 是否已经取消
        /// </summary>
        private bool _isCanceled = false;

        /// <summary>
        /// 获取是否已经取消
        /// </summary>
        [Browsable(false)]
        public bool IsCanceled
        {
            get { return this._isCanceled; }
        }

        /// <summary>
        /// 获取或设置提示标题
        /// </summary>
        [Category("异步等待")]
        [DisplayName("提示标题")]
        [Description("获取或设置提示标题")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public string Caption
        {
            get
            {
                return labelControlCaption.Text;
            }
            set
            {
                labelControlCaption.Text = value;
            }
        }

        /// <summary>
        /// 获取或设置提示内容
        /// </summary>
        [Category("异步等待")]
        [DisplayName("提示内容")]
        [Description("获取或设置提示内容")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public string Hint
        {
            get
            {
                return labelControlTitle.Text;
            }
            set
            {
                labelControlTitle.Text = value;
            }
        }

        /// <summary>
        /// 获取或设置是否显示取消按钮
        /// </summary>
        [Category("异步等待")]
        [DisplayName("是否显示取消按钮")]
        [Description("获取或设置是否显示取消按钮")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public bool IsShowCancel
        {
            get { return btnCancell.Visible; }
            set { btnCancell.Visible = value; }
        }

        /// <summary>
        /// 获取或设置动画背景色
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("获取或设置动画背景色")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color AsynWaitBackground
        {
            get { return this.BackColor; }
            set { this.BackColor = value; }
        }

        /// <summary>
        /// 取消事件
        /// </summary>
        public event EventHandler Canceled;

        /// <summary>
        /// 线程锁
        /// </summary>
        private readonly object _monitor = new object();

        /// <summary>
        /// 取消操作
        /// </summary>
        public void Cancel()
        {
            if (this._isCanceled)
            {
                return;
            }

            lock (this._monitor)
            {
                if (this._isCanceled)
                {
                    return;
                }

                this._isCanceled = true;
            }

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    btnCancell.Text = "正在取消";
                    btnCancell.Enabled = false;
                }));
            }
            else
            {
                btnCancell.Text = "正在取消";
                btnCancell.Enabled = false;
            }

            var handler = this.Canceled;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        /// <summary>
        /// 开始动画
        /// </summary>
        public void StartAnimation()
        {
            //metroRotaionIndicator.StartAnimal();
        }

        /// <summary>
        /// 停止动画
        /// </summary>
        public void StopAnimation()
        {
            //metroRotaionIndicator.StopAnimal();
        }

        /// <summary>
        /// 设置信息(保留接口)
        /// </summary>
        /// <param name="para">参数</param>
        public void SetInfo(object para)
        {

        }

        /// <summary>
        /// 重置异步等待框
        /// </summary>
        public void Reset()
        {
            this._isCanceled = false;
            btnCancell.Text = "取消";
            btnCancell.Enabled = true;
        }

        /// <summary>
        /// 获取UI是否处于设计器模式
        /// </summary>
        public bool UIDesignMode
        {
            get { return this.DesignMode; }
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public UCMetroShadeControl3()
        {
            InitializeComponent();
        }

        private void UCMetroShadeControl2_Load(object sender, EventArgs e)
        {

        }

        private void btnCancell_Click(object sender, EventArgs e)
        {
            this.Cancel();
        }
    }
}
