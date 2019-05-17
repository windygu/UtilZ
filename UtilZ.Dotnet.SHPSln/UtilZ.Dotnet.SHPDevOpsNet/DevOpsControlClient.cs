using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Log;

namespace UtilZ.Dotnet.SHPDevOpsNet
{
    /// <summary>
    /// 客户端
    /// </summary>
    public class DevOpsControlClient : IDisposable
    {
        /// <summary>
        /// 通讯对象
        /// </summary>
        private Ice.Communicator _ic = null;

        private readonly IDevOpsControlPrx _devOpsControl = null;
        /// <summary>
        /// 代理接口对象
        /// </summary>
        public IDevOpsControlPrx DevOpsControl
        {
            get { return _devOpsControl; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="conStr">连接字符串</param>
        public DevOpsControlClient(string conStr)
            : this(new string[] { }, conStr)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="conStr">连接字符串</param>
        /// <param name="args">初始化参数</param>
        public DevOpsControlClient(string[] args, string conStr)
        {
            if (args == null)
            {
                args = new string[] { };
            }

            _ic = Ice.Util.initialize(ref args);
            Ice.ObjectPrx obj = _ic.stringToProxy(conStr);
            _devOpsControl = IDevOpsControlPrxHelper.checkedCast(obj);
            if (_devOpsControl == null)
            {
                throw new ApplicationException("Invalid proxy");
            }
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        /// <param name="isDisposing">释放标识</param>
        protected virtual void Dispose(bool isDisposing)
        {
            if (this._ic != null)
            {
                try
                {
                    this._ic.destroy();
                }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                }
            }
        }
    }
}
