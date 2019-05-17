using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Log;

namespace UtilZ.Dotnet.SHPDevOpsNet
{
    /// <summary>
    /// ICE服务端
    /// </summary>
    public class DevOpsControlServer : IDisposable
    {
        /// <summary>
        /// ICE通讯对象
        /// </summary>
        private readonly Ice.Communicator _ic = null;

        /// <summary>
        /// CE监视数据/QoS参数收集处理对象
        /// </summary>
        private readonly DevOpsControlDisp _devOpsControl = null;

        /// <summary>
        /// ice连接适配器
        /// </summary>
        private Ice.ObjectAdapter _adapter;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sendCommand">处理发送命令委托</param>
        public DevOpsControlServer(Func<string, string> sendCommand)
        {
            string[] args = new string[] { };
            this._ic = Ice.Util.initialize(ref args);
            this._devOpsControl = new DevOpsControlDisp(sendCommand);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="name">ice名称</param>
        /// <param name="endpoints">ice配置终结点信息</param>
        /// <returns>返回主机host</returns>
        public string Init(string name, string endpoints)
        {
            try
            {
                this._adapter = this._ic.createObjectAdapterWithEndpoints(name, endpoints);
            }
            catch (Ice.SocketException ex)
            {
                throw ex.InnerException;
            }

            dynamic dd = this._adapter.getEndpoints()[0].getInfo();
            string host = dd.host;
            this._adapter.add(this._devOpsControl, this._ic.stringToIdentity(name));
            return host;
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

        /// <summary>
        /// 启动ICE监听[会同步阻塞]
        /// </summary>
        public void Start()
        {
            this._adapter.activate();
            this._ic.waitForShutdown();
        }

        /// <summary>
        /// 停止ICE监听
        /// </summary>
        public void Stop()
        {
            this._adapter.deactivate();
            this._ic.shutdown();
        }
    }
}
